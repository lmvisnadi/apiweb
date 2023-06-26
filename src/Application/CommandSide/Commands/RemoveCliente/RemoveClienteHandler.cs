using dominio.Repositories;
using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.RemoveCliente
{
    public class RemoveClienteHandler : IRequestHandler<RemoveClienteCommand, NewResponse<Unit>>
    {
        private readonly IClienteRepository _ClienteRepository;

        public RemoveClienteHandler(IClienteRepository ClienteRepository)
        {
            _ClienteRepository = ClienteRepository;
        }
        public async Task<NewResponse<Unit>> Handle(RemoveClienteCommand request, CancellationToken cancellationToken)
        {
            var validation = new RemoveClienteValidator().Validate(request);

            if (!validation.IsValid)
                return new UnprocessableResponse<Unit>(validation.Errors.Select(x => x.ErrorMessage));

            var Cliente = await _ClienteRepository.GetAsync(request.Id);
            if (Cliente is null)
                return new NotFoundResponse<Unit>("clienteNaoEncontrado");

            _ClienteRepository.Remove(Cliente);
            await _ClienteRepository.SaveAsync(cancellationToken);

            return new NoContentResponse<Unit>();
        }
    }
}
