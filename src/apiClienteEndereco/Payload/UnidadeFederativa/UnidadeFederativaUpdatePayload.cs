using Application.CommandSide.Commands.UpdateUnidadeFederativa;

namespace apiClienteEndereco.Payload.UnidadeFederativa
{
    public struct UnidadeFederativaUpdatePayload
    {
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public UnidadeFederativaUpdatePayload(string nome, string sigla)
        {
            Nome = nome;
            Sigla = sigla;
        }
        public UpdateUnidadeFederativaCommand AsCommand(Guid id)
            => new(id, Nome, Sigla);
    }
}
