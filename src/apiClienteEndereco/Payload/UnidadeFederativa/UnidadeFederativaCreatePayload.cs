using Application.CommandSide.Commands.CreateUnidadeFederativa;

namespace apiClienteEndereco.Payload.UnidadeFederativa
{
    public struct UnidadeFederativaCreatePayload
    {
        public string Sigla { get; set; }
        public string Nome { get; set; }

        public UnidadeFederativaCreatePayload(string sigla, string nome)
        {
            Sigla = sigla;
            Nome = nome;
        }

        public CreateUnidadeFederativaCommand AsCommand()
            => new(Sigla, Nome);
    }
}
