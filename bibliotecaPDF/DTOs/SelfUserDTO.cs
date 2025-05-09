using bibliotecaPDF.Models;

namespace bibliotecaPDF.DTOs;

public record SelfUserDTO(string Name, string Email, string Telefone, double CapacityUsed)
{
    
    public SelfUserDTO(User user) : this(user.Name, user.Email, user.PhoneNumber, ((double)user.ByteAmounts / (double)user.CapacityPlan.BytesCapacity) * 100)
    {
    }
};