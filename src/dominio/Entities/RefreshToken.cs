using Infraestructure.Domain.Base;

namespace dominio.Entities
{
    public class RefreshToken : EntityBase
    {
        public RefreshToken(Guid usuarioId, DateTime dataValidade)
        {
            Id = Guid.NewGuid();
            Token = Guid.NewGuid();
            UsuarioId = usuarioId;
            DataValidade = dataValidade;
        }

        public Guid Token { get; private set; }
        public DateTime DataValidade { get; private set; }
        public Guid UsuarioId { get; private set; }

    }
}
