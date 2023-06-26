using FluentValidation;

namespace Application.QuerySide.GetCliente
{
    public class GetClienteValidator : AbstractValidator<GetClienteQuery>
    {
        public GetClienteValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty()
                .WithMessage("Id inválido!");
        }
    }
}
