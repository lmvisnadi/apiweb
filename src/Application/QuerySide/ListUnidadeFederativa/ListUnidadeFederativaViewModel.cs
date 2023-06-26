namespace Application.QuerySide.ListUnidadeFederativa
{
    public class ListUnidadeFederativaViewModel
    {
        public Guid Id { get; init; }
        public string Nome { get; init; }
        public string Sigla { get; init; }
        public ListUnidadeFederativaViewModel(Guid id, string nome, string sigla)
        {
            Id = id;
            Nome = nome;
            Sigla = sigla;
        }
    }
}
