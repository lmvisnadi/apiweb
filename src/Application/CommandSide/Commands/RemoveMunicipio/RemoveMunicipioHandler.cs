using dominio.Repositories;
using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.RemoveMuncipio
{
    public class RemoveMunicipioHandler : IRequestHandler<RemoveMunicipioCommand, NewResponse<Unit>>
    {
        private readonly IMunicipioRepository _municipioRepository;

        public RemoveMunicipioHandler(IMunicipioRepository municipioRepository)
        {
            _municipioRepository = municipioRepository;
        }
        public async Task<NewResponse<Unit>> Handle(RemoveMunicipioCommand request, CancellationToken cancellationToken)
        {
            var validation = new RemoveMunicipioValidator().Validate(request);

            if (!validation.IsValid)
                return new UnprocessableResponse<Unit>(validation.Errors.Select(x => x.ErrorMessage));

            var municipio = await _municipioRepository.GetAsync(request.Id);
            if (municipio is null)
                return new NotFoundResponse<Unit>("municipioNaoEncontrado");

            _municipioRepository.Remove(municipio);
            await _municipioRepository.SaveAsync(cancellationToken);

            return new NoContentResponse<Unit>();
        }
    }
}
