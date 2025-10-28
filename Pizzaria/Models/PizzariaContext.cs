using Microsoft.EntityFrameworkCore;

namespace Pizzaria.Models
{
    public class PizzariaContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Usa o SQL Server LocalDB (instalado junto com o Visual Studio)
                optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PizzariaDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relacionamento entre Pedido e ItemPedido
            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Pedido)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(i => i.PedidoId);

            // Relacionamento entre Pizza e ItemPedido
            modelBuilder.Entity<ItemPedido>()
                .HasOne(i => i.Pizza)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(i => i.PizzaId);

            // Relacionamento entre Cliente e Pedido
            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClienteId);
        }
    }
}
