using Application.CommandSide.Commands.RemoveUnidadeFederativa;
using Data.Context;
using Data.Repositories;
using FluentAssertions;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Command.UnidadeFederativa
{
    public class RemoveUnidadeFederativaHandlerTest : ApplicationTestBase
    {
        private readonly UnidadeFederativaRepository _UnidadeFederativaRepository;
        private readonly RemoveUnidadeFederativaHandler _handler;

        public RemoveUnidadeFederativaHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid());

            _UnidadeFederativaRepository = new UnidadeFederativaRepository(_context);

            _handler = new RemoveUnidadeFederativaHandler(_UnidadeFederativaRepository);
        }

        [Fact]
        public async Task Handler_ShouldRemoveUnidadeFederativa_WhenUnidadeFederativaIdExists()
        {
            var unidadeFederativaCreate = UnidadeFederativaMother.SaoPaulo();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativaCreate);
            await _context.SaveChangesAsync();

            var command = new RemoveUnidadeFederativaCommand(
                unidadeFederativaCreate.Id
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var UnidadeFederativaFromDatabase = await _UnidadeFederativaRepository.GetAsync(unidadeFederativaCreate.Id);

            result.IsFailure.Should().BeFalse();
            UnidadeFederativaFromDatabase.Should().BeNull();
        }

        [Fact]
        public async Task Handler_ShouldReturnsFailure_WhenUnidadeFederativaIdNotExists()
        {
            var unidadeFederativaCreate = UnidadeFederativaMother.SaoPaulo();

            await _context.UnidadesFederativas.AddAsync(unidadeFederativaCreate);
            await _context.SaveChangesAsync();

            var command = new RemoveUnidadeFederativaCommand(
                Guid.NewGuid()
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var UnidadeFederativaFromDatabase = await _UnidadeFederativaRepository.ListAsync();

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("unidadeFederativaNaoEncontrada");
            UnidadeFederativaFromDatabase.Should().HaveCount(1);
        }
    }
}
