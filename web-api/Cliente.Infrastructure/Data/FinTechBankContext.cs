using Cliente.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cliente.Infrastructure.Data
{
    public class FinTechBankContext : DbContext
    {
        public FinTechBankContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Genero> Generos => Set<Genero>();
        public DbSet<TipoCliente> TiposCliente => Set<TipoCliente>();
        public DbSet<Entities.Cliente> Clientes => Set<Entities.Cliente>();
        public DbSet<Usuario> Usuarios => Set<Usuario>();
    }
}