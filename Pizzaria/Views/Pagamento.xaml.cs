using System.Windows;

namespace Pizzaria.Views
{
    public partial class Pagamento : Window
    {
        public string FormaPagamento { get; private set; } = string.Empty;
        public decimal Troco { get; private set; }

        public Pagamento()
        {
            InitializeComponent();
        }

        private void rbDinheiro_Checked(object sender, RoutedEventArgs e)
        {
            spTroco.Visibility = Visibility.Visible;
        }

        private void ConfirmarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (rbDinheiro.IsChecked == true)
            {
                FormaPagamento = "Dinheiro";
                if (decimal.TryParse(txtTroco.Text, out var troco))
                    Troco = troco;
            }
            else if (rbCartao.IsChecked == true)
            {
                FormaPagamento = "Cartão";
            }
            else if (rbPix.IsChecked == true)
            {
                FormaPagamento = "Pix";
            }
            else
            {
                MessageBox.Show("Selecione uma forma de pagamento!");
                return;
            }

            DialogResult = true;
            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
