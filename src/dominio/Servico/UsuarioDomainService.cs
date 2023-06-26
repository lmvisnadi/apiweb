using dominio.Entities;
using dominio.Repositories;

namespace dominio.Servico
{
    public class UsuarioDomainService : IUsuarioDomainService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioDomainService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public async Task<Usuario?> GetAsync(string email, string senha)
        {
            return await _repository.GetUsuarioByLoginAsync(email, senha);
        }

        public async Task<bool> IsUniqueAsync(Usuario usuarioMustBeVerified, string email)
        {
            var userFromDatabase = await _repository.FindAsync(p => p.Email == email);

            return userFromDatabase == null || usuarioMustBeVerified.Id == userFromDatabase.Id;
        }
    }
}
