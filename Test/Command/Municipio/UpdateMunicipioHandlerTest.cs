using Application.CommandSide.Commands.UpdateMunicipio;
using Data.Context;
using Data.Repositories;
using FluentAssertions;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Command.Municipio
{
    public class UpdateMunicipioHandlerTest : ApplicationTestBase
    {
        private readonly UpdateMunicipioHandler _handler;
        private readonly MunicipioRepository _municipioRepository;
        private readonly UnidadeFederativaRepository _unidadeFederativaRepository;

        public UpdateMunicipioHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid(), contaId: Guid.NewGuid());

            _municipioRepository = new MunicipioRepository(_context);
            _unidadeFederativaRepository = new UnidadeFederativaRepository(_context);

            _handler = new UpdateMunicipioHandler(_municipioRepository, _unidadeFederativaRepository);
        }

        [Fact]
        public async Task Handler_ShouldUpdateMunicipio_WhenMunicipioIdExists()
        {
            var municipioCreate = MunicipioMother.Curitiba();
            var unidadeFederativaCreate = UnidadeFederativaMother.SantaCatarina();

            await _context.Municipios.AddAsync(municipioCreate);
            await _context.UnidadesFederativas.AddAsync(unidadeFederativaCreate);
            await _context.SaveChangesAsync();

            var nomeChanged = "Blumenau";

            var command = new UpdateMunicipioCommand(
                municipioCreate.Id,
                nomeChanged,
                unidadeFederativaCreate.Id
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var municipioFromDatabase = await _municipioRepository.GetAsync(municipioCreate.Id);

            result.IsFailure.Should().BeFalse();
            municipioFromDatabase.Should().NotBeNull();
            municipioFromDatabase!.Id.Should().Be(municipioCreate.Id);
            municipioFromDatabase.Nome.Should().Be(nomeChanged);
            municipioFromDatabase.UnidadeFederativaId.Should().Be(unidadeFederativaCreate.Id);
        }

        [Fact]
        public async Task Handler_ShouldReturnsFailure_WhenMunicipioIdNotExists()
        {
            var municipioCreate = MunicipioMother.Curitiba();
            var unidadeFederativaCreate = UnidadeFederativaMother.SantaCatarina();

            await _context.Municipios.AddAsync(municipioCreate);
            await _context.UnidadesFederativas.AddAsync(unidadeFederativaCreate);
            await _context.SaveChangesAsync();

            var nomeChanged = "Blumenau";

            var command = new UpdateMunicipioCommand(
                Guid.NewGuid(),
                nomeChanged,
                unidadeFederativaCreate.Id
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var municipioFromDatabase = await _municipioRepository.GetAsync(municipioCreate.Id);

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("municipioNaoEncontrado");
            municipioFromDatabase!.Nome.Should().Be(municipioCreate.Nome);
            municipioFromDatabase.UnidadeFederativaId.Should().Be(municipioCreate.UnidadeFederativaId);
        }

        [Fact]
        public async Task Handler_ShouldReturnsFailure_WhenUnidadeFederativaNotExists()
        {
            var municipioCreate = MunicipioMother.Curitiba();
            var unidadeFederativaCreate = UnidadeFederativaMother.SantaCatarina();

            await _context.Municipios.AddAsync(municipioCreate);
            await _context.UnidadesFederativas.AddAsync(unidadeFederativaCreate);
            await _context.SaveChangesAsync();

            var nomeChanged = "Blumenau";

            var command = new UpdateMunicipioCommand(
                municipioCreate.Id,
                nomeChanged,
                Guid.NewGuid()
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var municipioFromDatabase = await _municipioRepository.GetAsync(municipioCreate.Id);

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("unidadeFederativaInexistente");
            municipioFromDatabase!.UnidadeFederativaId.Should().Be(municipioCreate.UnidadeFederativaId);
        }
    }
}
