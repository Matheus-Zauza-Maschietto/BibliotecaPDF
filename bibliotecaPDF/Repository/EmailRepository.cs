using System.Net;
using System.Net.Mail;
using bibliotecaPDF.Models;
using bibliotecaPDF.Repository.Interfaces;
using Microsoft.Extensions.Options;

namespace bibliotecaPDF.Repository;

public class EmailRepository : IEmailRepository
{
    private readonly EmailSettings _emailSettings;

    public EmailRepository(
            IOptions<EmailSettings> emailSettings
        )
    {
        _emailSettings = emailSettings.Value;
    }
    
    
    public async Task SendEmailAsync(string email, string subject, string message, bool isBodyHtml=false)
    {
        using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
        {
            Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password),
            EnableSsl = true,
            Port = 587
        };
        
        var mailMessage = new MailMessage
        {
            From = new MailAddress(_emailSettings.Email, "Biblioteca PDF"),
            Subject = subject,
            Body = message,
            IsBodyHtml = isBodyHtml
        };
        mailMessage.To.Add(email);
        
        client.Send(mailMessage);
    }
}