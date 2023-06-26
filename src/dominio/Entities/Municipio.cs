using Infraestructure.Domain.Base;

namespace dominio.Entities
{
    public class Municipio : EntityBase
    {
        public string Nome { get; private set; } = null!;
        public Guid UnidadeFederativaId { get; private set; }
        public virtual UnidadeFederativa UnidadeFederativa { get; private set; } = null!;

        protected Municipio() { }

        public Municipio(string nome, UnidadeFederativa unidadeFederativa)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            UnidadeFederativa = unidadeFederativa;
            UnidadeFederativaId = unidadeFederativa.Id;
        }
        public void ChangeNome(string nome)
        {
            Nome = nome;
        }
        public void ChangeUnidadeFederativa(UnidadeFederativa unidadeFederativa)
        {
            UnidadeFederativa = unidadeFederativa;
            UnidadeFederativaId = unidadeFederativa.Id;
        }

    }
}
