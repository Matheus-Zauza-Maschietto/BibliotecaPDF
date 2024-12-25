using B2Net;
using B2Net.Models;
using bibliotecaPDF.Repository.Interfaces;

namespace bibliotecaPDF.Repository;

public class BackBlazeRepository : IBackBlazeRepository
{
    private readonly IB2Client _backBlazeClient;
    private readonly IConfiguration _configuration;

    public BackBlazeRepository(IB2Client backBlazeClient, IConfiguration configuration)
    {
        _backBlazeClient = backBlazeClient;
        _configuration = configuration;
        _backBlazeClient.Authorize().Wait();
    }

    public async Task<B2File> UploadB2File(byte[] fileContent, string fileName, Dictionary<string, string> formInfo = null)
    {
        return await _backBlazeClient.Files.Upload(fileContent, fileName, _configuration["BackBlaze:BucketId"], formInfo);
    }

    public async Task<B2File> DownloadB2File(string fileId)
    {
        return await _backBlazeClient.Files.DownloadById(fileId);
    }

    public async Task<B2File> DeleteB2File(string fileId, string fileName)
    {
        return await _backBlazeClient.Files.Delete(fileId, fileName);
    }
    
}