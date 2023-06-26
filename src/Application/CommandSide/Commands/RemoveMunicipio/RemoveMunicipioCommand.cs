using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.RemoveMuncipio
{
    public class RemoveMunicipioCommand : IRequest<NewResponse<Unit>>
    {
        public Guid Id { get; set; }

        public RemoveMunicipioCommand(Guid id)
        {
            Id = id;
        }
    }
}
