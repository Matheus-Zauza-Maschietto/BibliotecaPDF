using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;
using bibliotecaPDF.Repository;
using bibliotecaPDF.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace bibliotecaPDF.Services;

public class UserService
{
    private readonly UserManager<User> _userManager;
    private readonly JsonWebTokensService _webTokensService;
    private readonly IUserRepository _userRepository;
    public UserService(UserManager<User> userManager, JsonWebTokensService webTokensService, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _userManager = userManager;
        _webTokensService = webTokensService;
    }

    public async Task CreateUser(CreateUserDTO userDto)
    {
        User? existUser = await _userRepository.GetUserByEmail(userDto.Email);

        if(existUser is not null)
        {
            throw new Exception("Already exists an user with this email");
        }

        IdentityResult result = await _userRepository.CreateUser(userDto);

        if (!result.Succeeded)
        {
            throw new Exception(GetIdentityResultErros(result));
        }
    }

    public async Task<string> LoginUser(LoginUserDTO loginDto)
    {
        User? existUser = await _userManager.FindByEmailAsync(loginDto.Email);

        if (existUser is null)
        {
            throw new Exception("Not exists an user with this email");
        }

        bool passwordIsOk = await _userManager.CheckPasswordAsync(existUser, loginDto.Password);

        if(!passwordIsOk)
        {
            throw new Exception("Password was wrong");
        }

        return _webTokensService.GerarToken(GetClaims(loginDto));
    }

    private ClaimsIdentity GetClaims(LoginUserDTO loginDto)
    {
        return new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Email, loginDto.Email)
        });
    }

    private string GetIdentityResultErros(IdentityResult identityResult) => string.Join(". ", identityResult.Errors.Select(p => p.Description));
    
}
