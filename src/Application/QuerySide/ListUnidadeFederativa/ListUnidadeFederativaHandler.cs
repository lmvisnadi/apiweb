using Data.Context;
using Infraestructure.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.QuerySide.ListUnidadeFederativa
{
    public class ListUnidadeFederativaHandler : IRequestHandler<ListUnidadeFederativaQuery, NewResponse<IEnumerable<ListUnidadeFederativaViewModel>>>
    {
        private readonly SharedContext _context;
        public ListUnidadeFederativaHandler(SharedContext context)
        {
            _context = context;
        }
        public async Task<NewResponse<IEnumerable<ListUnidadeFederativaViewModel>>> Handle(ListUnidadeFederativaQuery request, CancellationToken cancellationToken)
        {
            var unidadesFederativas = await _context.UnidadesFederativas
                                        .OrderBy(p => p.Nome)
                                        .Select(p => new ListUnidadeFederativaViewModel(p.Id, p.Nome, p.Sigla))
                                        .ToListAsync();

            return new OkResponse<IEnumerable<ListUnidadeFederativaViewModel>>(unidadesFederativas);

        }
    }
}
