using System.Collections.Generic;

namespace Pizzaria.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}