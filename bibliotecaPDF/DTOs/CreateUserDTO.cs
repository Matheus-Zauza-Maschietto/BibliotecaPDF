namespace bibliotecaPDF.DTOs;

public record CreateUserDTO(string Email, string UserName, string? Password = null);
