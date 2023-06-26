namespace Application.QuerySide.ListMunicipio
{
    public record ListMunicipioUnidadeFederativaViewModel
    {
        public Guid Id { get; init; }
        public string Nome { get; init; }
        public string Sigla { get; init; }

        public ListMunicipioUnidadeFederativaViewModel(Guid id, string nome, string sigla)
        {
            Id = id;
            Nome = nome;
            Sigla = sigla;
        }
    }
}
