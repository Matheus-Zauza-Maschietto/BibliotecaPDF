using bibliotecaPDF.Context;
using bibliotecaPDF.Exceptions;
using bibliotecaPDF.Models;
using bibliotecaPDF.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bibliotecaPDF.Repository;

public class FileRepository: IFileRepository
{
    private readonly ApplicationDbContext _applicationContext;
    public FileRepository(ApplicationDbContext context)
    {
        _applicationContext = context;
    }

    public async Task<PdfFile?> GetFileByName(string name, User user)
    {
        PdfFile? file = await _applicationContext.PdfFile.AsNoTracking().FirstOrDefaultAsync(p => p.FileName == name && p.User.Id == user.Id);
        if (file == null)
        {
            throw new NotFoundException("File Not Found.");
        }
        return file;
    }

    public async Task<List<string>> GetFilesByUser(User user)
    {
        return await _applicationContext.PdfFile.AsNoTracking().Where(p => p.User == user).Select(p => p.FileName).ToListAsync();
    }

    public async Task CreateFile(PdfFile file)
    {
        _applicationContext.PdfFile.Add(file);
        await _applicationContext.SaveChangesAsync();
    }

    public async Task DeleteFileByNameAndUser(string fileName, User user)
    {
        PdfFile? file = _applicationContext.PdfFile.FirstOrDefault(p => p.FileName == fileName && p.User.Id == user.Id);
        if(file is null)
        {
            throw new NotFoundException("File Not Found.");
        }
        _applicationContext.PdfFile.Remove(file);
        _applicationContext.SaveChangesAsync();
    }

    public async Task SetFavoriteFileByName(string name, User user)
    {
        PdfFile? file = _applicationContext.PdfFile.FirstOrDefault(p => p.FileName == name && p.User.Id == user.Id);
        if (file is null)
        {
            throw new NotFoundException("File Not Found.");
        }

        if (file.IsFavorite == true)
        {
            return;
        }

        file.IsFavorite = true;
        _applicationContext.SaveChangesAsync();
    }

    public async Task SetUnfavoriteFileByName(string name, User user)
    {
        PdfFile? file = _applicationContext.PdfFile.FirstOrDefault(p => p.FileName == name && p.User.Id == user.Id);
        if (file is null)
        {
            throw new NotFoundException("Arquivo não encontrado.");
        }

        if (file.IsFavorite == false)
        {
            return;
        }

        file.IsFavorite = false;
        _applicationContext.SaveChangesAsync();
    }
}
    