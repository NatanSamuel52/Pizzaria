using Pizzaria.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Pizzaria.Views
{
    public partial class AdminPizzas : Window
    {
        private PizzariaContext _context = new PizzariaContext();
        private Pizza pizzaSelecionada;

        public AdminPizzas()
        {
            InitializeComponent();
            CarregarPizzas();
        }

        private void CarregarPizzas()
        {
            dgPizzasAdmin.ItemsSource = _context.Pizzas.ToList();
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomePizza.Text) || string.IsNullOrWhiteSpace(txtPrecoPizza.Text))
            {
                MessageBox.Show("Preencha nome e preço.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecoPizza.Text, out decimal preco))
            {
                MessageBox.Show("Preço inválido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var pizza = new Pizza
            {
                Nome = txtNomePizza.Text,
                Preco = preco,
                Categoria = ((ComboBoxItem)cbCategoriaPizza.SelectedItem).Content.ToString()
            };

            _context.Pizzas.Add(pizza);
            _context.SaveChanges();
            CarregarPizzas();
            LimparCampos();
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (pizzaSelecionada == null)
            {
                MessageBox.Show("Selecione uma pizza para editar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrecoPizza.Text, out decimal preco))
            {
                MessageBox.Show("Preço inválido.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            pizzaSelecionada.Nome = txtNomePizza.Text;
            pizzaSelecionada.Preco = preco;
            pizzaSelecionada.Categoria = ((ComboBoxItem)cbCategoriaPizza.SelectedItem).Content.ToString();

            _context.Pizzas.Update(pizzaSelecionada);
            _context.SaveChanges();
            CarregarPizzas();
            LimparCampos();
        }

        private void Excluir_Click(object sender, RoutedEventArgs e)
        {
            if (pizzaSelecionada == null)
            {
                MessageBox.Show("Selecione uma pizza para excluir.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Deseja excluir esta pizza?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                _context.Pizzas.Remove(pizzaSelecionada);
                _context.SaveChanges();
                CarregarPizzas();
                LimparCampos();
            }
        }

        private void dgPizzasAdmin_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            pizzaSelecionada = dgPizzasAdmin.SelectedItem as Pizza;
            if (pizzaSelecionada != null)
            {
                txtNomePizza.Text = pizzaSelecionada.Nome;
                txtPrecoPizza.Text = pizzaSelecionada.Preco.ToString("F2");
                cbCategoriaPizza.Text = pizzaSelecionada.Categoria;
            }
        }

        private void LimparCampos()
        {
            txtNomePizza.Text = "";
            txtPrecoPizza.Text = "";
            cbCategoriaPizza.SelectedIndex = 0;
            pizzaSelecionada = null;
            dgPizzasAdmin.UnselectAll();
        }
    }
}
