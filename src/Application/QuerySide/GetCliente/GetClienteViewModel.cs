namespace Application.QuerySide.GetCliente
{
    public class GetClienteViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Documento { get; set; }
        public List<GetEnderecoViewModel>? Enderecos { get; set; }
        public GetClienteViewModel(Guid id,
                                   string nome,
                                   string documento,
                                   List<GetEnderecoViewModel>? enderecos
                                   )
        {
            Id = id;
            Nome = nome;
            Documento = documento;
            Enderecos = enderecos;
        }
    }
}
