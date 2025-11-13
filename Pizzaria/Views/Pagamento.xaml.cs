using System.Linq;
using System.Windows;
using Pizzaria.Models;
using System.Collections.Generic;

namespace Pizzaria.Views
{
    public partial class Pagamento : Window
    {
        private Cliente? clienteSelecionado;
        private string? formaEntrega;
        private List<ItemPedido>? itensSelecionados;

        public Pagamento()
        {
            InitializeComponent();
        }

        public Pagamento(Cliente cliente, List<ItemPedido> itens, string entregaOuRetirada)
        {
            InitializeComponent();
            clienteSelecionado = cliente;
            itensSelecionados = itens;
            formaEntrega = entregaOuRetirada;
        }

        private void rbDinheiro_Checked(object sender, RoutedEventArgs e)
        {
            if (spTroco != null) spTroco.Visibility = Visibility.Visible;
        }

        private void ConfirmarPedido_Click(object sender, RoutedEventArgs e)
        {
            string formaPagamento = string.Empty;
            decimal troco = 0m;

            if (rbDinheiro != null && rbDinheiro.IsChecked == true)
            {
                formaPagamento = "Dinheiro";
                decimal.TryParse(txtTroco?.Text ?? "0", out troco);
            }
            else if (rbCartao != null && rbCartao.IsChecked == true)
            {
                formaPagamento = "Cartão";
            }
            else if (rbPix != null && rbPix.IsChecked == true)
            {
                formaPagamento = "Pix";
            }
            else
            {
                MessageBox.Show("Selecione uma forma de pagamento!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var itens = itensSelecionados ?? new List<ItemPedido>();
            decimal total = itens.Sum(i => i.Subtotal);

            using (var context = new PizzariaContext())
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteSelecionado?.Id ?? 0,
                    Total = total,
                    Status = formaEntrega ?? "Retirar",
                    TipoEntrega = formaEntrega ?? "Retirar",
                    Data = System.DateTime.Now
                };

                if (clienteSelecionado != null && clienteSelecionado.Id == 0)
                {
                    context.Clientes.Add(clienteSelecionado);
                    context.SaveChanges();
                    pedido.ClienteId = clienteSelecionado.Id;
                }

                foreach (var item in itens)
                {
                    item.Pizza = null; 
                }

                context.Pedidos.Add(pedido);
                context.SaveChanges();

                foreach (var it in itens)
                {
                    context.ItensPedido.Add(new ItemPedido
                    {
                        PedidoId = pedido.Id,
                        PizzaId = it.PizzaId,
                        Quantidade = it.Quantidade
                    });
                }

                context.SaveChanges();

                MessageBox.Show(
                    $"Pedido #{pedido.Id} criado com sucesso!\nTotal: {pedido.Total:C2}\nForma de pagamento: {formaPagamento}",
                    "Sucesso",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }

            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
