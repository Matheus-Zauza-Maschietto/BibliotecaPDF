using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;

namespace bibliotecaPDF.Services.Interfaces;

public interface IFileService
{
    Task DeleteFileByName(string fileName, string userEmail);
    Task<PdfFile?> GetFileByName(string fileName, string userEmail);

    Task<GetPdfFileDTO?> GetFileContentByName(string fileName, string userEmail);
    Task<List<PdfFileDTO>> GetFilesList(string userEmail);
    Task CreateFile(ICollection<IFormFile> formFiles, string userEmail);
    Task FavoriteFileByName(string fileName, string userEmail);
    Task UnfavoriteFileByName(string fileName, string userEmail);
}
