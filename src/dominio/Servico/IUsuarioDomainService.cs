using dominio.Entities;

namespace dominio.Servico
{
    public interface IUsuarioDomainService
    {
        Task<bool> IsUniqueAsync(Usuario userMustBeVerified, string email);
        Task<Usuario?> GetAsync(string email, string password);
    }
}
