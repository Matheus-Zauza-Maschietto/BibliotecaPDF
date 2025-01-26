using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;

namespace bibliotecaPDF.Services.Interfaces;

public interface IFileService
{
    Task DeleteFileById(int id, string userEmail);
    Task<PdfFile?> GetFileById(int id, string userEmail);

    Task<GetPdfFileDTO?> GetFileContentById(int id, string userEmail);
    Task<List<PdfFile>> GetFilesList(string userEmail);
    Task CreateFile(IFormFile? formFile, string userEmail);
    Task FavoriteFileById(int id, string userEmail);
    Task UnfavoriteFileById(int id, string userEmail);
    Task<List<PdfFile>> SearchPDFs(string query, string userEmail);
    Task<List<PdfFile>> SearchPublicPDFs(string query);
    Task<PdfFile?> UpdateFileById(int id, UpdatePdfFileDTO fileDto, string userEmail);

}
