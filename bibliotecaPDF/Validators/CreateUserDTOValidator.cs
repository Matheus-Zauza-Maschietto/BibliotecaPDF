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
            .NotEmpty().WithMessage("O campo \"E-mail.\" não pode ser vazio ou nulo")
            .MinimumLength(5).WithMessage("O campo \"E-mail.\" não pode ter menos de 5 caracteres ao total")
            .EmailAddress().WithMessage("O campo \"E-mail.\" não deve possui @ e dominio de E-mail.");

        RuleFor(p => p.UserName)
            .NotEmpty().WithMessage("O campo \"Nome\" não pode ser vazio ou nulo")
            .Length(3, 100).WithMessage("O campo \"Nome\" deve estar entre 3 a 100 caracteres");

        RuleFor(p => p.Password)
            .NotEmpty().WithMessage("O campo \"Senha\" não pode ser vazio ou nulo")
            .Length(3, 100).WithMessage("O campo \"Senha\" deve estar entre 3 a 100 caracteres")
            .Must(HasCaracters).WithMessage("O campo \"Senha\" deve conter pelo menos 1 caracter especial, 1 numero e 1 letra maiscula");
        
        RuleFor(p => p.ConfirmPassword)
            .NotEmpty().WithMessage("O campo \"Confirmar Senha\" não pode ser vazio ou nulo")
            .Length(3, 100).WithMessage("O campo \"Confirmar Senha\" deve estar entre 3 a 100 caracteres")
            .Must(HasCaracters).WithMessage("O campo \"Confirmar Senha\" deve conter pelo menos 1 caracter especial, 1 numero e 1 letra maiscula")
            .Equal(p => p.Password).WithMessage("O campo \"Confirmar Senha\" deve ser igual ao \"Senha\"");
        
    }


    private bool HasCaracters(string password) =>
        Regex.IsMatch(password, @"[!@#$%¨&*()_\-+={}?:><^~]") && Regex.IsMatch(password, @"[0-9]") && Regex.IsMatch(password, @"[A-Z]");
}
