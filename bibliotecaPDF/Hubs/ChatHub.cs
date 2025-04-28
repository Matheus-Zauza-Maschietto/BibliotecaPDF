using System.Collections.Concurrent;
using System.Security.Claims;
using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;
using bibliotecaPDF.Models.Enums;
using bibliotecaPDF.Services;
using bibliotecaPDF.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace bibliotecaPDF.Controllers;

[Authorize]
public class ChatHub : Hub
{
    private IConnectionMappingService _connectionMappingService;
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;
    private readonly IRedisObserverService _redisObserverService;

    public ChatHub(
        IMessageService messageService, 
        IUserService userService, 
        IConnectionMappingService connectionMappingService, 
        IRedisObserverService redisObserverService
    )
    {
        _redisObserverService = redisObserverService;
        _messageService = messageService;
        _userService = userService;
        _connectionMappingService = connectionMappingService;
    }
    
    public override async Task OnConnectedAsync()
    {
        var email = Context.User?.FindFirst(ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(email))
        {
            throw new UnauthorizedAccessException();
        }

        _connectionMappingService.Add(Context.ConnectionId, await _userService.GetUserByEmailOrUsername(email));
        await base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionMappingService.Remove(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(MessageCreateDTO message)
    {
        _connectionMappingService.TryGet(Context.ConnectionId, out User user);
        Message createdMessage = await _messageService.SendMessageAsync(user, message);
        await Clients.All.SendAsync("ReceiveMessage", new MessageGetDTO(createdMessage));
        await _redisObserverService.PublishAsync(RedisTopics.MESSAGES.ToString(), new MessageGetDTO(createdMessage));
    }

    public async Task GetLastMessages(int pageNumber)
    {
        ICollection<Message> messages = await _messageService.GetMessagesAsync(pageNumber);
        await Clients.Caller.SendAsync("ReceiveMessages", messages.Select(p => new MessageGetDTO(p)));
    }
}