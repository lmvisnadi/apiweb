using dominio.Entities;
using Infraestructure.Domain.Repositories;

namespace dominio.Repositories
{
    public interface IRefreshTokenRepository : IEntityBaseRepository<RefreshToken>
    {
        public Task<RefreshToken?> GetByTokenAndUserAsync(Guid refreshToken, Guid usuarioId);
    }
}
