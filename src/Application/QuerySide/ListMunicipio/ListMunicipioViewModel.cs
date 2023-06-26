namespace Application.QuerySide.ListMunicipio
{
    public record ListMunicipioViewModel
    {
        public Guid Id { get; init; }
        public string Nome { get; init; }
        public ListMunicipioUnidadeFederativaViewModel UnidadeFederativa { get; init; }

        public ListMunicipioViewModel(
            Guid id,
            string nome,
            ListMunicipioUnidadeFederativaViewModel unidadeFederativa)
        {
            Id = id;
            Nome = nome;
            UnidadeFederativa = unidadeFederativa;
        }
    }
}
