using Application.QuerySide.GetCliente;
using Data.Context;
using FluentAssertions;
using Infraestructure.ViewModels;
using Test.Base;
using Test.Mother;
using Xunit;

namespace Test.Query.Cliente
{
    public class GetClienteQueryHandlerTest : ApplicationTestBase
    {
        private readonly GetClienteHandler _handler;

        public GetClienteQueryHandlerTest()
        {
            _context = SQLiteContextFactory.Create(userId: Guid.NewGuid());
            _handler = new GetClienteHandler(_context);
        }

        [Fact]
        public async Task Handler_ShouldReturnCliente_WhenClienteIdExists()
        {
            var Cliente = ClienteMother.CarlosGomes();

            await _context.Clientes.AddAsync(Cliente);
            await _context.SaveChangesAsync();

            var command = new GetClienteQuery(
                Cliente.Id
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            var returnedViewModel = new GetClienteViewModel(
                    Cliente.Id,
                    Cliente.Nome,
                    Cliente.Documento,
                    Cliente.Enderecos
                        .Select(e => new GetEnderecoViewModel(
                            Cliente.Id,
                            e.TipoEndereco,
                            e.Cep,
                            e.Logradouro,
                            e.Numero,
                            e.Complemento,
                            e.Bairro,
                            new ViewModelWithIdAndNome(
                                e.UnidadeFederativaId,
                                e.UnidadeFederativa.Sigla),
                            new ViewModelWithIdAndNome(
                                e.MunicipioId,
                                e.Municipio.Nome)))
                        .ToList()
                    );

            result.IsFailure.Should().BeFalse();
            result.Data.Id.Should().Be(Cliente.Id);
            result.Data.Should().BeEquivalentTo(returnedViewModel, x => x.ComparingByMembers<GetClienteViewModel>());
        }

        [Fact]
        public async Task Handler_ShouldReturnsFailure_WhenClienteIdNotExists()
        {
            var ClienteCreate = ClienteMother.CarlosGomes();

            await _context.Clientes.AddAsync(ClienteCreate);
            await _context.SaveChangesAsync();

            var command = new GetClienteQuery(
                Guid.NewGuid()
                );

            var result = await _handler.Handle(command, CancellationToken.None);

            result.IsFailure.Should().BeTrue();
            result.Messages.Should().Contain("clienteNaoEncontrado");
        }
    }
}
