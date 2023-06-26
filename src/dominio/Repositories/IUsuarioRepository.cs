using dominio.Entities;
using Infraestructure.Domain.Repositories;

namespace dominio.Repositories
{
    public interface IUsuarioRepository : IEntityBaseRepository<Usuario>
    {
        Task<Usuario?> GetUsuarioByLoginAsync(string email, string senha);
    }
}
