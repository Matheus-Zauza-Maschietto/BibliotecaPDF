using bibliotecaPDF.Enums;
using FluentValidation.Results;

namespace bibliotecaPDF.DTOs;

public class ResponseDTO
{
    public Status Status { get; set; }
    public List<string> Messages { get; set; } = new List<string>();
    public object? Response { get; set; }

    public ResponseDTO(Status status, object response)
    {
        Status = status;    
        Response = response;    
    }
    
    public ResponseDTO(Status status, string message, object? response = null)
    {
        Status = status;
        Messages.Add(message);
        Response = response;
    }
    
    public ResponseDTO(Status status, List<string> messages, object? response = null)
    {
        Status = status;
        Messages = messages;
        Response = response;
    }
    
    public ResponseDTO(Status status, List<ValidationFailure> messages, object? response = null)
    {
        Status = status;
        Messages = messages.Select(p => p.ErrorMessage).ToList();
        Response = response;
    }
    
}