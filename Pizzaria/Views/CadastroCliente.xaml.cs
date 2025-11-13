using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Pizzaria.Models;

namespace Pizzaria.Views
{
    public partial class CadastroCliente : Window
    {
        private readonly PizzariaContext _context = new PizzariaContext();
        private List<ItemPedido> itensPedido = new List<ItemPedido>();
        private List<Pizza> pizzasDisponiveis = new List<Pizza>();

        public CadastroCliente()
        {
            InitializeComponent();
            CarregarPizzas();
            AtualizarCarrinho();
            AtualizarPedidos();
        }

        private void CarregarPizzas()
        {
            pizzasDisponiveis = _context.Pizzas.ToList();
            dgPizzasCliente.ItemsSource = pizzasDisponiveis;
        }

        private void AdicionarAoPedido_Click(object sender, RoutedEventArgs e)
        {
            if (!(dgPizzasCliente.SelectedItem is Pizza pizzaSelecionada))
            {
                MessageBox.Show("Selecione uma pizza do cardápio.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(txtQuantidade.Text, out int quantidade) || quantidade <= 0)
            {
                MessageBox.Show("Quantidade inválida.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var item = new ItemPedido
            {
                Pizza = pizzaSelecionada,
                PizzaId = pizzaSelecionada.Id,
                Quantidade = quantidade
            };

            itensPedido.Add(item);
            AtualizarCarrinho();
        }

        private void AtualizarCarrinho()
        {
            dgCarrinho.ItemsSource = null;
            dgCarrinho.ItemsSource = itensPedido;
            decimal total = itensPedido.Sum(i => i.Subtotal);
            txtTotal.Text = total.ToString("C2");
        }

        private void LimparCarrinho_Click(object sender, RoutedEventArgs e)
        {
            itensPedido.Clear();
            AtualizarCarrinho();
        }

        // AGORA ESTE É O MÉTODO CORRETO DE FINALIZAR PEDIDO (com tela de pagamento)
        private void FinalizarPedido_Click(object sender, RoutedEventArgs e)
        {
            string nome = txtNomeCliente.Text?.Trim();
            string endereco = txtEnderecoCliente.Text?.Trim();
            string telefone = txtTelefoneCliente.Text?.Trim();

            if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(telefone))
            {
                MessageBox.Show("Preencha nome e telefone do cliente.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (itensPedido.Count == 0)
            {
                MessageBox.Show("Adicione pelo menos uma pizza ao pedido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var cliente = new Cliente
            {
                Nome = nome,
                Endereco = endereco,
                Telefone = telefone
            };

            string tipoEntrega = rbEntregar.IsChecked == true ? "Entrega" : "Retirada";

            // 👉 Abre a tela de pagamento ANTES de salvar o pedido
            var telaPagamento = new Pagamento(cliente, new List<ItemPedido>(itensPedido), tipoEntrega);
            telaPagamento.Owner = this;
            bool? resultado = telaPagamento.ShowDialog();

            if (resultado == true)
            {
                // Atualiza a tela depois do pagamento concluído
                itensPedido.Clear();
                AtualizarCarrinho();
                AtualizarPedidos();
            }
        }

        private void AtualizarPedidos()
        {
            dgPedidos.ItemsSource = null;
            dgPedidos.ItemsSource = _context.Pedidos
                .OrderByDescending(p => p.Data)
                .Take(20)
                .ToList();
        }
    }
}
