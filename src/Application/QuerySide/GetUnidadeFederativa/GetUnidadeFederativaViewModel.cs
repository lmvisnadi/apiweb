namespace Application.QuerySide.GetUnidadeFederativa
{
    public class GetUnidadeFederativaViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }

        public GetUnidadeFederativaViewModel(Guid id, string nome, string sigla)
        {
            Id = id;
            Nome = nome;
            Sigla = sigla;
        }
    }
}
