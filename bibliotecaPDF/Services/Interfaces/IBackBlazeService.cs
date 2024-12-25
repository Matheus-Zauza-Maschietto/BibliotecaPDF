using B2Net.Models;

namespace bibliotecaPDF.Services.Interfaces;

public interface IBackBlazeService
{
    Task<B2File?> UploadFile(IFormFile formFile, string userId);
    Task<B2File> DownloadB2File(string fileId);
    Task<B2File?> DeleteB2File(string fileId, string fileName);
}