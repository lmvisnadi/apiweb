using dominio.Entities;

namespace Test.Mother
{
    public static class UnidadeFederativaMother
    {
        public static UnidadeFederativa Create(string sigla, string nome)
        {
            return new(sigla, nome);
        }
        public static UnidadeFederativa SantaCatarina()
        {
            return Create("SC", "Santa Catarina");
        }
        public static UnidadeFederativa SaoPaulo()
        {
            return Create("SP", "São Paulo");
        }
        public static UnidadeFederativa Parana()
        {
            return Create("PR", "Paraná");
        }
        public static List<UnidadeFederativa> ListaUnidadeFederativas()
        {
            List<UnidadeFederativa> listaUnidades = new()
            {
                SantaCatarina(),
                SaoPaulo()
            };

            return listaUnidades;
        }
    }
}
