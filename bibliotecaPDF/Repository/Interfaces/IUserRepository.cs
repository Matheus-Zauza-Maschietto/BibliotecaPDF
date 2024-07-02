using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;
using Microsoft.AspNetCore.Identity;

namespace bibliotecaPDF.Repository.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmail(string email);
    Task<IdentityResult> CreateUser(CreateUserDTO userDto);
}
