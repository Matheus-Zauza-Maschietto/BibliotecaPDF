using bibliotecaPDF.Models;

namespace bibliotecaPDF.DTOs;

public class PdfFileDTO
{
    public int? Id { get; set; }
    public string? FileName { get; set; }
    public bool? IsFavorite { get; set; }
    public long? FileSize { get; set; }
    public bool? IsPublic { get; set; }
    
    public PdfFileDTO()
    {
        
    }
    
    public PdfFileDTO(PdfFile fileDto)
    {
        Id = fileDto.Id;
        FileName = fileDto.FileSurname;
        IsFavorite = fileDto.IsFavorite;
        FileSize = fileDto.FileSize;
        IsPublic = fileDto.IsPublic;
    }
}
