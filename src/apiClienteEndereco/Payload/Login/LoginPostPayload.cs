using Application.CommandSide.Commands.Login;

namespace apiClienteEndereco.Payload.Login
{
    public struct LoginPostPayload
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public decimal Timezone { get; init; }

        public LoginPostPayload(
            string email,
            string password,
            decimal timezone
            )
        {
            Email = email;
            Password = password;
            Timezone = timezone;
        }

        public LoginCommand AsCommand()
            => new(Email, Password, Timezone);
    }
}
