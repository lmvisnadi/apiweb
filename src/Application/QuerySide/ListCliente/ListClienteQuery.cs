using Infraestructure.Pagination;
using Infraestructure.Response;
using MediatR;

namespace Application.QuerySide.ListCliente
{

    public struct ListClienteQuery : IRequest<NewResponse<Page<ListClienteViewModel>>>
    {
        public PaginationOptions? PaginationOptions { get; init; }

        public ListClienteQuery(PaginationOptions? paginationOptions)
        {
            PaginationOptions = paginationOptions ?? new PaginationOptions();
        }
    }
}
