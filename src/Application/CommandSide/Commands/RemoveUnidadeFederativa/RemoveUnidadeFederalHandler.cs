using dominio.Repositories;
using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.RemoveUnidadeFederativa
{
    public class RemoveUnidadeFederativaHandler : IRequestHandler<RemoveUnidadeFederativaCommand, NewResponse<Unit>>
    {
        private readonly IUnidadeFederativaRepository _unidadeFederativaRepository;
        public RemoveUnidadeFederativaHandler(IUnidadeFederativaRepository unidadeFederativaRepository)
        {
            _unidadeFederativaRepository = unidadeFederativaRepository;
        }
        public async Task<NewResponse<Unit>> Handle(RemoveUnidadeFederativaCommand request, CancellationToken cancellationToken)
        {
            var validation = new RemoveUnidadeFederativaValidator().Validate(request);
            if (!validation.IsValid)
                return new UnprocessableResponse<Unit>(validation.Errors.Select(x => x.ErrorMessage));

            var unidadeFederativa = await _unidadeFederativaRepository.GetAsync(request.Id);
            if (unidadeFederativa is null)
                return new NotFoundResponse<Unit>("unidadeFederativaNaoEncontrada");

            _unidadeFederativaRepository.Remove(unidadeFederativa);
            await _unidadeFederativaRepository.SaveAsync(cancellationToken);

            return new NoContentResponse<Unit>();
        }
    }
}
