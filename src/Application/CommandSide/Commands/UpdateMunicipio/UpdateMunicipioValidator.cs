using FluentValidation;

namespace Application.CommandSide.Commands.UpdateMunicipio
{
    public class UpdateMunicipioValidator : AbstractValidator<UpdateMunicipioCommand>
    {
        public UpdateMunicipioValidator()
        {
            RuleFor(p => p.Nome)
                    .NotEmpty()
                    .WithMessage("O nome é obrigatório")
                    .MaximumLength(200)
                    .WithMessage("Máximo 200 caracteres para o nome");
        }
    }
}
