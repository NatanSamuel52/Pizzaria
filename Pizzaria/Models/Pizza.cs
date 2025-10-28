namespace Pizzaria.Models
{
    public class Pizza
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public string Categoria { get; set; } // Tradicional, Especial, Doce

        // Relação: 1 Pizza -> N ItensPedido
        public ICollection<ItemPedido> ItensPedido { get; set; }
    }
}
