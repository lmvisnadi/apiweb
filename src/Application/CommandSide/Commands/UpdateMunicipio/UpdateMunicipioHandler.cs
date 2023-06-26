using dominio.Repositories;
using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.UpdateMunicipio
{
    public class UpdateMunicipioHandler : IRequestHandler<UpdateMunicipioCommand, NewResponse<Unit>>
    {
        private readonly IMunicipioRepository _municipioRepository;
        private readonly IUnidadeFederativaRepository _ufRepository;

        public UpdateMunicipioHandler(IMunicipioRepository municipioRepository, IUnidadeFederativaRepository ufRepository)
        {
            _municipioRepository = municipioRepository;
            _ufRepository = ufRepository;
        }

        public async Task<NewResponse<Unit>> Handle(UpdateMunicipioCommand request, CancellationToken cancellationToken)
        {
            var validation = new UpdateMunicipioValidator().Validate(request);
            if (!validation.IsValid)
                return new UnprocessableResponse<Unit>(validation.Errors.Select(x => x.ErrorMessage));

            var municipio = await _municipioRepository.GetAsync(request.Id);
            if (municipio is null)
                return new NotFoundResponse<Unit>("municipioNaoEncontrado");

            var unidadeFederativa = await _ufRepository.GetAsync(request.UnidadeFederativaId);
            if (unidadeFederativa == null)
                return new NotFoundResponse<Unit>("unidadeFederativaInexistente");

            municipio.ChangeUnidadeFederativa(unidadeFederativa!);
            municipio.ChangeNome(request.Nome);

            await _municipioRepository.SaveAsync(cancellationToken);

            return new NoContentResponse<Unit>();
        }
    }
}
