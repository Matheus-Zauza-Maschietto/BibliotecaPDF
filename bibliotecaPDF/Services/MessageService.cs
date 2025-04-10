using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;
using bibliotecaPDF.Repository.Interfaces;
using bibliotecaPDF.Services.Interfaces;

namespace bibliotecaPDF.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _messageRepository;
    public MessageService(IMessageRepository messageRepository)
    {
        _messageRepository = messageRepository;
    }
    
    public async Task<Message> SendMessageAsync(User user, MessageCreateDTO messageCreate)
    {
        Message newMessage = new Message(user, messageCreate);
        return await _messageRepository.CreateMessage(newMessage);
    }

    public async Task<ICollection<Message>> GetMessagesAsync(int pageNumber)
    {
        if(pageNumber <= 0) throw new ArgumentException(nameof(pageNumber));
        return await _messageRepository.GetPaginatedMessages(pageNumber, 20);
    }
}