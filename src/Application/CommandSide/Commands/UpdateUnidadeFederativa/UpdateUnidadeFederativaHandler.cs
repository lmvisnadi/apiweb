using dominio.Repositories;
using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.UpdateUnidadeFederativa
{
    public class UpdateUnidadeFederativaHandler : IRequestHandler<UpdateUnidadeFederativaCommand, NewResponse<Unit>>
    {
        private readonly IUnidadeFederativaRepository _unidadeFederativaRepository;

        public UpdateUnidadeFederativaHandler(IUnidadeFederativaRepository unidadeFederativaRepository)
        {
            _unidadeFederativaRepository = unidadeFederativaRepository;
        }
        public async Task<NewResponse<Unit>> Handle(UpdateUnidadeFederativaCommand request, CancellationToken cancellationToken)
        {
            var validation = new UpdateUnidadeFederativaValidator().Validate(request);
            if (!validation.IsValid)
                return new UnprocessableResponse<Unit>(validation.Errors.Select(x => x.ErrorMessage));

            var unidadeFederativa = await _unidadeFederativaRepository.GetAsync(request.Id);
            if (unidadeFederativa is null)
                return new NotFoundResponse<Unit>("unidadeFederativaNaoEncontrada");

            if (request.Sigla is not null)
                unidadeFederativa.ChangeSigla(request.Sigla);

            if (request.Nome is not null)
                unidadeFederativa.ChangeNome(request.Nome);

            await _unidadeFederativaRepository.SaveAsync(cancellationToken);

            return new NoContentResponse<Unit>();

        }
    }
}
