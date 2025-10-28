namespace Pizzaria.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }

        // Relação: 1 Cliente -> N Pedidos
        public ICollection<Pedido> Pedidos { get; set; }
    }
}
