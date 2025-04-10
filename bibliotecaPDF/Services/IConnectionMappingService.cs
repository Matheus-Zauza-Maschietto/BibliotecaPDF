using System.Collections.Concurrent;
using bibliotecaPDF.Models;
using bibliotecaPDF.Services.Interfaces;

namespace bibliotecaPDF.Services;

public class ConnectionMappingService : IConnectionMappingService
{
    private readonly ConcurrentDictionary<string, User> _connections = new();

    public void Add(string connectionId, User info)
    {
        _connections[connectionId] = info;
    }

    public bool TryGet(string connectionId, out User info)
    {
        return _connections.TryGetValue(connectionId, out info);
    }

    public void Remove(string connectionId)
    {
        _connections.TryRemove(connectionId, out _);
    }

    public IEnumerable<KeyValuePair<string, User>> GetAll() => _connections;
}