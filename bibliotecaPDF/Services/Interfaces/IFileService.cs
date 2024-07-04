using bibliotecaPDF.Models;

namespace bibliotecaPDF.Services.Interfaces;

public interface IFileService
{
    Task DeleteFileByName(string fileName, string userEmail);
    Task<PdfFile?> GetFileByName(string fileName, string userEmail);
    Task<List<string>> GetFilesList(string userEmail);
    Task CreateFile(IFormFile formFile, string userEmail);
    Task FavoriteFileByName(string fileName, string userEmail);
    Task UnfavoriteFileByName(string fileName, string userEmail);
}
