using FluentValidation;

namespace Application.CommandSide.Commands.CreateMunicipio
{
    public class CreateMunicipioValidator : AbstractValidator<CreateMunicipioCommand>
    {
        public CreateMunicipioValidator()
        {
            RuleFor(p => p.Nome)
                .NotEmpty()
                .WithMessage("nomeObrigatorio")
                .MaximumLength(200)
                .WithMessage("nomeMaiorQue200Caracteres");
        }
    }
}
