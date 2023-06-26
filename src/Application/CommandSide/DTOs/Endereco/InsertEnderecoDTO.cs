using dominio.Enums;

namespace Application.CommandSide.Commands.DTOs.Endereco
{
    public struct InsertEnderecoDTO
    {
        public EnumTipoEndereco TipoEndereco { get; init; }
        public string Cep { get; init; }
        public string Logradouro { get; init; }
        public string Numero { get; init; }
        public string Complemento { get; init; }
        public string Bairro { get; init; }
        public Guid UnidadeFederativaId { get; init; }
        public Guid MunicipioId { get; init; }

        public InsertEnderecoDTO(
                EnumTipoEndereco tipoEndereco,
                string cep,
                string logradouro,
                string numero,
                string complemento,
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
