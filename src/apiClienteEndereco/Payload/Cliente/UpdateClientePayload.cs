using Application.CommandSide.Commands.DTOs.Endereco;
using Application.CommandSide.Commands.UpdateCliente;

namespace apiClienteEndereco.Payload.Cliente
{
    public struct ClienteUpdatePayload
    {
        public string Nome { get; init; }
        public string Documento { get; init; }
        public List<EnderecoUpdatePayload> Enderecos { get; init; }

        public ClienteUpdatePayload(
            string nome,
            string documento,
            List<EnderecoUpdatePayload> enderecos
            )
        {
            Nome = nome;
            Documento = documento;
            Enderecos = enderecos;
        }
        public UpdateClienteCommand AsCommand(Guid id)
            => new(
                id,
                Nome,
                Documento,
                Enderecos.Select(e => new UpdateEnderecoDTO(
                                                e.Id,
                                                e.TipoEndereco,
                                                e.Cep,
                                                e.Logradouro,
                                                e.Numero,
                                                e.Complemento ?? string.Empty,
                                                e.Bairro,
                                                e.UnidadeFederativaId,
                                                e.MunicipioId
                                                )).ToList()
                );
    }
}
