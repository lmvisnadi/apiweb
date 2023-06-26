using dominio.Entities;
using dominio.Enums;

namespace Test.Mother
{
    public static class ClienteMother
    {
        public static Cliente Create(
            string nome,
            string documento
        )
        {
            return new(
                nome,
                documento);
        }
        public static Cliente CarlosGomes()
        {
            var cliente = Create("Carlos Gomes",
                "38146012035");

            cliente.AddEndereco(new Endereco(cliente.Id,
                                                        EnumTipoEndereco.Residencial,
                                                        "45555-555",
                                                        "rua teste",
                                                        "55",
                                                        "",
                                                        "Bairro teste",
                                                        UnidadeFederativaMother.SaoPaulo(),
                                                        MunicipioMother.Santos()));

            return cliente;
        }
        public static Cliente RachelQueiroz()
        {
            var cliente = Create("Rachel Queiroz",
                "54147681000144");

            cliente.AddEndereco(new Endereco(cliente.Id,
                                                        EnumTipoEndereco.Residencial,
                                                        "45555-555",
                                                        "rua teste",
                                                        "55",
                                                        "",
                                                        "Bairro teste",
                                                        UnidadeFederativaMother.SaoPaulo(),
                                                        MunicipioMother.Santos()));

            return cliente;
        }
        public static Cliente ThomasFerreira()
        {
            var cliente = Create("Thomas Ferreira",
                "38146012035");

            cliente.AddEndereco(new Endereco(cliente.Id,
                                                        EnumTipoEndereco.Residencial,
                                                        "45555-555",
                                                        "rua teste",
                                                        "55",
                                                        "",
                                                        "Bairro teste",
                                                        UnidadeFederativaMother.SaoPaulo(),
                                                        MunicipioMother.Santos()));

            return cliente;
        }
        public static IEnumerable<Cliente> PrepareClientes()
        {
            return new List<Cliente>
            {
                CarlosGomes(),
                RachelQueiroz(),
                ThomasFerreira()
            };
        }
    }
}
