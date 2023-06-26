using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.CreateMunicipio
{
    public class CreateMunicipioCommand : IRequest<NewResponse<Guid>>
    {
        public string Nome { get; private set; }
        public Guid UnidadeFederativaId { get; private set; }
        public CreateMunicipioCommand(string nome, Guid unidadeFederativaId)
        {
            Nome = nome;
            UnidadeFederativaId = unidadeFederativaId;
        }
    }
}
