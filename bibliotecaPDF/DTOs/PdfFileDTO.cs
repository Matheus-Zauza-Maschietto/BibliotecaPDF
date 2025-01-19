namespace bibliotecaPDF.DTOs;

public record PdfFileDTO(int id, string? FileName, bool? IsFavorite, long? fileSize);
