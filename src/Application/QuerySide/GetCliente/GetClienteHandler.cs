using Data.Context;
using Infraestructure.Response;
using Infraestructure.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.QuerySide.GetCliente
{
    public class GetClienteHandler : IRequestHandler<GetClienteQuery, NewResponse<GetClienteViewModel>>
    {
        private readonly SharedContext _context;
        public GetClienteHandler(SharedContext context)
        {
            _context = context;
        }
        public async Task<NewResponse<GetClienteViewModel>> Handle(GetClienteQuery request, CancellationToken cancellationToken)
        {
            var validation = new GetClienteValidator().Validate(request);
            if (!validation.IsValid)
                return new UnprocessableResponse<GetClienteViewModel>(validation.Errors.Select(x => x.ErrorMessage));

            var Cliente = await _context.Clientes
                .Include(p => p.Enderecos)
                    .ThenInclude(p => p.UnidadeFederativa)
                .Include(p => p.Enderecos)
                    .ThenInclude(p => p.Municipio)
                .FirstOrDefaultAsync(p => p.Id == request.Id);

            if (Cliente == null)
                return new NotFoundResponse<GetClienteViewModel>("clienteNaoEncontrado");

            return new OkResponse<GetClienteViewModel>(
                new GetClienteViewModel(
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
                        .ToList()));
        }
    }
}
