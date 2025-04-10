using System.ComponentModel.DataAnnotations;
using bibliotecaPDF.DTOs;

namespace bibliotecaPDF.Models;

public class Message
{
    [Key]
    public int Id { get; set; }
    public User User { get; set; }
    [MaxLength(5000)]
    public string Text { get; set; }
    public DateTime DateTime { get; set; }

    public Message()
    {
        
    }

    public Message(User user, MessageCreateDTO messageCreate)
    {
        User = user;    
        Text = messageCreate.Text;
        DateTime = messageCreate.DateTime;
    }
}