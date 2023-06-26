using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.RemoveCliente
{
    public class RemoveClienteCommand : IRequest<NewResponse<Unit>>
    {
        public Guid Id { get; set; }

        public RemoveClienteCommand(Guid id)
        {
            Id = id;
        }
    }
}
