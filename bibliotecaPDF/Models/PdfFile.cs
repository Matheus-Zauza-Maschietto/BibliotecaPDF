using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using bibliotecaPDF.DTOs;
using NpgsqlTypes;

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
    
    [MinLength(3)]
    [MaxLength(100)]
    public string FileSurname { get; set; }

    public long FileSize { get; set; }
    public string BackBlazeId { get; set; }
    public bool IsFavorite { get; set; } = false;
    public bool IsPublic { get; set; } = false;
    public NpgsqlTsVector? FileContentTsVector { get; set; }

    public PdfFile()
    {
        
    }

    public PdfFile UpdatePdfFile(UpdatePdfFileDTO updatePdfFile)
    {
        this.FileSurname = updatePdfFile.FileSurname;
        this.IsPublic = updatePdfFile.IsPublic;
        return this;
    }
}
