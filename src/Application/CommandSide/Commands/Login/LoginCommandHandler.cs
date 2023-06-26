using dominio.Entities;
using dominio.Repositories;
using dominio.Servico;
using Infraestructure.Response;
using Infraestructure.Token;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace Application.CommandSide.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, NewResponse<LoginCommandViewModel>>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUsuarioDomainService _userDomainService;
        private readonly IConfiguration _configuration;

        public LoginCommandHandler(
            IRefreshTokenRepository refreshTokenRepository,
            IUsuarioDomainService usuarioDomainService,
            IConfiguration configuration
            )
        {
            _refreshTokenRepository = refreshTokenRepository;
            _userDomainService = usuarioDomainService;
            _configuration = configuration;
        }
        public async Task<NewResponse<LoginCommandViewModel>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var validationResult = new LoginCommandValidator().Validate(request);
            if (!validationResult.IsValid)
                return new UnprocessableResponse<LoginCommandViewModel>(validationResult.Errors.Select(e => e.ErrorMessage));

            var usuario = await _userDomainService.GetAsync(request.Email, MD5Geral.CriptogragarMD5(request.Senha));

            if (usuario == null)
                return new UnauthorizedResponse<LoginCommandViewModel>("naoAutorizado");

            var bearerToken = new TokenBuilder()
                .WithUserId(usuario.Id)
                .WithUsername(usuario.Nome)
                .WithTimeZone(request.Timezone)
                .WithJwtIssuer(_configuration["Jwt:Issuer"])
                .WithJwtKey(_configuration["Jwt:Key"])
                .Build();

            var ValidadeRefreshToken = DateTime.UtcNow.AddHours(int.Parse(_configuration["RefreshToken:ValidadeInHours"]));

            var refreshToken = new RefreshToken(usuario.Id, ValidadeRefreshToken);

            await _refreshTokenRepository.InsertAsync(refreshToken);
            await _refreshTokenRepository.SaveAsync();

            return new OkResponse<LoginCommandViewModel>(new LoginCommandViewModel(bearerToken, refreshToken.Token, usuario.Id));
        }
    }
}
