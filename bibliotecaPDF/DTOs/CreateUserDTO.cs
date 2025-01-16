namespace bibliotecaPDF.DTOs;

public record CreateUserDTO(string Email, string Name, string? Password = null, string? ConfirmPassword = null);
