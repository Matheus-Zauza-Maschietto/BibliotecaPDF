using bibliotecaPDF.Models;

namespace bibliotecaPDF.Repository;

public interface IUserRepository
{
    Task<User?> GetUserWithPlanByEmail(string email);
    Task<User?> GetUserByEmailOrUsername(string email);
}