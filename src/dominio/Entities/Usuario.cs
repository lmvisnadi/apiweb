using Infraestructure.Domain.Base;
using Infraestructure.Token;

namespace dominio.Entities
{
    public class Usuario : EntityBase
    {
        public string Nome { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string Senha { get; private set; } = null!;
        public bool FlagAtivo { get; private set; } = true;

        protected Usuario() { }
        public Usuario(
            string nome,
            string email,
            string senha,
            bool flagAtivo
        )
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Email = email;
            Senha = MD5Geral.CriptogragarMD5(senha);
            FlagAtivo = flagAtivo;
        }
        public void ChangeNome(string nome)
            => Nome = nome;
        public void ChangeEmail(string email)
            => Email = email;
        public void ChangeSenha(string senha)
            => Senha = MD5Geral.CriptogragarMD5(senha);
        public void ChangeFlagAtivo(bool flagAtivo)
            => FlagAtivo = flagAtivo;

    }
}
