using FluentValidation;

namespace Application.CommandSide.Commands.UpdateUnidadeFederativa
{
    public class UpdateUnidadeFederativaValidator : AbstractValidator<UpdateUnidadeFederativaCommand>
    {
        public UpdateUnidadeFederativaValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty()
                .WithMessage("Id é obrigatório");

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
