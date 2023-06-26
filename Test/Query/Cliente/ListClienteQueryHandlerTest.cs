using Application.QuerySide.ListCliente;
using Data.Context;
using FluentAssertions;
using Infraestructure.Pagination;
using Test.Base;
using Xunit;

namespace Test.Query.Cliente
{
    public class ListClienteHandlerTest : ApplicationTestBase
    {

        private readonly ListClienteHandler _handler;
        public ListClienteHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid());
            _handler = new ListClienteHandler(_context);
        }

        [Fact]
        public async Task Handler_ShouldReturnClienteList_WhenClienteExists()
        {
            var command = new ListClienteQuery();

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeFalse();
            result.Data!.Items.Count().Equals(2);
        }

        [Fact]
        public async Task Handler_ShouldReturnClienteList_WhenClienteExistsPagination()
        {
            PaginationOptions pagination = new PaginationOptions();
            pagination.Page = 1;
            pagination.AmountPerPage = 2;

            var command = new ListClienteQuery(
                pagination);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeFalse();
            result.Data!.Items.Count().Equals(2);

            pagination.Page = 2;
            pagination.AmountPerPage = 2;

            command = new ListClienteQuery(
                pagination);

            result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeFalse();
            result.Data!.Items.Count().Equals(1);
        }

    }
}
