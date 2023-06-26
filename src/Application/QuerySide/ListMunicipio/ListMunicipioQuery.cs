using Infraestructure.Pagination;
using Infraestructure.Response;
using MediatR;

namespace Application.QuerySide.ListMunicipio
{
    public struct ListMunicipioQuery : IRequest<NewResponse<Page<ListMunicipioViewModel>>>
    {
        public Guid? UnidadeFederativaId { get; set; }
        public string? Nome { get; init; }
        public PaginationOptions? PaginationOptions { get; init; }

        public ListMunicipioQuery(Guid? unidadeFederativaId, string? nome, PaginationOptions? paginationOptions)
        {
            UnidadeFederativaId = unidadeFederativaId;
            Nome = nome;
            PaginationOptions = paginationOptions ?? new PaginationOptions();
        }
    }
}
