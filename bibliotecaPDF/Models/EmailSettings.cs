namespace bibliotecaPDF.Models;

public class EmailSettings
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string SmtpServer { get; set; }
    public int Port { get; set; }
    public bool UseSSL { get; set; }
}