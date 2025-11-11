using System.Linq;
using System.Windows;
using Pizzaria.Models;

namespace Pizzaria.Views
{
    public partial class Pagamento : Window
    {
        private Cliente clienteSelecionado;
        private string formaEntrega; 
        private System.Collections.Generic.List<ItemPedido> itensSelecionados;

        public Pagamento(Cliente cliente, System.Collections.Generic.List<ItemPedido> itens, string entregaOuRetirada)
        {
            InitializeComponent();
            clienteSelecionado = cliente;
            itensSelecionados = itens;
            formaEntrega = entregaOuRetirada;
        }

        private void rbDinheiro_Checked(object sender, RoutedEventArgs e)
        {
            spTroco.Visibility = Visibility.Visible;
        }

        private void ConfirmarPedido_Click(object sender, RoutedEventArgs e)
        {
            string formaPagamento = string.Empty;
            decimal troco = 0;

            if (rbDinheiro.IsChecked == true)
            {
                formaPagamento = "Dinheiro";
                if (decimal.TryParse(txtTroco.Text, out var valor))
                    troco = valor;
            }
            else if (rbCartao.IsChecked == true)
            {
                formaPagamento = "Cartão";
            }
            else if (rbPix.IsChecked == true)
            {
                formaPagamento = "Pix";
            }
            else
            {
                MessageBox.Show("Selecione uma forma de pagamento!", "Atenção", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            
            using (var context = new PizzariaContext())
            {
                var pedido = new Pedido
                {
                    ClienteId = clienteSelecionado.Id,
                    ItensPedido = itensSelecionados,
                    Total = itensSelecionados.Sum(i => i.Subtotal),
                    Status = formaEntrega 
                };

                context.Pedidos.Add(pedido);
                context.SaveChanges();

          
                var telaConfirmacao = new TelaConfirmacao(pedido);
                telaConfirmacao.ShowDialog();
            }

            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
