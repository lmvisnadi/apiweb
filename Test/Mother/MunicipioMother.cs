using dominio.Entities;

namespace Test.Mother
{
    public static class MunicipioMother
    {
        public static Municipio Create(string nome, UnidadeFederativa unidadeFederativa)
        {
            return new(nome, unidadeFederativa);
        }
        public static Municipio Jaragua()
        {
            return Create("Jaragua", UnidadeFederativaMother.SantaCatarina());
        }
        public static Municipio Santos()
        {
            return Create("Santos", UnidadeFederativaMother.SantaCatarina());
        }
        public static Municipio Curitiba()
        {
            return Create("Curitiba", UnidadeFederativaMother.Parana());
        }

        public static IEnumerable<Municipio> PrepareMunicipios()
        {
            var _unidadeFederativa = UnidadeFederativaMother.SantaCatarina();

            var jaragua = Jaragua();
            jaragua.ChangeUnidadeFederativa(_unidadeFederativa);
            var santos = Santos();
            santos.ChangeUnidadeFederativa(_unidadeFederativa);
            var curitiba = Curitiba();
            curitiba.ChangeUnidadeFederativa(_unidadeFederativa);
            return new List<Municipio>
            {
                jaragua,
                santos,
                curitiba
            };
        }
    }
}
