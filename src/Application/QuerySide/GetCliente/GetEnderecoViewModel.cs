using dominio.Enums;
using Infraestructure.ViewModels;

namespace Application.QuerySide.GetCliente
{
    public struct GetEnderecoViewModel
    {
        public Guid Id { get; init; }
        public EnumTipoEndereco TipoEndereco { get; init; }
        public string Cep { get; init; }
        public string Logradouro { get; init; }
        public string Numero { get; init; }
        public string Complemento { get; init; }
        public string Bairro { get; init; }
        public ViewModelWithIdAndNome UnidadeFederativa { get; init; }
        public ViewModelWithIdAndNome Municipio { get; init; }

        public GetEnderecoViewModel(
            Guid id,
            EnumTipoEndereco tipoEndereco,
            string cep,
            string logradouro,
            string numero,
            string complemento,
            string bairro,
            ViewModelWithIdAndNome unidadeFederativa,
            ViewModelWithIdAndNome municipio
        )
        {
            Id = id;
            TipoEndereco = tipoEndereco;
            Cep = cep;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            UnidadeFederativa = unidadeFederativa;
            Municipio = municipio;
        }

    }
}
