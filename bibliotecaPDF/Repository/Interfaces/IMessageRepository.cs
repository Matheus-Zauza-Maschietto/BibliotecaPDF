using bibliotecaPDF.Models;

namespace bibliotecaPDF.Repository.Interfaces;

public interface IMessageRepository
{
    Task<Message> CreateMessage(Message message);
    Task<ICollection<Message>> GetPaginatedMessages(int pageNumber, int pageSize);
}