using dominio.Enums;

namespace apiClienteEndereco.Payload.Cliente
{
    public class EnderecoCreatePayload
    {
        public EnumTipoEndereco TipoEndereco { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string? Complemento { get; set; }
        public string Bairro { get; set; }
        public Guid UnidadeFederativaId { get; set; }
        public Guid MunicipioId { get; set; }

        public EnderecoCreatePayload(
        EnumTipoEndereco tipoEndereco,
        string cep,
        string logradouro,
        string numero,
        string? complemento,
        string bairro,
        Guid unidadeFederativaId,
        Guid municipioId
    )
        {
            TipoEndereco = tipoEndereco;
            Cep = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            UnidadeFederativaId = unidadeFederativaId;
            MunicipioId = municipioId;
        }
    }
}
