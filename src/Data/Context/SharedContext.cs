using dominio.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Infraestructure.Context;

namespace Data.Context
{
    public class SharedContext : BaseContext
    {
        public SharedContext(
            DbContextOptions<BaseContext> options,
            Guid usuarioId
        ) : base(options, usuarioId)
        {
        }
        public DbSet<UnidadeFederativa> UnidadesFederativas => Set<UnidadeFederativa>();
        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Municipio> Municipios => Set<Municipio>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
