using System.Windows;
using Pizzaria.Views;

namespace Pizzaria
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AbrirClientes_Click(object sender, RoutedEventArgs e)
        {
            var janela = new CadastroCliente();
            janela.ShowDialog();
        }

        private void AbrirPizzas_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tela de Pizzas ainda não implementada!");
        }

        private void AbrirPedidos_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tela de Pedidos ainda não implementada!");
        }

        private void AbrirRelatorios_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tela de Relatórios ainda não implementada!");
        }
    }
}
