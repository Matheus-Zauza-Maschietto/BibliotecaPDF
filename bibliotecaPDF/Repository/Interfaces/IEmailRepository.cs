using bibliotecaPDF.Models;

namespace bibliotecaPDF.Repository.Interfaces;

public interface IEmailRepository
{
    Task SendEmailAsync(string email, string subject, string message, bool isBodyHtml=false);
}