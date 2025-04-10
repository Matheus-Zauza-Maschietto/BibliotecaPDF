using bibliotecaPDF.Models;

namespace bibliotecaPDF.DTOs;

public class MessageGetDTO
{
    public string UserName { get; set; }
    public string Text { get; set; }
    public DateTime DateTime { get; set; }

    public MessageGetDTO()
    {
        
    }

    public MessageGetDTO(Message message)
    {
        UserName = message.User.Name;
        Text = message.Text;
        DateTime = message.DateTime;
    }
}