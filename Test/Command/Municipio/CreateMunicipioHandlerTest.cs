using Application.CommandSide.Commands.CreateMunicipio;
using Data.Context;
using Data.Repositories;
using FluentAssertions;
using System.Text;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Command.Municipio
{
    public class CreateMunicipioHandlerTest : ApplicationTestBase
    {
        private readonly CreateMunicipioHandler _handler;
        private readonly MunicipioRepository _municipioRepository;
        private readonly UnidadeFederativaRepository _unidadeFederativaRepository;

        public CreateMunicipioHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid(), contaId: Guid.NewGuid());

            _municipioRepository = new MunicipioRepository(_context);
            _unidadeFederativaRepository = new UnidadeFederativaRepository(_context);

            _handler = new CreateMunicipioHandler(_municipioRepository, _unidadeFederativaRepository);
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
        public async Task Handler_ShouldCreateMunicipio_WhenMunicipioNotExists()
        {
            var unidadeFederativa = UnidadeFederativaMother.SantaCatarina();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativa);
            await _context.SaveChangesAsync();

            var nome = "Florianopolis";

            var command = new CreateMunicipioCommand(
                    nome,
                    unidadeFederativa.Id
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var municipioFromDatabase = await _municipioRepository.ListAsync();

            result.IsFailure.Should().BeFalse();
            municipioFromDatabase.Should().HaveCount(1);
            municipioFromDatabase.First().Id.Should().Be(result.Data);
            municipioFromDatabase.First().Nome.Should().Be(nome);
        }

        [Fact]
        public async Task Handler_ShouldReturnsFailure_WhenUnidadeFederativaNotExists()
        {
            var unidadeFederativa = UnidadeFederativaMother.SantaCatarina();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativa);
            await _context.SaveChangesAsync();

            var nome = "Florianopolis";

            var command = new CreateMunicipioCommand(
                    nome,
                    Guid.NewGuid()
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var municipioFromDatabase = await _municipioRepository.ListAsync();

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("unidadeFederativaInexistente");
            municipioFromDatabase.Should().HaveCount(0);
        }

        [Theory]
        [InlineData(null, "nomeObrigatorio")]
        [InlineData("textoGrande", "nomeMaiorQue200Caracteres")]
        public async Task Handler_ShouldReturnsFailure_WhenDataValidationFault(string nome,
                                                                               string exceptionMessage)
        {
            var unidadeFederativa = UnidadeFederativaMother.SantaCatarina();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativa);
            await _context.SaveChangesAsync();


            var command = new CreateMunicipioCommand(
                    nome == "textoGrande" ? TextoGrande() : nome,
                    unidadeFederativa.Id
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var municipioFromDatabase = await _municipioRepository.ListAsync();

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain(exceptionMessage);
            municipioFromDatabase.Should().HaveCount(0);
        }
    }
}
