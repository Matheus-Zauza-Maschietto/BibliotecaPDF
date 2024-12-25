using B2Net.Models;
using bibliotecaPDF.Repository.Interfaces;
using bibliotecaPDF.Services.Interfaces;

namespace bibliotecaPDF.Services;

public class BackBlazeService : IBackBlazeService
{
    private readonly IBackBlazeRepository _backBlazeRepository;
    private readonly IConfiguration _configuration;

    public BackBlazeService(IBackBlazeRepository backBlazeRepository, IConfiguration configuration)
    {
        _backBlazeRepository = backBlazeRepository;
    }

    public async Task<B2File> DownloadB2File(string fileId)
    {
        return await _backBlazeRepository.DownloadB2File(fileId);
    }

    public async Task<B2File> DeleteB2File(string fileId, string fileName)
    {
        return await _backBlazeRepository.DeleteB2File(fileId, fileName);
    }
    
    public async Task<B2File?> UploadFile(IFormFile formFile, string userId)
    {
        byte[] fileBytes = await GetByteArrayFromFormFile(formFile);
        var file = await _backBlazeRepository.UploadB2File(fileBytes, formFile.FileName, new Dictionary<string, string>()
        {
            { "userId", userId }
        });
        return file;
    }
    
    private async Task<byte[]> GetByteArrayFromFormFile(IFormFile formFile)
    {
        byte[] fileBytes;
        using (var memoryStream = new MemoryStream())
        {
            await formFile.CopyToAsync(memoryStream);
            fileBytes = memoryStream.ToArray();
        }
        return fileBytes;
    }
    
    
}