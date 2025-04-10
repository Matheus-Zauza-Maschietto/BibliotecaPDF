using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;

namespace bibliotecaPDF.Services.Interfaces;

public interface IMessageService
{
    Task<Message> SendMessageAsync(User user, MessageCreateDTO messageCreate);
    Task<ICollection<Message>> GetMessagesAsync(int pageNumber);
}