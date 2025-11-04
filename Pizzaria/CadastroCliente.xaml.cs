using System.Linq;
using System.Windows;
using Pizzaria.Models;

namespace Pizzaria.Views
{
    public partial class CadastroCliente : Window
    {
        private PizzariaContext _context;
        private Cliente _clienteSelecionado;

        public CadastroCliente()
        {
            InitializeComponent();
            _context = new PizzariaContext();
            ListarClientes();
        }

        private void ListarClientes()
        {
            dgClientes.ItemsSource = _context.Clientes.ToList();
        }

        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            var novoCliente = new Cliente
            {
                Nome = txtNome.Text,
                Telefone = txtTelefone.Text,
                Endereco = txtEndereco.Text
            };
            
            _context.Clientes.Add(novoCliente);
            _context.SaveChanges();
            MessageBox.Show("Cliente cadastrado com sucesso!");
            LimparCampos();
            ListarClientes();
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (_clienteSelecionado != null)
            {
                _clienteSelecionado.Nome = txtNome.Text;
                _clienteSelecionado.Telefone = txtTelefone.Text;
                _clienteSelecionado.Endereco = txtEndereco.Text;

                _context.Clientes.Update(_clienteSelecionado);
                _context.SaveChanges();

                MessageBox.Show("Cliente atualizado com sucesso!");
                LimparCampos();
                ListarClientes();
            }
            else
            {
                MessageBox.Show("Selecione um cliente para editar.");
            }
        }

        private void Excluir_Click(object sender, RoutedEventArgs e)
        {
            if (_clienteSelecionado != null)
            {
                if (MessageBox.Show("Deseja excluir este cliente?", "Confirmação", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    _context.Clientes.Remove(_clienteSelecionado);
                    _context.SaveChanges();
                    MessageBox.Show("Cliente excluído com sucesso!");
                    LimparCampos();
                    ListarClientes();
                }
            }
            else
            {
                MessageBox.Show("Selecione um cliente para excluir.");
            }
        }

        private void dgClientes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _clienteSelecionado = dgClientes.SelectedItem as Cliente;
            if (_clienteSelecionado != null)
            {
                txtNome.Text = _clienteSelecionado.Nome;
                txtTelefone.Text = _clienteSelecionado.Telefone;
                txtEndereco.Text = _clienteSelecionado.Endereco;
            }
        }

        private void LimparCampos()
        {
            txtNome.Text = "";
            txtTelefone.Text = "";
            txtEndereco.Text = "";
            _clienteSelecionado = null;
        }
    }
}
