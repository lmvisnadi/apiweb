using FluentValidation;

namespace Application.CommandSide.Commands.UpdateCliente
{
    public class UpdateClienteValidator : AbstractValidator<UpdateClienteCommand>
    {
        public UpdateClienteValidator()
        {
            RuleFor(p => p.Id)
                .NotEmpty()
                .WithMessage("idObrigatorio");

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
               .SetValidator(new UpdateEnderecoValidator());
        }
    }
}
