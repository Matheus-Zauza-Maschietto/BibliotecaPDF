using bibliotecaPDF.Models;

namespace bibliotecaPDF.Services.Interfaces;

public interface IEmailService
{
    Task SendUserActivationEmailAsync(string email, string id, string token);
}