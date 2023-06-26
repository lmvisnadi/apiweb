using Application.CommandSide.Commands.RemoveCliente;
using Data.Context;
using Data.Repositories;
using FluentAssertions;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Command.Cliente
{
    public class RemoveClienteHandlerTest : ApplicationTestBase
    {
        private readonly RemoveClienteHandler _handler;
        private readonly ClienteRepository _ClienteRepository;

        public RemoveClienteHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid());

            _ClienteRepository = new ClienteRepository(_context);

            _handler = new RemoveClienteHandler(_ClienteRepository);
        }

        [Fact]
        public async Task Handler_ShouldRemoveCliente_WhenClienteIdExists()
        {
            var ClienteCreate = ClienteMother.CarlosGomes();

            await _context.Clientes.AddAsync(ClienteCreate);
            await _context.SaveChangesAsync();

            var command = new RemoveClienteCommand(
                ClienteCreate.Id
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var ClienteFromDatabase = await _ClienteRepository.GetAsync(ClienteCreate.Id);

            result.IsFailure.Should().BeFalse();
            ClienteFromDatabase.Should().BeNull();
        }

        [Fact]
        public async Task Handler_ShouldReturnsFailure_WhenClienteIdNotExists()
        {
            var ClienteCreate = ClienteMother.CarlosGomes();

            await _context.Clientes.AddAsync(ClienteCreate);
            await _context.SaveChangesAsync();

            var command = new RemoveClienteCommand(
                Guid.NewGuid()
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var ClienteFromDatabase = await _ClienteRepository.ListAsync();

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("clienteNaoEncontrado");
            ClienteFromDatabase.Should().HaveCount(1);
        }

    }
}
