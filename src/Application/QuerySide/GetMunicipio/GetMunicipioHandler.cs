using Data.Context;
using Infraestructure.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.QuerySide.GetMunicipio
{
    public class GetMunicipioHandler : IRequestHandler<GetMunicipioQuery, NewResponse<GetMunicipioViewModel>>
    {
        private readonly SharedContext _context;
        public GetMunicipioHandler(SharedContext context)
        {
            _context = context;
        }

        public async Task<NewResponse<GetMunicipioViewModel>> Handle(GetMunicipioQuery request, CancellationToken cancellationToken)
        {
            var validation = new GetMunicipioValidator().Validate(request);
            if (!validation.IsValid)
                return new UnprocessableResponse<GetMunicipioViewModel>(validation.Errors.Select(x => x.ErrorMessage));

            var municipio = await _context.Municipios
                .FirstOrDefaultAsync(p => p.Id == request.Id);
            if (municipio is null)
                return new NotFoundResponse<GetMunicipioViewModel>("municipioNaoEncontrado");

            var municipioViewModel = new GetMunicipioViewModel(municipio.Id, municipio.Nome, municipio.UnidadeFederativaId);
            return new OkResponse<GetMunicipioViewModel>(municipioViewModel);
        }
    }
}
