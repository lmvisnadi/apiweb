using Data.Context;
using Infraestructure.Pagination;
using Infraestructure.Response;
using MediatR;

namespace Application.QuerySide.ListMunicipio
{
    public class ListMunicipioHandler : IRequestHandler<ListMunicipioQuery, NewResponse<Page<ListMunicipioViewModel>>>
    {
        private readonly SharedContext _context;

        public ListMunicipioHandler(SharedContext context)
        {
            _context = context;
        }

        public async Task<NewResponse<Page<ListMunicipioViewModel>>> Handle(ListMunicipioQuery request,
            CancellationToken cancellationToken)
        {
            var municipios = _context.Municipios
                .OrderBy(p => p.Nome)
                .Select(p => new ListMunicipioViewModel(
                    p.Id,
                    p.Nome,
                    new ListMunicipioUnidadeFederativaViewModel(
                        p.UnidadeFederativa.Id,
                        p.UnidadeFederativa.Nome,
                        p.UnidadeFederativa.Sigla))
                )
                .Paginate(request.PaginationOptions);

            return new OkResponse<Page<ListMunicipioViewModel>>(municipios);
        }
    }
}
