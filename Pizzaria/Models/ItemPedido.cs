namespace Pizzaria.Models
{
    public class ItemPedido
    {
        public int Id { get; set; }

        // Relação com Pedido
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; }

        // Relação com Pizza
        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; }

        public int Quantidade { get; set; }

        // Subtotal calculado (não mapeado no banco)
        public decimal Subtotal => Quantidade * (Pizza?.Preco ?? 0);
    }
}
