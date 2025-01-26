using B2Net.Models;
using bibliotecaPDF.DTOs;
using bibliotecaPDF.Models;
using bibliotecaPDF.Models.Exceptions;
using bibliotecaPDF.Repository.Interfaces;
using bibliotecaPDF.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

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

    public async Task<PdfFile?> UpdateFileById(int id, UpdatePdfFileDTO fileDto, string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);

        PdfFile? pdfFile = await _fileRepository.GetFileById(id, user);

        if(pdfFile is null)
            throw new BusinessException("PDF não encontrado !");

        pdfFile.UpdatePdfFile(fileDto);
        _genericRepository.Update(pdfFile);
        _genericRepository.SaveChanges();
        return pdfFile;
    }
    
    public async Task DeleteFileById(int id, string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);

        await GetFileById(id, userEmail);
        
        await _fileRepository.DeleteFileByIdAndUser(id, user);
    }

    public async Task<PdfFile?> GetFileByName(string pdfName, string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);
        
        PdfFile? pdfFile = await _fileRepository.GetFileByName(pdfName, user);

        if (pdfFile is null)
        {
            throw new BusinessException("Arquivo PDF não encontrado.");
        }
        
        return pdfFile;
    }
    
    public async Task<PdfFile?> GetFileById(int id, string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);
        
        PdfFile? pdfFile = await _fileRepository.GetFileById(id, user);

        if (pdfFile is null)
        {
            throw new BusinessException("Arquivo PDF não encontrado.");
        }
        
        return pdfFile;
    }

    public async Task<List<PdfFile>> SearchPDFs(string query, string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);
        
        List<PdfFile> pdfFiles = await _fileRepository.GetPDFsBySearch(query, user);
        
        return  pdfFiles;
    }
    
    public async Task<List<PdfFile>> SearchPublicPDFs(string query)
    {
        List<PdfFile> pdfFiles = await _fileRepository.GetPublicPDFsBySearch(query);
        
        return  pdfFiles;
    }

    
    public async Task<GetPdfFileDTO?> GetFileContentById(int id, string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);

        PdfFile? pdfFile = await _fileRepository.GetFileById(id, user);

        if (pdfFile is null)
        {
            throw new BusinessException("Arquivo PDF não encontrado.");
        }

        B2File b2File = await _backBlazeService.DownloadB2File(pdfFile.BackBlazeId);
        return new GetPdfFileDTO(pdfFile.FileName, b2File.FileData);
    }

    public async Task<List<PdfFile>> GetFilesList(string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);

        return  await _fileRepository.GetFilesByUser(user);
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
        string pdfText = ExtractTextFromPdfBytes(fileBytes);
        NpgsqlTsVector? tsVector = await _fileRepository.GetTsVectorByConcatString(b2File.FileName, pdfText); 
        PdfFile file = new PdfFile()
        {
            User = user,
            FileName = b2File.FileName,
            FileSurname   = b2File.FileName,
            BackBlazeId = b2File.FileId,
            FileSize = long.Parse(b2File.ContentLength),
            FileContentTsVector = tsVector,
            IsPublic = false
        };
        
        await _fileRepository.CreateFile(file);
        user.ByteAmounts += long.Parse(b2File.ContentLength);
        _genericRepository.Update(user);
        _genericRepository.SaveChanges();   
    }
    
    private string ExtractTextFromPdfBytes(byte[] pdfBytes)
    {
        using (var memoryStream = new MemoryStream(pdfBytes))
        using (var pdfReader = new PdfReader(memoryStream))
        using (var pdfDocument = new PdfDocument(pdfReader))
        {
            var text = string.Empty;

            for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
            {
                var page = pdfDocument.GetPage(i);
                text += PdfTextExtractor.GetTextFromPage(page);
            }

            return text;
        }
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
    
    public async Task FavoriteFileById(int id, string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);

        await _fileRepository.SetFavoriteFileById(id, user);    
    }

    public async Task UnfavoriteFileById(int id, string userEmail)
    {
        User user = await _userService.GetUserByEmail(userEmail);

        await _fileRepository.SetUnfavoriteFileById(id, user);
    }
}
