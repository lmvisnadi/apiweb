using dominio.Enums;

namespace dominio.Entities
{
    public class Endereco
    {
        public Guid Id { get; private set; }
        public Guid ClienteId { get; private set; }
        public EnumTipoEndereco TipoEndereco { get; private set; }
        public string Cep { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public Guid UnidadeFederativaId { get; private set; }
        public virtual UnidadeFederativa UnidadeFederativa { get; private set; }
        public Guid MunicipioId { get; private set; }
        public virtual Municipio Municipio { get; private set; }

        protected Endereco() { }

        public Endereco(
            Guid clienteId,
            EnumTipoEndereco tipoEndereco,
            string cep,
            string logradouro,
            string numero,
            string complemento,
            string bairro,
            UnidadeFederativa unidadeFederativa,
            Municipio municipio
        )
        {
            Id = Guid.NewGuid();
            ClienteId = clienteId;
            TipoEndereco = tipoEndereco;
            Cep = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            UnidadeFederativa = unidadeFederativa;
            UnidadeFederativaId = unidadeFederativa.Id;
            Municipio = municipio;
            MunicipioId = municipio.Id;
        }

        public void ChangeEndereco(
            EnumTipoEndereco tipoEndereco,
            string cep,
            string logradouro,
            string numero,
            string complemento,
            string bairro,
            UnidadeFederativa unidadeFederativa,
            Municipio municipio
        )
        {
            TipoEndereco = tipoEndereco;
            Cep = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            UnidadeFederativa = unidadeFederativa;
            UnidadeFederativaId = unidadeFederativa.Id;
            Municipio = municipio;
            MunicipioId = municipio.Id;
        }


    }
}
