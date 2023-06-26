using Application.QuerySide.ListUnidadeFederativa;

namespace apiClienteEndereco.Payload.UnidadeFederativa
{
    public record UnidadeFederativaListPayload
    {
        public UnidadeFederativaListPayload()
        {

        }
        public ListUnidadeFederativaQuery AsQuery()
            => new();
    }
}
