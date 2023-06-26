using Application.QuerySide.GetMunicipio;
using Data.Context;
using FluentAssertions;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Query.Municipio
{
    public class GetMunicipioHandlerTest : ApplicationTestBase
    {
        private readonly GetMunicipioHandler _handler;

        public GetMunicipioHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid(), contaId: Guid.NewGuid());
            _handler = new GetMunicipioHandler(_context);
        }

        [Fact]
        public async Task Handler_ShouldReturnMunicipio_WhenMunicipioIdExists()
        {
            var municipioCreate = MunicipioMother.Curitiba();

            await _context.Municipios.AddAsync(municipioCreate);
            await _context.SaveChangesAsync();

            var command = new GetMunicipioQuery(
                municipioCreate.Id
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeFalse();
            result.Data!.Nome.Should().Be(municipioCreate.Nome);
        }

        [Fact]
        public async Task Handler_ShouldReturnsFailure_WhenMunicipioIdNotExists()
        {
            var municipioCreate = MunicipioMother.Curitiba();

            await _context.Municipios.AddAsync(municipioCreate);
            await _context.SaveChangesAsync();

            var command = new GetMunicipioQuery(
                Guid.NewGuid()
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("municipioNaoEncontrado");
        }
    }
}
