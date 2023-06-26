namespace Application.QuerySide.ListCliente
{
    public struct ListClienteViewModel
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public string Documento { get; private set; }
        public ListClienteViewModel(Guid id,
                                    string nome,
                                    string documento)
        {
            Id = id;
            Nome = nome;
            Documento = documento;
        }
    }
}
