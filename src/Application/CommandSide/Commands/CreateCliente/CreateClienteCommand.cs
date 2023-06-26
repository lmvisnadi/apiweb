using Application.CommandSide.Commands.DTOs.Endereco;
using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.CreateCliente
{
    public struct CreateClienteCommand : IRequest<NewResponse<Guid>>
    {
        public string Nome { get; init; }
        public string Documento { get; init; }
        public List<InsertEnderecoDTO> Enderecos { get; init; }

        public CreateClienteCommand(string nome, string documento, List<InsertEnderecoDTO> enderecos)
        {
            Nome = nome;
            Documento = documento;
            Enderecos = enderecos;
        }
    }
}

