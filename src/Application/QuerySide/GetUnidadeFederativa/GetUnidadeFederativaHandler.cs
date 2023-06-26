using Data.Context;
using Infraestructure.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.QuerySide.GetUnidadeFederativa
{
    public class GetUnidadeFederativaHandler : IRequestHandler<GetUnidadeFederativaQuery, NewResponse<GetUnidadeFederativaViewModel>>
    {
        private readonly SharedContext _context;
        public GetUnidadeFederativaHandler(SharedContext context)
        {
            _context = context;
        }

        public async Task<NewResponse<GetUnidadeFederativaViewModel>> Handle(GetUnidadeFederativaQuery request, CancellationToken cancellationToken)
        {
            var unidadeFederativa = await _context.UnidadesFederativas
                                                    .FirstOrDefaultAsync(p => p.Id == request.Id);
            if (unidadeFederativa is null)
                return new NotFoundResponse<GetUnidadeFederativaViewModel>("unidadeFederativaNaoEncontrada");

            var ufViewModel = new GetUnidadeFederativaViewModel(unidadeFederativa.Id, unidadeFederativa.Nome, unidadeFederativa.Sigla); ;
            return new OkResponse<GetUnidadeFederativaViewModel>(ufViewModel);

        }
    }
}
