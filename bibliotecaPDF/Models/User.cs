using Microsoft.AspNetCore.Identity;

namespace bibliotecaPDF.Models;

public class User: IdentityUser
{
    public List<PdfFile> Files { get; set; }
}
