using Infraestructure.Domain.Base;

namespace dominio.Entities
{
    public class UnidadeFederativa : EntityBase
    {
        public string Sigla { get; private set; }
        public string Nome { get; private set; }

        public UnidadeFederativa(string sigla, string nome)
        {
            Id = Guid.NewGuid();
            Sigla = sigla;
            Nome = nome;
        }
        public void ChangeSigla(string sigla)
        {
            Sigla = sigla;
        }
        public void ChangeNome(string nome)
        {
            Nome = nome;
        }
    }

}
