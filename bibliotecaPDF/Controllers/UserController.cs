using System.Security.Claims;
using System.Web;
using bibliotecaPDF.DTOs;
using bibliotecaPDF.Enums;
using bibliotecaPDF.Models;
using bibliotecaPDF.Models.Exceptions;
using bibliotecaPDF.Services;
using bibliotecaPDF.Services.Interfaces;
using bibliotecaPDF.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace bibliotecaPDF.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [AllowAnonymous]
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
            return Ok(new ResponseDTO(Status.OK, "Usuário criado com sucesso. Confirme seu email."));
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch(Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> LoginUser(LoginUserDTO loginDTO)
    {
        try
        {
            string token = await _userService.LoginUser(loginDTO);  
            return Ok(new ResponseDTO(Status.OK, "Usuário logado com sucesso.", token));
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch(Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }
    
    [AllowAnonymous]
    [HttpPost("{id}/Active")]
    public async Task<IActionResult> ActivateUser([FromRoute]string id, [FromQuery]string token)
    {
        try
        {
            
            await _userService.ActivateUser(id, token);  
            return Ok(new ResponseDTO(Status.OK, "Usuário ativado com sucesso.", token));
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch(Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }
    
    [Authorize]
    [HttpPost("Self")]
    public async Task<IActionResult> GetSelfUser()
    {
        try
        {
            string? userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            User selfUser = await _userService.GetUserWithPlanByEmail(userEmailClaim);  
            return Ok(new ResponseDTO(Status.OK, new SelfUserDTO(selfUser)));
        }
        catch (BusinessException ex)
        {
            return BadRequest(new ResponseDTO(Status.ERROR, ex.Message));
        }
        catch(Exception ex)
        {
            return StatusCode(500, new ResponseDTO(Status.ERROR, ex.Message));
        }
    }
    
    [Authorize]
    [HttpPost("Check")]
    public async Task<IActionResult> CheckUserToken()
    {
        return Ok();
    }
}
