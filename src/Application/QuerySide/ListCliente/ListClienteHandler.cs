using Data.Context;
using Infraestructure.Pagination;
using Infraestructure.Response;
using MediatR;

namespace Application.QuerySide.ListCliente
{
    public class
    ListClienteHandler : IRequestHandler<ListClienteQuery, NewResponse<Page<ListClienteViewModel>>>
    {
        private readonly SharedContext _context;

        public ListClienteHandler(SharedContext context)
        {
            _context = context;
        }
        public async Task<NewResponse<Page<ListClienteViewModel>>> Handle(ListClienteQuery request,
            CancellationToken cancellationToken)
        {
            var Cliente = _context.Clientes
                .AsQueryable()
                .Select(p => new ListClienteViewModel(
                    p.Id,
                    p.Nome,
                    p.Documento
                )).Paginate(request.PaginationOptions);

            return new OkResponse<Page<ListClienteViewModel>>(Cliente);
        }
    }
}
