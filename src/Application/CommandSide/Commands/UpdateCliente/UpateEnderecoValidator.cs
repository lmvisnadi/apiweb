using Application.CommandSide.Commands.DTOs.Endereco;
using FluentValidation;

namespace Application.CommandSide.Commands.UpdateCliente
{
    public class UpdateEnderecoValidator : AbstractValidator<UpdateEnderecoDTO>
    {
        public UpdateEnderecoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("idDeveEstarPreencido");

            RuleFor(x => x.Logradouro)
                .NotEmpty()
                .WithMessage("logradouroDeveEstarPreencido")
                .MaximumLength(150)
                .WithMessage("maximoLogradouro150Caractes");

            RuleFor(x => x.Cep)
                .NotEmpty()
                .WithMessage("CEPDeveSerPreenchido")
                .Length(9)
                .WithMessage("CEPDeveTer9Caracteres");

            RuleFor(x => x.Numero)
                .NotEmpty()
                .WithMessage("numeroDeveSerPreenchido")
                .MaximumLength(5)
                .WithMessage("maximo5CaracteresParaNumero");

            RuleFor(x => x.Complemento)
                .MaximumLength(150)
                .WithMessage("maximo150CaracteresParaComplemento");

            RuleFor(x => x.Bairro)
                .NotEmpty()
                .WithMessage("bairroDeveSerPreenchido")
                .MaximumLength(80)
                .WithMessage("maximo80CaracteresParaBairro");
        }
    }
}
