using FluentValidation;

namespace Application.CommandSide.Commands.CreateUnidadeFederativa
{
    public class CreateUnidadeFederativaValidator : AbstractValidator<CreateUnidadeFederativaCommand>
    {
        public CreateUnidadeFederativaValidator()
        {
            RuleFor(p => p.Sigla)
                .MaximumLength(2)
                .WithMessage("maximo2CaracteresParaSigla")
                .NotEmpty()
                .WithMessage("siglaEObrigatorio");

            RuleFor(p => p.Nome)
                .NotEmpty()
                .WithMessage("nomeEObrigatorio")
                .MaximumLength(150)
                .WithMessage("nomeDeveSerMenorQue150Caracteres");

        }
    }
}
