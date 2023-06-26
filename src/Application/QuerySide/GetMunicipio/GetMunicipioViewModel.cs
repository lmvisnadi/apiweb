namespace Application.QuerySide.GetMunicipio
{
    public class GetMunicipioViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; private set; }
        public Guid UnidadeFederativaId { get; private set; }

        public GetMunicipioViewModel(Guid id, string nome, Guid unidadeFederativaId)
        {
            Id = id;
            Nome = nome;
            UnidadeFederativaId = unidadeFederativaId;
        }
    }
}
