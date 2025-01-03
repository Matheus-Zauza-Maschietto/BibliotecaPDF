using System.Collections;
using B2Net.Models;
using bibliotecaPDF.Models.Exceptions;
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
    
    public async Task<B2File?> UploadFile(ICollection<IFormFile> formFiles, string userId)
    {
        IFormFile formFile = formFiles.First(p => p.Name == "formFile");
        IFormFile? customName = formFiles.FirstOrDefault(p => p.Name == "customName");
        byte[] fileBytes = await GetByteArrayFromFormFile(formFile);
        var file = await _backBlazeRepository.UploadB2File(
            fileBytes, 
            customName is not null ? GetCustomNameFromFormFile(customName) : formFile.FileName, 
            new Dictionary<string, string>()
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

    private string GetCustomNameFromFormFile(IFormFile customName)
    {
        using (var reader = new StreamReader(customName.OpenReadStream()))
        {
            return reader.ReadToEnd();
        }
    }
    
    
}