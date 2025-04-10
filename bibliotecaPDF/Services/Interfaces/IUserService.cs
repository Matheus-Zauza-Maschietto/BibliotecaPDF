using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;
using Microsoft.AspNetCore.Identity;

namespace bibliotecaPDF.Services.Interfaces;

public interface IUserService
{
    Task<User> GetUntrackedUserByEmail(string email);
    Task CreateUser(CreateUserDTO userDto);
    Task<string> LoginUser(LoginUserDTO loginDto);
    Task ActivateUser(string id, string token);
    Task<string> GenerateUserActivationToken(User user);
    Task<User> GetUserWithPlanByEmail(string email);
    Task<User> GetUserByEmailOrUsername(string email);
}