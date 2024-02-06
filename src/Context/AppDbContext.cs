using Microsoft.EntityFrameworkCore;
using rinha_de_backend_2024_q1.Entities;

namespace rinha_de_backend_2024_q1.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Saldo> Saldos { get; set; }

    }
}
