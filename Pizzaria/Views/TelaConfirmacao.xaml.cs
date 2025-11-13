using System;
using System.Windows;
using Pizzaria.Models;

namespace Pizzaria.Views
{
    public partial class TelaConfirmacao : Window
    {
        public TelaConfirmacao(Pedido pedido)
        {
            InitializeComponent();

    
            int tempoEstimado = pedido.ItensPedido.Count * 20;

            txtMensagem.Text = $"Pedido nº {pedido.Id} confirmado com sucesso!";
            txtTempoEntrega.Text = $"⏰ Tempo estimado de entrega: {tempoEstimado} minutos";

          
            if (pedido.Status == "Retirada")
                txtTempoEntrega.Text = $"⏰ Seu pedido estará pronto em {tempoEstimado} minutos para retirada.";
        }

        private void Fechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
