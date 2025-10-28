namespace Pizzaria.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string Status { get; set; } // Ex: "Em preparo", "Entregue"

        // Relação com o Cliente
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        // Relação: 1 Pedido -> N ItensPedido
        public ICollection<ItemPedido> ItensPedido { get; set; }
    }
}
