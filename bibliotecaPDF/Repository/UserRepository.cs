using bibliotecaPDF.Context;
using bibliotecaPDF.Models;
using Microsoft.EntityFrameworkCore;

namespace bibliotecaPDF.Repository;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<User?> GetUserWithPlanByEmail(string email)
    {
        return await _context.Users
            .Include(p => p.CapacityPlan)
            .FirstOrDefaultAsync(p => p.Email == email); 
    }

    public async Task<User?> GetUserByEmailOrUsername(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(p => p.Email == email || p.UserName == email);
    }
}