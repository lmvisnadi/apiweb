using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.UpdateMunicipio
{
    public class UpdateMunicipioCommand : IRequest<NewResponse<Unit>>
    {
        public Guid Id { get; set; }
        public string Nome { get; private set; }
        public Guid UnidadeFederativaId { get; private set; }

        public UpdateMunicipioCommand(Guid id, string nome, Guid unidadeFederativaId)
        {
            Id = id;
            Nome = nome;
            UnidadeFederativaId = unidadeFederativaId;
        }
    }
}
