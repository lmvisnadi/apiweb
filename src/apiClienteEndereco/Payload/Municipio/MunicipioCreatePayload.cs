using Application.CommandSide.Commands.CreateMunicipio;

namespace apiClienteEndereco.Payload.Municipio
{
    public class MunicipioCreatePayload
    {
        public string Nome { get; private set; }
        public Guid UnidadeFederativaId { get; private set; }

        public MunicipioCreatePayload(string nome, Guid unidadeFederativaId)
        {
            Nome = nome;
            UnidadeFederativaId = unidadeFederativaId;
        }
        public CreateMunicipioCommand AsCommand()
            => new(Nome, UnidadeFederativaId);
    }
}
