namespace bibliotecaPDF.DTOs;

public record PdfFileDTO(string? FileName, bool? IsFavorite, long? fileSize);
