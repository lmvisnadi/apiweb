using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.UpdateUnidadeFederativa
{
    public class UpdateUnidadeFederativaCommand : IRequest<NewResponse<Unit>>
    {
        public Guid Id { get; init; }
        public string Nome { get; init; }
        public string Sigla { get; init; }

        public UpdateUnidadeFederativaCommand(Guid id, string nome, string sigla)
        {
            Id = id;
            Nome = nome;
            Sigla = sigla;
        }
    }
}

