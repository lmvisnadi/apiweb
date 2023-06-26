using Application.CommandSide.Commands.DTOs.Endereco;
using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.UpdateCliente
{
    public struct UpdateClienteCommand : IRequest<NewResponse<Unit>>
    {
        public Guid Id { get; init; }
        public string Nome { get; init; }
        public string Documento { get; init; }
        public List<UpdateEnderecoDTO> Enderecos { get; init; }
        public UpdateClienteCommand(Guid id, string nome, string documento, List<UpdateEnderecoDTO> enderecos)
        {
            Id = id;
            Nome = nome;
            Documento = documento;
            Enderecos = enderecos;
        }
    }
}
