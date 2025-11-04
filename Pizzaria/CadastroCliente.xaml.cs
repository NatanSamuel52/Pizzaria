using System;
using System.Linq;
using System.Windows;
using Pizzaria.Models;

namespace Pizzaria.Views
{
    public partial class CadastroCliente : Window
    {
        private PizzariaContext _context = new PizzariaContext();
        private Cliente clienteSelecionado;

        public CadastroCliente()
        {
            InitializeComponent();
            CarregarClientes();
        }

        private void CarregarClientes()
        {
            dgClientes.ItemsSource = _context.Clientes.ToList();
        }

        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                    string.IsNullOrWhiteSpace(txtTelefone.Text) ||
                    string.IsNullOrWhiteSpace(txtEndereco.Text))
                {
                    MessageBox.Show("Preencha todos os campos!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var novoCliente = new Cliente
                {
                    Nome = txtNome.Text,
                    Telefone = txtTelefone.Text,
                    Endereco = txtEndereco.Text
                };

                _context.Clientes.Add(novoCliente);
                _context.SaveChanges();

                MessageBox.Show("Cliente cadastrado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                LimparCampos();
                CarregarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar cliente: " + ex.Message);
            }
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clienteSelecionado == null)
                {
                    MessageBox.Show("Selecione um cliente para editar!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                clienteSelecionado.Nome = txtNome.Text;
                clienteSelecionado.Telefone = txtTelefone.Text;
                clienteSelecionado.Endereco = txtEndereco.Text;

                _context.Clientes.Update(clienteSelecionado);
                _context.SaveChanges();

                MessageBox.Show("Cliente atualizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                LimparCampos();
                CarregarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao editar cliente: " + ex.Message);
            }
        }

        private void Excluir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (clienteSelecionado == null)
                {
                    MessageBox.Show("Selecione um cliente para excluir!", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show("Tem certeza que deseja excluir este cliente?", "Confirmação",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    _context.Clientes.Remove(clienteSelecionado);
                    _context.SaveChanges();

                    MessageBox.Show("Cliente excluído com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    LimparCampos();
                    CarregarClientes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir cliente: " + ex.Message);
            }
        }

        private void dgClientes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            clienteSelecionado = dgClientes.SelectedItem as Cliente;
            if (clienteSelecionado != null)
            {
                txtNome.Text = clienteSelecionado.Nome;
                txtTelefone.Text = clienteSelecionado.Telefone;
                txtEndereco.Text = clienteSelecionado.Endereco;
            }
        }

        private void LimparCampos()
        {
            txtNome.Text = "";
            txtTelefone.Text = "";
            txtEndereco.Text = "";
            clienteSelecionado = null;
            dgClientes.UnselectAll();
        }
    }
}
