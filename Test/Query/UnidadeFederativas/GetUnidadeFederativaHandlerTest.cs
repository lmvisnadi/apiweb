using Application.QuerySide.GetUnidadeFederativa;
using Data.Context;
using FluentAssertions;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Query.UnidadeFederativa
{
    public class GetUnidadeFederativaHandlerTest : ApplicationTestBase
    {
        private readonly GetUnidadeFederativaHandler _handler;

        public GetUnidadeFederativaHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid(), contaId: Guid.NewGuid());
            _handler = new GetUnidadeFederativaHandler(_context);
        }

        [Fact]
        public async Task Handler_ShouldReturnUnidadeFederativa_WhenUnidadeFederativaIdExists()
        {
            var unidadeFederativaCreate = UnidadeFederativaMother.SantaCatarina();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativaCreate);
            await _context.SaveChangesAsync();

            var command = new GetUnidadeFederativaQuery(
                unidadeFederativaCreate.Id
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeFalse();
            result.Data.Id.Should().Be(unidadeFederativaCreate.Id);
            result.Data.Nome.Should().Be(unidadeFederativaCreate.Nome);
            result.Data.Sigla.Should().Be(unidadeFederativaCreate.Sigla);
        }

        [Fact]
        public async Task Handler_ShouldReturnsFailure_WhenUnidadeFederativaIdNotExists()
        {
            var unidadeFederativaCreate = UnidadeFederativaMother.SantaCatarina();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativaCreate);
            await _context.SaveChangesAsync();

            var command = new GetUnidadeFederativaQuery(
                Guid.NewGuid()
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("unidadeFederativaNaoEncontrada");
        }
    }
}
