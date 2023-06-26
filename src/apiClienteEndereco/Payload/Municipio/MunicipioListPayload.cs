using Application.QuerySide.ListMunicipio;
using Infraestructure.Pagination;

namespace apiClienteEndereco.Payload.Municipio
{
    public record MunicipioListPayload
    {
        public Guid? UnidadeFederativaId { get; init; }
        public string? Nome { get; init; }
        public MunicipioListPayload()
        {
        }
        public MunicipioListPayload(string? nome)
        {
            Nome = nome;
        }
        public ListMunicipioQuery AsQuery(PaginationOptions? paginationOptions)
            => new(UnidadeFederativaId, Nome, paginationOptions);
    }
}
