using bibliotecaPDF.Context;
using bibliotecaPDF.Models;
using bibliotecaPDF.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bibliotecaPDF.Repository;

public class MessageRepository :IMessageRepository
{
    private readonly ApplicationDbContext _applicationContext;

    public MessageRepository(ApplicationDbContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public async Task<Message> CreateMessage(Message message)
    {
        _applicationContext.Attach(message.User);
        Message createdMessage =  _applicationContext.Messages.Add(message).Entity;
        await _applicationContext.SaveChangesAsync();
        return createdMessage;
    }

    public async Task<ICollection<Message>> GetPaginatedMessages(int pageNumber, int pageSize)
    {
        return await _applicationContext.Messages
            .Include(m => m.User)
            .AsNoTracking()
            .OrderByDescending(m => m.DateTime)
            .ThenByDescending(m => m.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}