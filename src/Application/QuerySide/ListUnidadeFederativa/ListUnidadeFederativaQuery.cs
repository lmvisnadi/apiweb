using Infraestructure.Response;
using MediatR;

namespace Application.QuerySide.ListUnidadeFederativa
{
    public class ListUnidadeFederativaQuery : IRequest<NewResponse<IEnumerable<ListUnidadeFederativaViewModel>>>
    {
    }
}
