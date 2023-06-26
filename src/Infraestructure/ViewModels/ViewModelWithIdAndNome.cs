namespace Infraestructure.ViewModels
{
    public struct ViewModelWithIdAndNome
    {
        public Guid Id { get; init; }
        public string Nome { get; init; }
        public ViewModelWithIdAndNome(
            Guid id,
            string nome
        )
        {
            Id = id;
            Nome = nome;
        }
    }

}
