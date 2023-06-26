using FluentValidation;

namespace Application.QuerySide.GetMunicipio
{
    public class GetMunicipioValidator : AbstractValidator<GetMunicipioQuery>
    {
        public GetMunicipioValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty()
                .WithMessage("Id inválido!");
        }
    }
}
