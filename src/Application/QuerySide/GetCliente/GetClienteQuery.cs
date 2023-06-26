using Infraestructure.Response;
using MediatR;

namespace Application.QuerySide.GetCliente
{
    public struct GetClienteQuery : IRequest<NewResponse<GetClienteViewModel>>
    {
        public Guid Id { get; init; }

        public GetClienteQuery(Guid id)
        {
            Id = id;
        }
    }
}
