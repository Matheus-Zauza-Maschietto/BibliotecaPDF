using B2Net.Models;
using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;
using bibliotecaPDF.Models.Exceptions;
using bibliotecaPDF.Repository.Interfaces;
using bibliotecaPDF.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace bibliotecaPDF.Services;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;
    private readonly IUserService _userService;
    private readonly IBackBlazeService _backBlazeService;
    private readonly IGenericRepository _genericRepository;
    public FileService(
        IFileRepository fileRepository, 
        IBackBlazeService backBlazeService,
        IUserService userService,
        IGenericRepository genericRepository)
    {
        _userService = userService;
        _fileRepository = fileRepository;
        _backBlazeService = backBlazeService;
        _genericRepository = genericRepository;
    }

    public async Task DeleteFileByName(string fileName, string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);

        await GetFileByName(fileName, userEmail);
        
        await _fileRepository.DeleteFileByNameAndUser(fileName, user);
    }

    public async Task<PdfFile?> GetFileByName(string fileName, string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);
        
        PdfFile? pdfFile = await _fileRepository.GetFileByName(fileName, user);

        if (pdfFile is null)
        {
            throw new BusinessException("Arquivo PDF não encontrado.");
        }
        
        return pdfFile;
    }
    
    public async Task<GetPdfFileDTO?> GetFileContentByName(string fileName, string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);

        PdfFile? pdfFile = await _fileRepository.GetFileByName(fileName, user);

        if (pdfFile is null)
        {
            throw new BusinessException("Arquivo PDF não encontrado.");
        }

        B2File b2File = await _backBlazeService.DownloadB2File(pdfFile.BackBlazeId);
        return new GetPdfFileDTO(pdfFile.FileName, b2File.FileData);
    }

    public async Task<List<PdfFileDTO>> GetFilesList(string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);

        return  (await _fileRepository.GetFilesByUser(user)).Select(p => new PdfFileDTO(p.Id, p.FileName, p.IsFavorite, p.FileSize)).ToList();
    }

    public async Task CreateFile(ICollection<IFormFile> formFiles, string userEmail)
    {
        if (formFiles.FirstOrDefault(p => p.Name == "formFile") == null || formFiles.FirstOrDefault(p => p.Name == "formFile")?.Length == 0)
        {
            throw new BusinessException("Arquivo vazio.");
        }
        User user = await _userService.GetUserByEmail(userEmail);
        IFormFile formFile = formFiles.First(p => p.Name == "formFile");

        PdfFile? pdfFile = await _fileRepository.GetFileByName(formFile.FileName, user);
        if (pdfFile is not null) throw new BusinessException("Você já possui um pdf com esse nome.");
        
        byte[] fileBytes = await GetByteArrayFromFormFile(formFile);

        B2File? b2File = await _backBlazeService.UploadFile(fileBytes, formFile.FileName, user.Id);
        
        PdfFile file = new PdfFile()
        {
            User = user,
            FileName = b2File.FileName,
            BackBlazeId = b2File.FileId,
            FileSize = long.Parse(b2File.ContentLength)
        };
        
        await _fileRepository.CreateFile(file);
        user.ByteAmounts += long.Parse(b2File.ContentLength);
        _genericRepository.Update(user);
        _genericRepository.SaveChanges();   
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
    

    public async Task FavoriteFileByName(string fileName, string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);

        await _fileRepository.SetFavoriteFileByName(fileName, user);    
    }

    public async Task UnfavoriteFileByName(string fileName, string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);

        await _fileRepository.SetUnfavoriteFileByName(fileName, user);
    }
}
