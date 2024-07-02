using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;
using bibliotecaPDF.Repository.Interfaces;
using bibliotecaPDF.Services;
using Microsoft.AspNetCore.Identity;

namespace bibliotecaPDF.Repository;

public class UserRepository: IUserRepository
{
    private readonly UserManager<User> _userManager;

    public UserRepository(UserManager<User> userManager, JsonWebTokensService webTokensService)
    {
        _userManager = userManager;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IdentityResult> CreateUser(CreateUserDTO userDto)
    {
        User newUser = new User()
        {
            Email = userDto.Email,
            UserName = userDto.UserName
        };

        return await _userManager.CreateAsync(newUser, userDto.Password);
    }
}
