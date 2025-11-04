using System;
using System.Collections.Generic;

namespace Pizzaria.Models
{
    public class Pedido
    {
        public int Id { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public decimal Total { get; set; }
        public string Status { get; set; } 
        public string TipoEntrega { get; set; } 

        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
    }
}
