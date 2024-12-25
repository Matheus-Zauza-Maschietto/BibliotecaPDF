using B2Net.Models;
using bibliotecaPDF.Context;
using bibliotecaPDF.DTOs;
using bibliotecaPDF.Exceptions;
using bibliotecaPDF.Models;
using bibliotecaPDF.Repository.Interfaces;
using bibliotecaPDF.Services.Interfaces;

namespace bibliotecaPDF.Services;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;
    private readonly IUserRepository _userRepository;
    private readonly IBackBlazeService _backBlazeService;
    public FileService(IFileRepository fileRepository, IUserRepository userRepository, IBackBlazeService backBlazeService)
    {
        _fileRepository = fileRepository;
        _userRepository = userRepository;
        _backBlazeService = backBlazeService;
    }

    public async Task DeleteFileByName(string fileName, string userEmail)
    {
        User? user = await _userRepository.GetUserByEmail(userEmail);
        if (user == null)
        {
            throw new BussinessException("User not found, logout and login");
        }
        await _fileRepository.DeleteFileByNameAndUser(fileName, user);
    }

    public async Task<PdfFile?> GetFileByName(string fileName, string userEmail)
    {
        User? user = await _userRepository.GetUserByEmail(userEmail);
        if (user == null)
        {
            throw new BussinessException("User not found, logout and login");
        }
        
        PdfFile? pdfFile = await _fileRepository.GetFileByName(fileName, user);

        if (pdfFile is null)
        {
            throw new BussinessException("PDF file not found");
        }
        
        return pdfFile;
    }
    
    public async Task<GetPdfFileDTO?> GetFileContentByName(string fileName, string userEmail)
    {
        User? user = await _userRepository.GetUserByEmail(userEmail);
        if (user == null)
        {
            throw new BussinessException("User not found, logout and login");
        }
        
        PdfFile? pdfFile = await _fileRepository.GetFileByName(fileName, user);

        if (pdfFile is null)
        {
            throw new BussinessException("PDF file not found");
        }

        B2File b2File = await _backBlazeService.DownloadB2File(pdfFile.BackBlazeId);
        return new GetPdfFileDTO(pdfFile.FileName, b2File.FileData);
    }

    public async Task<List<string>> GetFilesList(string userEmail)
    {
        User? user = await _userRepository.GetUserByEmail(userEmail);
        if (user is null)
        {
            throw new BussinessException("User not found, logout and login");
        }

        return  await _fileRepository.GetFilesByUser(user);
    }

    public async Task CreateFile(IFormFile formFile, string userEmail)
    {
        if (formFile == null || formFile.Length == 0)
        {
            throw new BussinessException("Empty File");
        }

        User? user = await _userRepository.GetUserByEmail(userEmail);

        if(user is null)
        {
            throw new BussinessException("User not found, logout and login");
        }

        B2File? b2File = await _backBlazeService.UploadFile(formFile, user.Id);
        
        PdfFile file = new PdfFile()
        {
            User = user,
            FileName = formFile.FileName,
            BackBlazeId = b2File.FileId,
            FileSize = b2File.Size
        };

        await _fileRepository.CreateFile(file);
    }
    

    public async Task FavoriteFileByName(string fileName, string userEmail)
    {
        User? user = await _userRepository.GetUserByEmail(userEmail);
        if (user is null)
        {
            throw new BussinessException("User not found, logout and login");
        }

        await _fileRepository.SetFavoriteFileByName(fileName, user);    
    }

    public async Task UnfavoriteFileByName(string fileName, string userEmail)
    {
        User? user = await _userRepository.GetUserByEmail(userEmail);
        if (user is null)
        {
            throw new BussinessException("User not found, logout and login");
        }

        await _fileRepository.SetUnfavoriteFileByName(fileName, user);
    }
}
