namespace bibliotecaPDF.DTOs;

public record GetPdfFileDTO(string FileName, byte[] FileContent);