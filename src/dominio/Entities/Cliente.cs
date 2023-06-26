using Infraestructure.Domain.Base;

namespace dominio.Entities
{
    public class Cliente : EntityBase
    {
        public string Nome { get; private set; }
        public string Documento { get; private set; }
        public virtual IReadOnlyCollection<Endereco> Enderecos => _endereco;
        private List<Endereco> _endereco = new();
        protected Cliente()
        {
        }

        public Cliente(string nome, string documeto)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Documento = documeto;
        }

        public void ChangeNome(string nome)
                => Nome = nome;

        public void ChangeDocumento(string documento)
                => Documento = documento;

        public void AddEndereco(Endereco endereco)
            => _endereco.Add(endereco);
        public void ChangeEndereco(Endereco endereco)
        {
            _endereco
                .Where(e => e.Id == endereco.Id)
                .First().ChangeEndereco(
                    endereco.TipoEndereco,
                    endereco.Cep,
                    endereco.Logradouro,
                    endereco.Numero,
                    endereco.Complemento,
                    endereco.Bairro,
                    endereco.UnidadeFederativa,
                    endereco.Municipio
                    );
        }

        public void RemoveEndereco(Endereco endereco)
        {
            if (_endereco.Any(e => e.Id == endereco.Id))
                _endereco.Remove(endereco);
        }

    }
}
