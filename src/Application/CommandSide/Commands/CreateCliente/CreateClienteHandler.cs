using dominio.Entities;
using dominio.Repositories;
using Infraestructure.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CommandSide.Commands.CreateCliente
{
    public class CreateClienteHandler : IRequestHandler<CreateClienteCommand, NewResponse<Guid>>
    {
        private IUnidadeFederativaRepository _unidadeFederativaRepository;
        private IMunicipioRepository _municipioRepository;
        private IClienteRepository _clienteRepository;
        public CreateClienteHandler(IUnidadeFederativaRepository unidadeFederativaRepository,
                                    IMunicipioRepository municipioRepository,
                                    IClienteRepository clienteRepository)
        {
            _unidadeFederativaRepository = unidadeFederativaRepository;
            _municipioRepository = municipioRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task<NewResponse<Guid>> Handle(CreateClienteCommand request, CancellationToken cancellationToken)
        {
            var validationResult = new CreateClienteValidator().Validate(request);
            if (!validationResult.IsValid)
                return new UnprocessableResponse<Guid>(validationResult.Errors.Select(e => e.ErrorMessage));

            var distinctUFId = request.Enderecos.Select(c => c.UnidadeFederativaId).Distinct();

            var ufs = await _unidadeFederativaRepository.GetAll()
                .Where(p => distinctUFId.Any(p2 => p2 == p.Id)).AsQueryable().ToListAsync();

            if (ufs.Count() != distinctUFId.Count())
                return new NotFoundResponse<Guid>("ufInexistente");

            var distinctMunicipioId = request.Enderecos.Select(c => c.MunicipioId).Distinct();

            var municipios = await _municipioRepository.GetAll()
                .Where(p => distinctMunicipioId.Any(p2 => p2 == p.Id)).AsQueryable().ToListAsync();

            if (municipios.Count() != distinctMunicipioId.Count())
                return new NotFoundResponse<Guid>("municipioInexistente");

            var cliente = new Cliente(request.Nome, request.Documento);

            request.Enderecos.ForEach(e =>
            {
                var unidadeFederativa = ufs.First(p => p.Id == e.UnidadeFederativaId);
                var municipio = municipios.First(p => p.Id == e.MunicipioId);

                cliente.AddEndereco(new Endereco(
                                                    cliente.Id,
                                                    e.TipoEndereco,
                                                    e.Cep,
                                                    e.Logradouro,
                                                    e.Numero,
                                                    e.Complemento,
                                                    e.Bairro,
                                                    unidadeFederativa!,
                                                    municipio!));
            });

            await _clienteRepository.InsertAsync(cliente);
            await _clienteRepository.SaveAsync(cancellationToken);

            return new CreatedResponse<Guid>(cliente.Id);
        }

    }
}
