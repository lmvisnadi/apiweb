using Infraestructure.Response;
using MediatR;

namespace Application.QuerySide.GetUnidadeFederativa
{
    public class GetUnidadeFederativaQuery : IRequest<NewResponse<GetUnidadeFederativaViewModel>>
    {
        public Guid Id { get; set; }

        public GetUnidadeFederativaQuery(Guid id)
        {
            Id = id;
        }
    }
}
