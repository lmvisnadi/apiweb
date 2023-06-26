using Application.CommandSide.Commands.CreateCliente;
using Application.CommandSide.Commands.DTOs.Endereco;

namespace apiClienteEndereco.Payload.Cliente
{
    public struct ClienteCreatePayload
    {
        public string Nome { get; init; }
        public string Documento { get; set; }
        public List<EnderecoCreatePayload> Enderecos { get; set; }

        public ClienteCreatePayload(
            string nome,
            string documento,
            List<EnderecoCreatePayload> enderecos
            )
        {
            Nome = nome;
            Documento = documento;
            Enderecos = enderecos;
        }
        public CreateClienteCommand AsCommand()
            => new(
                nome: Nome,
                documento: Documento,
                enderecos: Enderecos
                    .Select(e => new InsertEnderecoDTO(
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
