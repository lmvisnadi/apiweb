using Data.Context;
using dominio.Entities;
using dominio.Repositories;
using Infraestructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UsuarioRepository : EntityBaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(SharedContext context) : base(context)
        {
        }

        public async Task<Usuario?> GetUsuarioByLoginAsync(string email, string senha)
        {
            return await DbSet.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Email == email && p.Senha == senha);
        }
    }
}
