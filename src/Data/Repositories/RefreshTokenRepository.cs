using Data.Context;
using dominio.Entities;
using dominio.Repositories;
using Infraestructure.Repository;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class RefreshTokenRepository : EntityBaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(SharedContext context) : base(context)
        {
        }

        public async Task<RefreshToken?> GetByTokenAndUserAsync(Guid refreshToken, Guid usuarioId)
        {
            return await DbSet.IgnoreQueryFilters().FirstOrDefaultAsync(r => r.Token == refreshToken && r.UsuarioId == usuarioId && r.DataValidade >= DateTime.UtcNow);
        }

        public override void Remove(RefreshToken item)
        {
            _context.Remove(item);
        }
    }
}
