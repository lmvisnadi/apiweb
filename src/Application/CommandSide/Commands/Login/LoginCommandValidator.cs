using FluentValidation;

namespace Application.CommandSide.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(command => command.Email)
                .NotEmpty()
                .WithMessage("emailNaoPodeSerVazio");

            RuleFor(command => command.Senha)
                .NotEmpty()
                .WithMessage("senhaNaoPodeSerVazio");
        }
    }
}
