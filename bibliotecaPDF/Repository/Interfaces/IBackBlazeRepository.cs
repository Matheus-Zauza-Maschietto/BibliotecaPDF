using B2Net.Models;

namespace bibliotecaPDF.Repository.Interfaces;

public interface IBackBlazeRepository
{
    Task<B2File> UploadB2File(byte[] fileContent, string fileName, Dictionary<string, string> formInfo = null);
    Task<B2File> DownloadB2File(string fileId);
    Task<B2File> DeleteB2File(string fileId, string fileName);
}