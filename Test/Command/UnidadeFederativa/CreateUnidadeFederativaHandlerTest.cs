using Application.CommandSide.Commands.CreateUnidadeFederativa;
using Data.Context;
using Data.Repositories;
using FluentAssertions;
using System.Text;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Command.UnidadeFederativa
{
    public class CreateUnidadeFederativaHandlerTest : ApplicationTestBase
    {
        private readonly UnidadeFederativaRepository _unidadeFederativaRepository;
        private readonly CreateUnidadeFederativaHandler _handler;

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
        public CreateUnidadeFederativaHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid(), contaId: Guid.NewGuid());

            _unidadeFederativaRepository = new UnidadeFederativaRepository(_context);

            _handler = new CreateUnidadeFederativaHandler(_unidadeFederativaRepository);
        }

        [Fact]
        public async Task Handler_ShouldCreateunidadeFederativa_WhenUnidadeFederativaNotExists()
        {
            var sigla = "sp";
            var nome = "São Paulo";

            var command = new CreateUnidadeFederativaCommand(
                    sigla: sigla,
                    nome: nome
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var unidadeFederativaFromDatabase = await _unidadeFederativaRepository.ListAsync();

            result.IsFailure.Should().BeFalse();
            unidadeFederativaFromDatabase.Should().HaveCount(1);
            unidadeFederativaFromDatabase.First().Id.Should().Be(result.Data);
            unidadeFederativaFromDatabase.First().Sigla.Should().Be(sigla);
            unidadeFederativaFromDatabase.First().Nome.Should().Be(nome);
        }

        [Fact]
        public async Task Handler_ShouldReturnsFailure_WhenUnidadeFederativaCodigoAlreadyExists()
        {
            var unidadeFederativaCreate = UnidadeFederativaMother.Parana();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativaCreate);
            await _context.SaveChangesAsync();

            var sigla = unidadeFederativaCreate.Sigla;
            var nome = "São Paulo";

            var command = new CreateUnidadeFederativaCommand(
                    sigla: sigla,
                    nome: nome
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var unidadeFederativaFromDatabase = await _unidadeFederativaRepository.ListAsync();

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("siglaJaExistente");
            unidadeFederativaFromDatabase.Should().HaveCount(1);
        }

        [Theory]
        [InlineData(null, "SP", "nomeEObrigatorio")]
        [InlineData("textoGrande", "SP", "nomeDeveSerMenorQue150Caracteres")]
        [InlineData("São Paulo", null, "siglaEObrigatorio")]
        [InlineData("São Paulo", "textoGrande", "maximo2CaracteresParaSigla")]
        public async Task Handler_ShouldReturnsFailure_WhenDataValidationFault(string nome,
                                                                               string sigla,
                                                                               string exceptionMessage)
        {
            var command = new CreateUnidadeFederativaCommand(
                    sigla == "textoGrande" ? TextoGrande() : sigla,
                    nome == "textoGrande" ? TextoGrande() : nome
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var unidadeFederativaFromDatabase = await _unidadeFederativaRepository.ListAsync();

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain(exceptionMessage);
            unidadeFederativaFromDatabase.Should().HaveCount(0);
        }
    }
}
