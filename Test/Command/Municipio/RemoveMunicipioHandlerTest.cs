using Application.CommandSide.Commands.RemoveMuncipio;
using Data.Context;
using Data.Repositories;
using FluentAssertions;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Command.Municipio
{
    public class RemoveMunicipioHandlerTest : ApplicationTestBase
    {
        private readonly RemoveMunicipioHandler _handler;
        private readonly MunicipioRepository _municipioRepository;

        public RemoveMunicipioHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid());

            _municipioRepository = new MunicipioRepository(_context);

            _handler = new RemoveMunicipioHandler(_municipioRepository);
        }

        [Fact]
        public async Task Handler_ShouldRemoveMunicipio_WhenMunicipioIdExists()
        {
            var municipioCreate = MunicipioMother.Curitiba();

            await _context.Municipios.AddAsync(municipioCreate);
            await _context.SaveChangesAsync();

            var command = new RemoveMunicipioCommand(
                municipioCreate.Id
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var municipioFromDatabase = await _municipioRepository.GetAsync(municipioCreate.Id);

            result.IsFailure.Should().BeFalse();
            municipioFromDatabase.Should().BeNull();
        }

        [Fact]
        public async Task Handler_ShouldReturnsFailure_WhenMunicipioIdNotExists()
        {
            var municipioCreate = MunicipioMother.Curitiba();

            await _context.Municipios.AddAsync(municipioCreate);
            await _context.SaveChangesAsync();

            var command = new RemoveMunicipioCommand(
                Guid.NewGuid()
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var municipioFromDatabase = await _municipioRepository.ListAsync();

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("municipioNaoEncontrado");
            municipioFromDatabase.Should().HaveCount(1);
        }
    }
}
