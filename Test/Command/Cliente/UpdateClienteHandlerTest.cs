using Application.CommandSide.Commands.DTOs.Endereco;
using Application.CommandSide.Commands.UpdateCliente;
using Data.Context;
using Data.Repositories;
using dominio.Enums;
using dominio.Repositories;
using FluentAssertions;
using System.Text;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Command.Cliente
{
    public class UpdateClienteHandlerTest : ApplicationTestBase
    {
        private readonly UpdateClienteHandler _handler;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMunicipioRepository _municipioRepository;
        private readonly IUnidadeFederativaRepository _unidadeFederativaRepository;

        public UpdateClienteHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid());
            _clienteRepository = new ClienteRepository(_context);
            _municipioRepository = new MunicipioRepository(_context);
            _unidadeFederativaRepository = new UnidadeFederativaRepository(_context);

            _handler = new UpdateClienteHandler(_clienteRepository,
                                                _municipioRepository,
                                                _unidadeFederativaRepository);
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

        [Fact]
        public async Task Handler_ShouldUpdateCliente_WhenClienteIdExists()
        {
            var clienteCreate = ClienteMother.CarlosGomes();
            var unidadeFederativa = clienteCreate.Enderecos.First().UnidadeFederativa;
            var municipio = clienteCreate.Enderecos.First().Municipio;

            await _context.UnidadesFederativas.AddAsync(unidadeFederativa);
            await _context.Municipios.AddAsync(municipio);
            await _context.Clientes.AddAsync(clienteCreate);
            await _context.SaveChangesAsync();

            var nomeChanged = "Teste Alterado 1";

            var enderecos = clienteCreate.Enderecos
                                .Select(p => new UpdateEnderecoDTO(
                                    p!.Id,
                                    p.TipoEndereco,
                                    p.Cep,
                                    p.Logradouro,
                                    p.Numero,
                                    p.Complemento,
                                    p.Bairro,
                                    unidadeFederativa.Id,
                                    municipio.Id))
                                .ToList();

            var command = new UpdateClienteCommand(
                clienteCreate.Id,
                nomeChanged,
                clienteCreate.Documento,
                enderecos
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var ClientesFromDatabase = await _clienteRepository.GetAsync(clienteCreate.Id);

            result.IsFailure.Should().BeFalse();
            ClientesFromDatabase.Should().NotBeNull();
            ClientesFromDatabase!.Id.Should().Be(command.Id);
            ClientesFromDatabase.Nome.Should().Be(command.Nome);
            ClientesFromDatabase.Enderecos.First().MunicipioId.Should().Be(municipio.Id);
            ClientesFromDatabase.Enderecos.First().UnidadeFederativaId.Should().Be(unidadeFederativa.Id);
        }

        [Fact]
        public async Task Handler_ShouldReturnsFailure_WhenClienteIdNotExists()
        {
            var clienteCreate = ClienteMother.CarlosGomes();
            var unidadeFederativa = clienteCreate.Enderecos.First().UnidadeFederativa;
            var municipio = clienteCreate.Enderecos.First().Municipio;

            await _context.UnidadesFederativas.AddAsync(unidadeFederativa);
            await _context.Municipios.AddAsync(municipio);
            await _context.Clientes.AddAsync(clienteCreate);
            await _context.SaveChangesAsync();

            var nomeChanged = "Teste Alterado 2";

            var enderecos = clienteCreate.Enderecos
                                .Select(p => new UpdateEnderecoDTO(
                                    p!.Id,
                                    p.TipoEndereco,
                                    p.Cep,
                                    p.Logradouro,
                                    p.Numero,
                                    p.Complemento,
                                    p.Bairro,
                                    unidadeFederativa.Id,
                                    municipio.Id))
                                .ToList();

            var command = new UpdateClienteCommand(
                Guid.NewGuid(),
                nomeChanged,
                clienteCreate.Documento,
                enderecos
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var ClientesFromDatabase = await _clienteRepository.GetAsync(clienteCreate.Id);

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("clienteNaoEncontrado");
            ClientesFromDatabase!.Nome.Should().Be(clienteCreate.Nome);

        }

        [Fact]
        public async Task Handler_ShouldUpdateCliente_WhenMunicipioNotExists()
        {
            var clienteCreate = ClienteMother.CarlosGomes();
            var unidadeFederativa = clienteCreate.Enderecos.First().UnidadeFederativa;
            var municipio = clienteCreate.Enderecos.First().Municipio;

            await _context.UnidadesFederativas.AddAsync(unidadeFederativa);
            await _context.Municipios.AddAsync(municipio);
            await _context.Clientes.AddAsync(clienteCreate);
            await _context.SaveChangesAsync();

            var nomeChanged = "Teste Alterado 3";

            var enderecos = clienteCreate.Enderecos
                                .Select(p => new UpdateEnderecoDTO(
                                    p!.Id,
                                    p.TipoEndereco,
                                    p.Cep,
                                    p.Logradouro,
                                    p.Numero,
                                    p.Complemento,
                                    p.Bairro,
                                    unidadeFederativa.Id,
                                    Guid.NewGuid()))
                                .ToList();

            var command = new UpdateClienteCommand(
               clienteCreate.Id,
               nomeChanged,
               clienteCreate.Documento,
               enderecos
               );

            var result = await _handler.Handle(command, CancellationToken.None);

            var ClientesFromDatabase = await _clienteRepository.GetAsync(clienteCreate.Id);

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("municipioInexistente");
            ClientesFromDatabase!.Enderecos.First().MunicipioId
                .Should().Be(clienteCreate.Enderecos.First().MunicipioId);
        }

        [Fact]
        public async Task Handler_ShouldUpdateCliente_WhenUFNotExists()
        {
            var clienteCreate = ClienteMother.CarlosGomes();
            var unidadeFederativa = clienteCreate.Enderecos.First().UnidadeFederativa;
            var municipio = clienteCreate.Enderecos.First().Municipio;

            await _context.UnidadesFederativas.AddAsync(unidadeFederativa);
            await _context.Municipios.AddAsync(municipio);
            await _context.Clientes.AddAsync(clienteCreate);
            await _context.SaveChangesAsync();

            var nomeChanged = "Teste Alterado 3";

            var enderecos = clienteCreate.Enderecos
                                .Select(p => new UpdateEnderecoDTO(
                                    p!.Id,
                                    p.TipoEndereco,
                                    p.Cep,
                                    p.Logradouro,
                                    p.Numero,
                                    p.Complemento,
                                    p.Bairro,
                                    Guid.NewGuid(),
                                    municipio.Id))
                                .ToList();

            var command = new UpdateClienteCommand(
               clienteCreate.Id,
               nomeChanged,
               clienteCreate.Documento,
               enderecos
               );

            var result = await _handler.Handle(command, CancellationToken.None);

            var ClientesFromDatabase = await _clienteRepository.GetAsync(clienteCreate.Id);

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("ufInexistente");
            ClientesFromDatabase!.Enderecos.First().UnidadeFederativaId
                .Should().Be(clienteCreate.Enderecos.First().UnidadeFederativaId);
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
            var clienteCreate = ClienteMother.CarlosGomes();
            var unidadeFederativa = clienteCreate.Enderecos.First().UnidadeFederativa;
            var municipio = clienteCreate.Enderecos.First().Municipio;

            await _context.UnidadesFederativas.AddAsync(unidadeFederativa);
            await _context.Municipios.AddAsync(municipio);
            await _context.Clientes.AddAsync(clienteCreate);
            await _context.SaveChangesAsync();

            var enderecos = clienteCreate.Enderecos
                                .Select(p => new UpdateEnderecoDTO(
                                    p!.Id,
                                    p.TipoEndereco,
                                    p.Cep,
                                    p.Logradouro,
                                    p.Numero,
                                    p.Complemento,
                                    p.Bairro,
                                    unidadeFederativa.Id,
                                    municipio.Id))
                                .ToList();


            var command = new UpdateClienteCommand(
                    clienteCreate.Id,
                    nome == "textoGrande" ? TextoGrande() : nome,
                    documento,
                    enderecos);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain(exceptionMessage);
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
            var clienteCreate = ClienteMother.CarlosGomes();
            var unidadeFederativa = clienteCreate.Enderecos.First().UnidadeFederativa;
            var municipio = clienteCreate.Enderecos.First().Municipio;

            await _context.UnidadesFederativas.AddAsync(unidadeFederativa);
            await _context.Municipios.AddAsync(municipio);
            await _context.Clientes.AddAsync(clienteCreate);
            await _context.SaveChangesAsync();

            var enderecos = new List<UpdateEnderecoDTO>()
            {
                new UpdateEnderecoDTO(
                    id: Guid.NewGuid(),
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

            var command = new UpdateClienteCommand(
                clienteCreate.Id,
                clienteCreate.Nome,
                clienteCreate.Documento,
                enderecos
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain(exceptionMessage);
        }
    }
}
