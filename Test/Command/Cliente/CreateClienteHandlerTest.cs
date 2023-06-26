using Application.CommandSide.Commands.CreateCliente;
using Application.CommandSide.Commands.DTOs.Endereco;
using Data.Context;
using Data.Repositories;
using dominio.Enums;
using dominio.Repositories;
using FluentAssertions;
using KeL.Infrastructure.Security.Context;
using System.Text;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Command.Cliente
{
    public class CreateClienteHandlerTest : ApplicationTestBase
    {
        private readonly CreateClienteHandler _handler;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMunicipioRepository _municipioRepository;
        private readonly IUnidadeFederativaRepository _unidadeFederativaRepository;
        private readonly RequestContext _requestContext;

        public CreateClienteHandlerTest()
        {
            var userId = Guid.NewGuid();

            _context = SQLiteContextFactory.Create(userId: userId);
            _clienteRepository = new ClienteRepository(_context);
            _municipioRepository = new MunicipioRepository(_context);
            _unidadeFederativaRepository = new UnidadeFederativaRepository(_context);

            _requestContext = new RequestContext(
                token: "AnyToken",
                userId: userId,
                userName: "Usuario Nome",
                requestId: Guid.NewGuid(),
                 timeZone: -3
                );

            _handler = new CreateClienteHandler(_unidadeFederativaRepository,
                            _municipioRepository,
                            _clienteRepository);
        }

        private string TextoGrande()
        {
            const string preencher = "1234567890";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 1000; i++)
            {
                sb.Append(preencher);
            }
            return sb.ToString();
        }

        private List<InsertEnderecoDTO> EnderecosDefault(Guid unidadeFederativaId, Guid municipioId)
        {
            var enderecos = new List<InsertEnderecoDTO>()
            {
                new InsertEnderecoDTO(
                    tipoEndereco: EnumTipoEndereco.Residencial,
                    cep: "45555-555",
                    logradouro: "rua teste",
                    numero: "55",
                    complemento:  "",
                    bairro: "Bairro teste",
                    unidadeFederativaId: unidadeFederativaId,
                    municipioId: municipioId)
            };

            return enderecos;
        }

        [Fact]
        public async Task Handler_ShouldCreateCliente_WhenClienteNotExists()
        {
            var unidadeFederativa = UnidadeFederativaMother.SaoPaulo();
            var municipio = MunicipioMother.Santos();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativa);
            await _context.Municipios.AddAsync(municipio);
            await _context.SaveChangesAsync();

            var ClienteNome = "Nome Teste 1";

            var command = new CreateClienteCommand(
                    ClienteNome,
                    "38146012035",
                    EnderecosDefault(unidadeFederativa.Id, municipio.Id));

            var result = await _handler.Handle(command, CancellationToken.None);

            var ClientesFromDatabase = await _clienteRepository.ListAsync();

            result.IsFailure.Should().BeFalse();
            ClientesFromDatabase.Should().HaveCount(1);
            ClientesFromDatabase.First().Id.Should().Be(result.Data);
            ClientesFromDatabase.First().Nome.Should().Be(ClienteNome);
            ClientesFromDatabase.First().Enderecos.First().MunicipioId.Should().Be(municipio.Id);
            ClientesFromDatabase.First().Enderecos.First().UnidadeFederativaId.Should().Be(unidadeFederativa.Id);
        }

        [Fact]
        public async Task Handler_ShouldCreateCliente_WhenMunicipioNotExists()
        {
            var unidadeFederativa = UnidadeFederativaMother.SaoPaulo();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativa);
            await _context.SaveChangesAsync();

            List<InsertEnderecoDTO> enderecos = new List<InsertEnderecoDTO>();
            enderecos.Add(new InsertEnderecoDTO(EnumTipoEndereco.Residencial,
                                                "45555-555",
                                                "rua teste",
                                                "55",
                                                "",
                                                "Bairro teste",
                                                unidadeFederativa.Id,
                                                Guid.NewGuid()));

            var ClienteNome = "Nome Teste 1";

            var command = new CreateClienteCommand(
                    ClienteNome,
                    "38146012035",
                    enderecos);

            var result = await _handler.Handle(command, CancellationToken.None);

            var ClientesFromDatabase = await _clienteRepository.ListAsync();

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("municipioInexistente");
            ClientesFromDatabase.Should().HaveCount(0);
        }

        [Fact]
        public async Task Handler_ShouldReturnsFailure_WhenUFNotExists()
        {
            var municipio = MunicipioMother.Santos();

            await _context.Municipios.AddAsync(municipio);
            await _context.SaveChangesAsync();

            List<InsertEnderecoDTO> enderecos = new List<InsertEnderecoDTO>();
            enderecos.Add(new InsertEnderecoDTO(EnumTipoEndereco.Residencial,
                                                "45555-555",
                                                "rua teste",
                                                "55",
                                                "",
                                                "Bairro teste",
                                                Guid.NewGuid(),
                                                municipio.Id));

            var ClienteNome = "Nome Teste 1";

            var command = new CreateClienteCommand(
                    ClienteNome,
                    "38146012035",
                    enderecos);

            var result = await _handler.Handle(command, CancellationToken.None);

            var ClientesFromDatabase = await _clienteRepository.ListAsync();

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("ufInexistente");
            ClientesFromDatabase.Should().HaveCount(0);
        }


        [Theory]
        [InlineData("textoGrande", "38146012035", "maximo150CaracteresParaNome")]
        [InlineData("", "38146012035", "nomeObrigatorio")]
        [InlineData("Teste nome", "", "documentoObrigatorio")]
        [InlineData("Teste nome", "1234567891234567", "documentoNaoDeveSerMaiorQue14Caracteres")]
        public async Task Handler_ShouldReturnsFailure_WhenClienteValidationFault(string nome,
                                                                                  string documento,
                                                                                  string exceptionMessage)
        {
            var unidadeFederativa = UnidadeFederativaMother.SaoPaulo();
            var municipio = MunicipioMother.Santos();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativa);
            await _context.Municipios.AddAsync(municipio);
            await _context.SaveChangesAsync();

            var command = new CreateClienteCommand(
                    nome == "textoGrande" ? TextoGrande() : nome,
                    documento,
                    EnderecosDefault(unidadeFederativa.Id, municipio.Id));

            var result = await _handler.Handle(command, CancellationToken.None);

            var ClientesFromDatabase = await _clienteRepository.ListAsync();

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain(exceptionMessage);
            ClientesFromDatabase.Should().HaveCount(0);
        }

        [Theory]
        [InlineData(EnumTipoEndereco.Residencial, "45555-555", "", "55", "", "Bairro teste", "logradouroDeveEstarPreencido")]
        [InlineData(EnumTipoEndereco.Residencial, "45555-555", "textoGrande", "55", "", "Bairro teste", "maximoLogradouro150Caractes")]
        [InlineData(EnumTipoEndereco.Residencial, "textoGrande", "rua teste", "55", "", "Bairro teste", "CEPDeveTer9Caracteres")]
        [InlineData(EnumTipoEndereco.Residencial, "", "rua teste", "55", "", "Bairro teste", "CEPDeveTer9Caracteres")]
        [InlineData(EnumTipoEndereco.Residencial, "45555-555", "rua teste", "", "", "Bairro teste", "numeroDeveSerPreenchido")]
        [InlineData(EnumTipoEndereco.Residencial, "45555-555", "rua teste", "1234567", "", "Bairro teste", "maximo5CaracteresParaNumero")]
        [InlineData(EnumTipoEndereco.Residencial, "45555-555", "rua teste", "55", "textoGrande", "Bairro teste", "maximo150CaracteresParaComplemento")]
        [InlineData(EnumTipoEndereco.Residencial, "45555-555", "rua teste", "55", "", "", "bairroDeveSerPreenchido")]
        [InlineData(EnumTipoEndereco.Residencial, "45555-555", "rua teste", "55", "", "textoGrande", "maximo80CaracteresParaBairro")]

        public async Task Handler_ShouldReturnsFailure_WhenEnderecoValidationFault(EnumTipoEndereco tipoEndereco,
                                                                                    string cep,
                                                                                    string logradouro,
                                                                                    string numero,
                                                                                    string complemento,
                                                                                    string bairro,
                                                                                    string exceptionMessage)
        {

            var unidadeFederativa = UnidadeFederativaMother.SaoPaulo();
            var municipio = MunicipioMother.Santos();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativa);
            await _context.Municipios.AddAsync(municipio);
            await _context.SaveChangesAsync();

            var enderecos = new List<InsertEnderecoDTO>()
            {
                new InsertEnderecoDTO(
                    tipoEndereco: tipoEndereco,
                    cep: cep == "textoGrande" ? TextoGrande() : cep,
                    logradouro: logradouro == "textoGrande" ? TextoGrande() : logradouro,
                    numero: numero,
                    complemento: complemento == "textoGrande" ? TextoGrande() : complemento,
                    bairro: bairro == "textoGrande" ? TextoGrande() : bairro,
                    unidadeFederativaId: unidadeFederativa.Id,
                    municipioId: municipio.Id
                )
            };

            var ClienteNome = "Nome Teste 1";

            var command = new CreateClienteCommand(
                ClienteNome,
                "38146012035",
                enderecos);

            var result = await _handler.Handle(command, CancellationToken.None);

            var ClientesFromDatabase = await _clienteRepository.ListAsync();

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain(exceptionMessage);
            ClientesFromDatabase.Should().HaveCount(0);
        }
    }
}
