using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;
using bibliotecaPDF.Services;
using bibliotecaPDF.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bibliotecaPDF.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateUser(CreateUserDTO userDTO)
    {
        try
        {
            new CreateUserDTOValidator().ValidateAndThrow(userDTO);
            await _userService.CreateUser(userDTO);
            return Ok("Usuario criado com sucesso");
        }
        catch(Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser(LoginUserDTO loginDTO)
    {
        try
        {
            string token = await _userService.LoginUser(loginDTO);  
            return Ok(token);
        }
        catch(Exception ex)
        {
            return Problem(ex.Message);
        }
    }
}
