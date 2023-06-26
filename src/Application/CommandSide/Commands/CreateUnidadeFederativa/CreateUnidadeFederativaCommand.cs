using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.CreateUnidadeFederativa
{
    public class CreateUnidadeFederativaCommand : IRequest<NewResponse<Guid>>
    {
        public string Sigla { get; init; }
        public string Nome { get; init; }
        public CreateUnidadeFederativaCommand(string sigla, string nome)
        {
            Sigla = sigla;
            Nome = nome;
        }
    }
}
