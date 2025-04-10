using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;
using bibliotecaPDF.Repository;
using bibliotecaPDF.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using bibliotecaPDF.Models.Exceptions;
using bibliotecaPDF.Services.Interfaces;

namespace bibliotecaPDF.Services;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly JsonWebTokensService _webTokensService;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly IGenericRepository _genericRepository;
    public UserService(
            UserManager<User> userManager, 
            JsonWebTokensService webTokensService,
            IEmailService emailService,
            IUserRepository userRepository,
            IGenericRepository genericRepository
        )
    {
        _userManager = userManager;
        _webTokensService = webTokensService;
        _emailService = emailService;
        _userRepository = userRepository;
        _genericRepository = genericRepository;
    }

    public async Task<User> GetUserWithPlanByEmail(string email)
    {
        User? user = await _userRepository.GetUserWithPlanByEmail(email);
        if (user == null)
        {
            throw new BusinessException("Usuário não encontrado.");
        }

        return user;
    }
    
    public async Task<User> GetUntrackedUserByEmail(string email)
    {
        User? user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new BusinessException("Usuário não encontrado.");
        }

        return user;
    }
    
    public async Task<User> GetUserByEmailOrUsername(string email)
    {
        User? user = await _userRepository.GetUserByEmailOrUsername(email);
        if (user == null)
        {
            throw new BusinessException("Usuário não encontrado.");
        }

        return user;
    }

    public async Task CreateUser(CreateUserDTO userDto)
    {
        User? existUser = await _userRepository.GetUserByEmailOrUsername(userDto.Email);

        if (existUser is not null)
        {
            throw new BusinessException("Já existe um usuário com esse E-mail.");
        }


        User newUser = new User()
        {
            Email = userDto.Email,
            UserName = userDto.Email,
            Name = userDto.Name,
            EmailConfirmed = false,
            CapacityPlan = _genericRepository.Get<CapacityPlan>(1)
        };
        
        IdentityResult result = await _userManager.CreateAsync(newUser, userDto.Password);

        if (!result.Succeeded)
        {
            throw new BusinessException(GetIdentityResultErros(result));
        }
        User nearlyCreatedUser = await GetUntrackedUserByEmail(userDto.Email);
        var emailConfirmToken = await GenerateUserActivationToken(nearlyCreatedUser);
        await _emailService.SendUserActivationEmailAsync(newUser.Email, newUser.Id, emailConfirmToken);
    }

    public async Task<string> GenerateUserActivationToken(User user)
    {
        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }
    
    public async Task ActivateUser(string id, string token)
    {
        User? user = await _userManager.FindByIdAsync(id);
        if (user is null) throw new BusinessException("Usuário não encontrado.");
        
        byte[] tokenB64 = System.Convert.FromBase64String(token);
        string finalToken = System.Text.ASCIIEncoding.ASCII.GetString(tokenB64);
        IdentityResult result = await _userManager.ConfirmEmailAsync(user, finalToken);
        
        if(!result.Succeeded) throw new BusinessException(GetIdentityResultErros(result));
    }
    
    public async Task<string> LoginUser(LoginUserDTO loginDto)
    {
        User? existUser = await _userManager.FindByEmailAsync(loginDto.Email);

        if (existUser is null)
        {
            throw new BusinessException("Não existe um usuário com esse E-mail.");
        }

        bool passwordIsOk = await _userManager.CheckPasswordAsync(existUser, loginDto.Password);

        if(!passwordIsOk)
        {
            throw new BusinessException("Senha errada.");
        }

        if (!await _userManager.IsEmailConfirmedAsync(existUser))
        {
            throw new BusinessException("E-Mail não confirmado. Acesse seu e-mail e confirme sua conta.");
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

    private string GetIdentityResultErros(IdentityResult identityResult) => string.Join(". \n", identityResult.Errors.Select(p => p.Description));
    
}
