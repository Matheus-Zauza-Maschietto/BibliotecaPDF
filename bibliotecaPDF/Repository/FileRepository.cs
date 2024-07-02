using bibliotecaPDF.Context;
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

    public async Task<PdfFile>? GetFileByName(string name, User user)
    {
        return _applicationContext.PdfFile.AsNoTracking().FirstOrDefault(p => p.FileName == name && p.User.Id == user.Id);
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
            throw new Exception("Arquivo não encontrado.");
        }
        _applicationContext.PdfFile.Remove(file);
        await _applicationContext.SaveChangesAsync();
    }
}
    