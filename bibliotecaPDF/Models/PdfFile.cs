using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bibliotecaPDF.Models;

public class PdfFile
{
    [Key]
    public int Id { get; set; }
    [Required]
    public User User { get; set; }
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public string FileName { get; set; }

    public long FileSize { get; set; }
    public string BackBlazeId { get; set; }
    public bool IsFavorite { get; set; }

    public PdfFile()
    {
        
    }
}
