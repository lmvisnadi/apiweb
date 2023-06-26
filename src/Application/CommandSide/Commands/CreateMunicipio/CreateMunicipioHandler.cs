using dominio.Entities;
using dominio.Repositories;
using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.CreateMunicipio
{
    public class CreateMunicipioHandler : IRequestHandler<CreateMunicipioCommand, NewResponse<Guid>>
    {
        private readonly IMunicipioRepository _municipioRepository;
        private readonly IUnidadeFederativaRepository _unidadeFederativaRepository;

        public CreateMunicipioHandler(IMunicipioRepository municipioRepository, IUnidadeFederativaRepository unidadeFederativaRepository)
        {
            _municipioRepository = municipioRepository;
            _unidadeFederativaRepository = unidadeFederativaRepository;
        }

        public async Task<NewResponse<Guid>> Handle(CreateMunicipioCommand request, CancellationToken cancellationToken)
        {
            var validation = new CreateMunicipioValidator().Validate(request);

            if (!validation.IsValid)
                return new UnprocessableResponse<Guid>(validation.Errors.Select(x => x.ErrorMessage));

            var unidadeFederativa = await _unidadeFederativaRepository.GetAsync(request.UnidadeFederativaId);
            if (unidadeFederativa == null)
                return new NotFoundResponse<Guid>("unidadeFederativaInexistente");

            var municipio = new Municipio(request.Nome, unidadeFederativa);

            await _municipioRepository.InsertAsync(municipio);
            await _municipioRepository.SaveAsync(cancellationToken);

            return new CreatedResponse<Guid>(municipio.Id);
        }
    }
}
