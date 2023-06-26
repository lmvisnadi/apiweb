using Infraestructure.Response;
using MediatR;

namespace Application.QuerySide.GetMunicipio
{
    public class GetMunicipioQuery : IRequest<NewResponse<GetMunicipioViewModel>>
    {
        public Guid Id { get; set; }

        public GetMunicipioQuery(Guid id)
        {
            Id = id;
        }
    }
}
