using dominio.Entities;
using dominio.Repositories;
using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.CreateUnidadeFederativa
{
    public class CreateUnidadeFederativaHandler : IRequestHandler<CreateUnidadeFederativaCommand, NewResponse<Guid>>
    {
        private IUnidadeFederativaRepository _unidadeFederativaRepository;
        public CreateUnidadeFederativaHandler(IUnidadeFederativaRepository unidadeFederativaRepository)
        {
            _unidadeFederativaRepository = unidadeFederativaRepository;
        }

        public async Task<NewResponse<Guid>> Handle(CreateUnidadeFederativaCommand request, CancellationToken cancellationToken)
        {
            var validation = new CreateUnidadeFederativaValidator().Validate(request);
            if (!validation.IsValid)
                return new UnprocessableResponse<Guid>(validation.Errors.Select(x => x.ErrorMessage));

            var existeCodigo = await _unidadeFederativaRepository.AnyAsync(p => p.Sigla == request.Sigla);
            if (existeCodigo)
                return new NotFoundResponse<Guid>("siglaJaExistente");

            var unidadeFederativa = new UnidadeFederativa(request.Sigla, request.Nome);
            await _unidadeFederativaRepository.InsertAsync(unidadeFederativa);
            await _unidadeFederativaRepository.SaveAsync(cancellationToken);

            return new CreatedResponse<Guid>(unidadeFederativa.Id);
        }
    }
}
