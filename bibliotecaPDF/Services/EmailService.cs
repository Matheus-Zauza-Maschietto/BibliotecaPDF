using System.Text;
using bibliotecaPDF.Repository.Interfaces;
using bibliotecaPDF.Services.Interfaces;
using Org.BouncyCastle.Utilities.Encoders;

namespace bibliotecaPDF.Services;

public class EmailService : IEmailService
{
    private readonly IEmailRepository _emailRepository;
    public EmailService(IEmailRepository emailRepository)
    {
        _emailRepository = emailRepository; 
    }
    
    public async Task SendUserActivationEmailAsync(string email, string id, string token)
    {
        byte[] textoAsBytes = Encoding.ASCII.GetBytes(token);
        string tokenB64 = System.Convert.ToBase64String(textoAsBytes);
        string message = string.Format(@"<div
                                            style='font-family: 'Franklin Gothic Medium', 'Arial Narrow', Arial, sans-serif; height: 150px; display: flex; flex-direction: column; justify-content: space-around; align-items: center;'>
                                            <h1 style='font-size: x-large'>Ativação de Conta em Biblioteca <span style='color: #ff0000;'>PDF</span></h1>
                                            <button type='button' 
                                                style='width: 300px; height: 50px; border-radius: 10px; background-color: #1D1D1D; box-shadow: 2px 2px 10px rgba(0, 0, 0, 0.472);'>
                                                <a href='http://localhost:8080/usuario/{0}/ativar/{1}' style='text-decoration: none; font-size: large; color: #F4F4F4; padding: 16px 0;'>
                                                    Clique aqui para ativar sua conta.
                                                </a>
                                            </button>
                                        </div>"
            ,id, tokenB64);
        await _emailRepository.SendEmailAsync(email, "Ativação de conta. <Não Responder>", message, true);
    }
}