using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.RemoveUnidadeFederativa
{
    public struct RemoveUnidadeFederativaCommand : IRequest<NewResponse<Unit>>
    {
        public Guid Id { get; init; }

        public RemoveUnidadeFederativaCommand(Guid id)
        {
            Id = id;
        }
    }
}
