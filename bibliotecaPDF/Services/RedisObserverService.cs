using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using bibliotecaPDF.Controllers;
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

    public async Task PublishAsync(string channel, object message)
    {
        try{
            string? messageString = message is string ? message.ToString() : JsonSerializer.Serialize(message);
            if (string.IsNullOrEmpty(messageString))
            {
                throw new ArgumentException("Message cannot be null or empty");
            }
            await _subscriber.PublishAsync(RedisChannel.Literal(channel), messageString);
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
            _hubContext.Clients.All.SendAsync("ReceiveMessage", channel.ToString(), message.ToString());
        });
    }
}
