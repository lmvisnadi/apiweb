using FluentValidation;

namespace Application.CommandSide.Commands.CreateCliente
{
    public class CreateClienteValidator : AbstractValidator<CreateClienteCommand>
    {
        public CreateClienteValidator()
        {
            RuleFor(p => p.Nome)
                .NotEmpty()
                .WithMessage("nomeObrigatorio")
                .MaximumLength(150)
                .WithMessage("maximo150CaracteresParaNome");

            RuleFor(p => p.Documento)
                .NotEmpty()
                .WithMessage("documentoObrigatorio")
                .MaximumLength(14)
                .WithMessage("documentoNaoDeveSerMaiorQue14Caracteres");

            RuleForEach(y => y.Enderecos)
               .SetValidator(new CreateEnderecoValidator());
        }
    }
}
