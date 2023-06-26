using Application.CommandSide.Commands.UpdateMunicipio;

namespace apiClienteEndereco.Payload.Municipio
{
    public class MunicipioUpdatePayload
    {
        public string Nome { get; private set; }
        public Guid UnidadeFederativaId { get; private set; }

        public MunicipioUpdatePayload(string nome, Guid unidadeFederativaId)
        {
            Nome = nome;
            UnidadeFederativaId = unidadeFederativaId;
        }
        public UpdateMunicipioCommand AsCommand(Guid id)
            => new(id, Nome, UnidadeFederativaId);
    }
}
