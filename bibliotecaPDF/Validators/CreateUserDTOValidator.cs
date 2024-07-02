using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;
using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;

namespace bibliotecaPDF.Validators;

public class CreateUserDTOValidator : AbstractValidator<CreateUserDTO>
{
    public CreateUserDTOValidator()
    {
        RuleFor(p => p.Email)
            .NotEmpty().WithMessage("O campo \"EMail\" não pode ser vazio ou nulo")
            .MinimumLength(5).WithMessage("O campo \"EMail\" não pode ter menos de 5 caracteres ao total")
            .EmailAddress().WithMessage("O campo \"EMail\" não deve possui @ e dominio de email");

        RuleFor(p => p.UserName)
            .NotEmpty().WithMessage("O campo \"UserName\" não pode ser vazio ou nulo")
            .Length(3, 100).WithMessage("O campo \"UserName\" deve estar entre 3 a 100 caracteres");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("O campo \"Password\" não pode ser vazio ou nulo")
            .Length(3, 100).WithMessage("O campo \"Password\" deve estar entre 3 a 100 caracteres")
            .Must(HasCaracters).WithMessage("O campo \"Password\" deve conter pelo menos 1 caracter especial, 1 numero e 1 letra maiscula")
            .When(p => p.UserName == "s");
    }


    private bool HasCaracters(string password) =>
        Regex.IsMatch(password, @"[!@#$%¨&*()_\-+={}?:><^~]") && Regex.IsMatch(password, @"[0-9]") && Regex.IsMatch(password, @"[A-Z]");
}
