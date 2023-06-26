using Infraestructure.Response;
using MediatR;

namespace Application.CommandSide.Commands.Login
{
    public class LoginCommand : IRequest<NewResponse<LoginCommandViewModel>>
    {
        public string Email { get; init; }
        public string Senha { get; init; }
        public decimal Timezone { get; init; }

        public LoginCommand(
            string email,
            string senha,
            decimal timezone
            )
        {
            Email = email;
            Senha = senha;
            Timezone = timezone;
        }
    }
}
