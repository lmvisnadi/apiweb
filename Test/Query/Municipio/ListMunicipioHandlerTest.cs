using Application.QuerySide.ListMunicipio;
using Data.Context;
using FluentAssertions;
using FluentAssertions.Execution;
using Infraestructure.Pagination;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Query.ListMunicipio
{
    public class ListMunicipioHandlerTest : ApplicationTestBase
    {
        private readonly ListMunicipioHandler _handler;

        public ListMunicipioHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid());
            _handler = new ListMunicipioHandler(_context);
        }

        [Fact]
        public async Task Handler_ShouldReturnMunicipioList_WhenMunicipioExists()
        {

            var lista = MunicipioMother.PrepareMunicipios();

            _context.UnidadesFederativas.Add(lista.First().UnidadeFederativa);
            _context.Municipios.AddRange(lista);
            _context.SaveChanges();

            var command = new ListMunicipioQuery();

            var result = await _handler.Handle(command, CancellationToken.None);

            var listViewModel = lista
                .Select(p => new ListMunicipioViewModel(
                    p.Id,
                    p.Nome,
                    new ListMunicipioUnidadeFederativaViewModel(
                        p.UnidadeFederativa.Id,
                        p.UnidadeFederativa.Nome,
                        p.UnidadeFederativa.Sigla
                    )
                ))
                .ToList();

            using (new AssertionScope())
            {
                result.IsFailure.Should().BeFalse();
                result.Data.Items.Should().BeInAscendingOrder(p => p.Nome);
                result.Data.Items.Should().BeEquivalentTo(listViewModel.AsEnumerable());
            }
        }

        [Fact]
        public async Task Handler_ShouldReturnMunicipioList_WhenMunicipioExistsWithPaginationOptions()
        {
            var lista = MunicipioMother.PrepareMunicipios();

            _context.UnidadesFederativas.Add(lista.First().UnidadeFederativa);
            _context.Municipios.AddRange(lista);
            _context.SaveChanges();

            var paginationOptions = new PaginationOptions
            {
                Page = 1,
                AmountPerPage = 2
            };
            var query = new ListMunicipioQuery(
                unidadeFederativaId: lista.First().UnidadeFederativa.Id,
                nome: null,
                paginationOptions
            );

            var result = await _handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.IsFailure.Should().BeFalse();
                result.Data.Items.Should().HaveCount(2);
            }

            paginationOptions.Page = 2;
            query = new ListMunicipioQuery(
                unidadeFederativaId: lista.First().UnidadeFederativa.Id,
                nome: null,
                paginationOptions
            );

            result = await _handler.Handle(query, CancellationToken.None);

            using (new AssertionScope())
            {
                result.IsFailure.Should().BeFalse();
                result.Data.Items.Should().HaveCount(1);
            }
        }
    }
}
