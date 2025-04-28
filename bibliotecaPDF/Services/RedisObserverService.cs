using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using bibliotecaPDF.Controllers;
using bibliotecaPDF.Models;
using bibliotecaPDF.Models.Enums;
using bibliotecaPDF.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;
using StackExchange.Redis;

namespace bibliotecaPDF.Services;

public class RedisObserverService : IRedisObserverService
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly ISubscriber _subscriber;
    private readonly IHubContext<ChatHub> _hubContext;

    public RedisObserverService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _subscriber = _connectionMultiplexer.GetSubscriber();
        SubscribeAsync(RedisTopics.MESSAGES.ToString()).Wait();
    }

    private string GetIndexedMessage(string message)
    {
        var indexedMessage = new WebsocketRedisMessage(message, _connectionMultiplexer.GetHashCode().ToString());
        return JsonSerializer.Serialize(indexedMessage);
    }

    public async Task PublishAsync(string channel, string? message){
        try{
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("Message cannot be null or empty");
            }

            await _subscriber.PublishAsync(RedisChannel.Literal(channel), GetIndexedMessage(message));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error publishing message: {ex.Message}");
        }
    }

    public async Task PublishAsync(string channel, object message)
    {
        try{
            string? messageString = message is string ? message.ToString() : JsonSerializer.Serialize(message);
            await PublishAsync(channel, messageString);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error publishing message: {ex.Message}");
        }
    }
    
    private async Task SubscribeAsync(string channel)
    {
        await _subscriber.SubscribeAsync(channel, (channel, message) =>
        {
            var websocketMessage = JsonSerializer.Deserialize<WebsocketRedisMessage>(message);
            if(_connectionMultiplexer.GetHashCode().ToString() == websocketMessage.OwnerHash.ToString())
            {
                return;
            }
            _hubContext.Clients.All.SendAsync("ReceiveMessage", channel.ToString(), message.ToString());
        });
    }
}
