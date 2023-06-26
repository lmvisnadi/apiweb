using Application.CommandSide.Commands.UpdateUnidadeFederativa;
using Data.Context;
using Data.Repositories;
using FluentAssertions;
using System.Text;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Command.UnidadeFederativa
{
    public class UpdateUnidadeFederativaHandlerTest : ApplicationTestBase
    {
        private readonly UnidadeFederativaRepository _unidadeFederativaRepository;
        private readonly UpdateUnidadeFederativaHandler _handler;

        public UpdateUnidadeFederativaHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid());

            _unidadeFederativaRepository = new UnidadeFederativaRepository(_context);

            _handler = new UpdateUnidadeFederativaHandler(_unidadeFederativaRepository);
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
        public async Task Handler_ShouldUpdateUnidadeFederativa_WhenUnidadeFederativaIdExists()
        {
            var unidadeFederativaCreate = UnidadeFederativaMother.Parana();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativaCreate);
            await _context.SaveChangesAsync();

            var nomeChanged = "Teste";
            var siglaChanged = "tt";

            var command = new UpdateUnidadeFederativaCommand(
                unidadeFederativaCreate.Id,
                nomeChanged,
                siglaChanged
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var UnidadeFederativaFromDatabase = await _unidadeFederativaRepository.GetAsync(unidadeFederativaCreate.Id);

            result.IsFailure.Should().BeFalse();
            UnidadeFederativaFromDatabase.Should().NotBeNull();
            UnidadeFederativaFromDatabase!.Id.Should().Be(command.Id);
            UnidadeFederativaFromDatabase.Nome.Should().Be(command.Nome);
            UnidadeFederativaFromDatabase.Sigla.Should().Be(command.Sigla);

        }

        [Fact]
        public async Task Handler_ShouldReturnsFailure_WhenUnidadeFederativaIdNotExists()
        {
            var nomeChanged = "Teste";
            var siglaChanged = "tt";

            var command = new UpdateUnidadeFederativaCommand(
                 Guid.NewGuid(),
                nomeChanged,
                siglaChanged
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var UnidadeFederativaFromDatabase = await _unidadeFederativaRepository.ListAsync();

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("unidadeFederativaNaoEncontrada");
            UnidadeFederativaFromDatabase.Should().HaveCount(0);
        }


        [Theory]
        [InlineData(1, null, "SP", "nomeEObrigatorio")]
        [InlineData(1, "textoGrande", "SP", "nomeDeveSerMenorQue150Caracteres")]
        [InlineData(1, "São Paulo", null, "siglaEObrigatorio")]
        [InlineData(1, "São Paulo", "textoGrande", "maximo2CaracteresParaSigla")]
        public async Task Handler_ShouldReturnsFailure_WhenDataValidationFault(int codigo,
                                                                                    string nome,
                                                                                    string sigla,
                                                                                    string exceptionMessage)
        {
            var unidadeFederativaCreate = UnidadeFederativaMother.Parana();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativaCreate);
            await _context.SaveChangesAsync();

            var command = new UpdateUnidadeFederativaCommand(
                unidadeFederativaCreate.Id,
                nome == "textoGrande" ? TextoGrande() : nome,
                sigla == "textoGrande" ? TextoGrande() : sigla
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var unidadeFederativaFromDatabase = await _unidadeFederativaRepository.ListAsync();

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain(exceptionMessage);
            unidadeFederativaFromDatabase.Should().HaveCount(1);
        }
    }
}
