using bibliotecaPDF.DTOs;
using bibliotecaPDF.Enums;
using bibliotecaPDF.Models;
using bibliotecaPDF.Models.Exceptions;
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
            var validationResult = new CreateUserDTOValidator().Validate(userDTO);
            if (!validationResult.IsValid)
            {
                return StatusCode(400, new ResponseDTO(Status.ERROR, validationResult.Errors));
            }

            await _userService.CreateUser(userDTO);
            return Ok(new ResponseDTO(Status.OK, "Usuário criado com sucesso."));
        }
        catch (BussinesException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch(Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }

    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser(LoginUserDTO loginDTO)
    {
        try
        {
            string token = await _userService.LoginUser(loginDTO);  
            return Ok(new ResponseDTO(Status.OK, "Usuário logado com sucesso.", token));
        }
        catch (BussinesException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch(Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }
}
