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

        private void AbrirAdmin_Click(object sender, RoutedEventArgs e)
        {
            var janela = new AdminPizzas();
            janela.ShowDialog();
        }

        private void Sair_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
