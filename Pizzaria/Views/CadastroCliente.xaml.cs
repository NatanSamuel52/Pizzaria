using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Pizzaria.Models;
using Pizzaria.Views; 

namespace Pizzaria.Views
{
    public partial class CadastroCliente : Window
    {
        private readonly PizzariaContext _context = new PizzariaContext();
        private readonly ObservableCollection<ItemPedido> carrinho = new ObservableCollection<ItemPedido>();

        public CadastroCliente()
        {
            InitializeComponent();
            dgCarrinho.ItemsSource = carrinho;
            CarregarPizzas();
            CarregarPedidos();
            AtualizarTotal();
        }

        private void CarregarPizzas()
        {
            dgPizzasCliente.ItemsSource = _context.Pizzas.ToList();
        }

        private void CarregarPedidos()
        {
            dgPedidos.ItemsSource = _context.Pedidos
                .OrderByDescending(p => p.Data)
                .Take(20)
                .ToList();
        }

        private void AdicionarAoPedido_Click(object sender, RoutedEventArgs e)
        {
            var pizza = dgPizzasCliente.SelectedItem as Pizza;
            if (pizza == null)
            {
                MessageBox.Show("Selecione uma pizza do cardápio.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtQuantidade.Text, out int qtd) || qtd <= 0)
            {
                MessageBox.Show("Quantidade inválida.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var item = new ItemPedido
            {
                Pizza = pizza,
                PizzaId = pizza.Id,
                Quantidade = qtd
            };

            carrinho.Add(item);
            AtualizarTotal();
        }

        private void AtualizarTotal()
        {
            decimal total = carrinho.Sum(i => i.Subtotal);
            txtTotal.Text = $"R$ {total:F2}";
        }

        private void LimparCarrinho_Click(object sender, RoutedEventArgs e)
        {
            carrinho.Clear();
            AtualizarTotal();
        }

        private void FinalizarPedido_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (carrinho.Count == 0)
                {
                    MessageBox.Show("Carrinho vazio.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtNomeCliente.Text) || string.IsNullOrWhiteSpace(txtTelefoneCliente.Text))
                {
                    MessageBox.Show("Preencha nome e telefone do cliente.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var janelaPagamento = new Pagamento();
                if (janelaPagamento.ShowDialog() != true)
                    return;

                var formaPagamento = janelaPagamento.FormaPagamento;
                var troco = janelaPagamento.Troco;

                var cliente = _context.Clientes.FirstOrDefault(c => c.Telefone == txtTelefoneCliente.Text);
                if (cliente == null)
                {
                    cliente = new Cliente
                    {
                        Nome = txtNomeCliente.Text,
                        Telefone = txtTelefoneCliente.Text,
                        Endereco = txtEnderecoCliente.Text
                    };
                    _context.Clientes.Add(cliente);
                    _context.SaveChanges();
                }
                else
                {
                    cliente.Nome = txtNomeCliente.Text;
                    cliente.Endereco = txtEnderecoCliente.Text;
                    _context.Clientes.Update(cliente);
                    _context.SaveChanges();
                }

                var pedido = new Pedido
                {
                    ClienteId = cliente.Id,
                    Cliente = cliente,
                    TipoEntrega = rbEntregar.IsChecked == true ? "Entregar" : "Retirar",
                    Status = "Recebido",
                    Data = DateTime.Now,
                    Total = carrinho.Sum(i => i.Subtotal)
                };
                _context.Pedidos.Add(pedido);
                _context.SaveChanges();

                foreach (var item in carrinho)
                {
                    var ip = new ItemPedido
                    {
                        PedidoId = pedido.Id,
                        PizzaId = item.PizzaId,
                        Quantidade = item.Quantidade
                    };
                    _context.ItensPedido.Add(ip);
                }

                _context.SaveChanges();

                MessageBox.Show(
                    $"Pedido #{pedido.Id} criado com sucesso!\n" +
                    $"Total: R$ {pedido.Total:F2}\n" +
                    $"Pagamento: {formaPagamento}" +
                    (formaPagamento == "Dinheiro" && troco > 0 ? $"\nTroco para: R$ {troco:F2}" : ""),
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                carrinho.Clear();
                AtualizarTotal();
                CarregarPedidos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao finalizar pedido: " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
