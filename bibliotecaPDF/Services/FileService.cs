using bibliotecaPDF.Context;
using bibliotecaPDF.Models;
using bibliotecaPDF.Repository.Interfaces;

namespace bibliotecaPDF.Services;

public class FileService
{
    private readonly IFileRepository _fileRepository;
    private readonly IUserRepository _userRepository;
    public FileService(IFileRepository fileRepository, IUserRepository userRepository)
    {
        _fileRepository = fileRepository;
        _userRepository = userRepository;
    }

    public async Task DeleteFileByName(string fileName, string userEmail)
    {
        User? user = await _userRepository.GetUserByEmail(userEmail);
        if (user == null)
        {
            throw new Exception("User not found, logout and login");
        }
        await _fileRepository.DeleteFileByNameAndUser(fileName, user);
    }

    public async Task<PdfFile> GetFileByName(string fileName, string userEmail)
    {
        User? user = await _userRepository.GetUserByEmail(userEmail);
        if (user == null)
        {
            throw new Exception("User not found, logout and login");
        }
        return await _fileRepository.GetFileByName(fileName, user);
    }

    public async Task<List<string>> GetFilesList(string userEmail)
    {
        User? user = await _userRepository.GetUserByEmail(userEmail);
        if (user is null)
        {
            throw new Exception("User not found, logout and login");
        }

        return  await _fileRepository.GetFilesByUser(user);
    }

    public async Task CreateFile(IFormFile formFile, string userEmail)
    {
        if (formFile == null || formFile.Length == 0)
        {
            throw new Exception("Empty File");
        }

        User? user = await _userRepository.GetUserByEmail(userEmail);

        if(user is null)
        {
            throw new Exception("User not found, logout and login");
        }

        PdfFile file = new PdfFile()
        {
            User = user,
            FileName = formFile.FileName,
            Content = await GetByteArrayFromFormFile(formFile)
        };

        await _fileRepository.CreateFile(file);
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
