using dominio.Entities;
using dominio.Repositories;
using Infraestructure.Extensions;
using Infraestructure.Response;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CommandSide.Commands.UpdateCliente
{
    public class UpdateClienteHandler : IRequestHandler<UpdateClienteCommand, NewResponse<Unit>>
    {
        private readonly IClienteRepository _repository;
        private readonly IMunicipioRepository _municipioRepository;
        private readonly IUnidadeFederativaRepository _unidadeFederativaRepository;

        public UpdateClienteHandler(
            IClienteRepository repository,
            IMunicipioRepository municipioRepository,
            IUnidadeFederativaRepository unidadeFederativaRepository
        )
        {
            _repository = repository;
            _municipioRepository = municipioRepository;
            _unidadeFederativaRepository = unidadeFederativaRepository;
        }

        public async Task<NewResponse<Unit>> Handle(UpdateClienteCommand request,
            CancellationToken cancellationToken)
        {
            var validationResult = new UpdateClienteValidator().Validate(request);
            if (!validationResult.IsValid)
                return new UnprocessableResponse<Unit>(validationResult.Errors.Select(e => e.ErrorMessage));

            var Cliente = await _repository.GetAsync(request.Id);

            if (Cliente == null)
                return new NotFoundResponse<Unit>("clienteNaoEncontrado");

            var distinctUFId = request.Enderecos.Select(c => c.UnidadeFederativaId).Distinct();
            var ufs = await _unidadeFederativaRepository.GetAll()
                .Where(p => distinctUFId.Any(p2 => p2 == p.Id))
                .ToListAsync();
            if (ufs.Count() != distinctUFId.Count()) return new UnprocessableResponse<Unit>("ufInexistente");

            var distinctMunicipioId = request.Enderecos.Select(c => c.MunicipioId).Distinct();
            var municipios = await _municipioRepository.GetAll()
                .Where(p => distinctMunicipioId.Any(p2 => p2 == p.Id))
                .ToListAsync();
            if (municipios.Count() != distinctMunicipioId.Count()) return new UnprocessableResponse<Unit>("municipioInexistente");

            Cliente.ChangeNome(request.Nome);
            Cliente.ChangeDocumento(request.Documento);

            Cliente.Enderecos
                .Where(e => request.Enderecos.Any(pe => pe.Id == e.Id)).ToList()
                .ForEach(endereco =>
                {
                    var requestEndereco = request.Enderecos.First(re => re.Id == endereco.Id);

                    var unidadeFederativa = ufs.First(p => p.Id == requestEndereco.UnidadeFederativaId);
                    var municipio = municipios.First(p => p.Id == requestEndereco.MunicipioId);

                    endereco.ChangeEndereco(
                        requestEndereco.TipoEndereco,
                        requestEndereco.Cep,
                        requestEndereco.Logradouro,
                        requestEndereco.Numero,
                        requestEndereco.Complemento,
                        requestEndereco.Bairro,
                        unidadeFederativa!,
                        municipio!);

                    Cliente.ChangeEndereco(endereco);
                });

            Cliente.Enderecos
                .Where(pe => !request.Enderecos
                    .Any(re => re.Id == pe.Id))
                .ToList()
                .ForEach(e => Cliente.RemoveEndereco(e));

            request.Enderecos
                .Where(pe => pe.Id is null)
                .ForEach(e =>
                {
                    var unidadeFederativa = ufs.First(p => p.Id == e.UnidadeFederativaId);
                    var municipio = municipios.First(p => p.Id == e.MunicipioId);

                    Cliente.AddEndereco(
                        new Endereco(
                            Cliente.Id,
                            e.TipoEndereco,
                            e.Cep,
                            e.Logradouro,
                            e.Numero,
                            e.Complemento,
                            e.Bairro,
                            unidadeFederativa!,
                            municipio!));
                });

            _repository.Update(Cliente);
            await _repository.SaveAsync(cancellationToken);

            return new NoContentResponse<Unit>();
        }
    }
}
