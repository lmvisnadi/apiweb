using Application.QuerySide.ListUnidadeFederativa;
using Data.Context;
using FluentAssertions;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Query.UnidadeFederativas
{
    public class ListUnidadeFederativaQueryHandlerTest : ApplicationTestBase
    {
        private readonly ListUnidadeFederativaHandler _handler;

        public ListUnidadeFederativaQueryHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid());
            _handler = new ListUnidadeFederativaHandler(_context);
        }

        [Fact]
        public async Task Handler_ShouldReturnUnidadeFederativaList_WhenUnidadeFederativaExists()
        {

            var listaCreate = UnidadeFederativaMother.ListaUnidadeFederativas();

            await _context.UnidadesFederativas.AddRangeAsync(listaCreate);
            await _context.SaveChangesAsync();

            var command = new ListUnidadeFederativaQuery();

            var result = await _handler.Handle(command, CancellationToken.None);

            var listViewModel = listaCreate
                .Select(p => new ListUnidadeFederativaViewModel(
                    p.Id,
                    p.Nome,
                    p.Sigla)).ToList();

            result.IsFailure.Should().BeFalse();
            result.Data.Should().BeInAscendingOrder(p => p.Nome);
            result.Data.Should().BeEquivalentTo(listViewModel.AsEnumerable());
        }
    }
}
