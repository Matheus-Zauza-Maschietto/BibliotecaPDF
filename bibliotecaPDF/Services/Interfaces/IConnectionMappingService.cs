using bibliotecaPDF.Models;

namespace bibliotecaPDF.Services.Interfaces;

public interface IConnectionMappingService
{
    void Add(string connectionId, User user);
    bool TryGet(string connectionId, out User user);
    void Remove(string connectionId);
    IEnumerable<KeyValuePair<string, User>> GetAll();
}