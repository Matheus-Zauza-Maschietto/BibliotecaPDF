using bibliotecaPDF.Context;
using bibliotecaPDF.Models;
using bibliotecaPDF.Models.Exceptions;
using bibliotecaPDF.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

namespace bibliotecaPDF.Repository;

public class FileRepository: IFileRepository
{
    private readonly ApplicationDbContext _applicationContext;
    public FileRepository(ApplicationDbContext context)
    {
        _applicationContext = context;
    }

    public async Task<List<PdfFile>> GetPublicPDFsBySearch(string searchTerm)
    {
        return _applicationContext.PdfFile
            .Where(p => p.IsPublic == true)
            .Where(p =>
                p.FileContentTsVector.Matches(EF.Functions.PhraseToTsQuery("portuguese", searchTerm))
            )
            .ToList();
    }
    
    public async Task<PdfFile?> GetFileById(int id, User user)
    {
        PdfFile? file = await _applicationContext
            .PdfFile
            .FirstOrDefaultAsync(p => p.Id == id && p.User.Id == user.Id);
        return file;
    }
    public async Task<PdfFile?> GetFileByName(string name, User user)
    {
        PdfFile? file = await _applicationContext
            .PdfFile
            .FirstOrDefaultAsync(p => p.FileName == name && p.User.Id == user.Id);
        return file;
    }

    public async Task<List<PdfFile>> GetFilesByUser(User user)
    {
        return await _applicationContext.PdfFile.AsNoTracking().Where(p => p.User == user).ToListAsync();
    }

    public async Task CreateFile(PdfFile file)
    {
        _applicationContext.PdfFile.Add(file);
        await _applicationContext.SaveChangesAsync();
    }

    public async Task DeleteFileByIdAndUser(int id, User user)
    {
        PdfFile? file = _applicationContext.PdfFile.FirstOrDefault(p => p.Id == id && p.User.Id == user.Id);
        if(file is null)
        {
            throw new BusinessException("Arquivo PDF não encontrado.");
        }
        _applicationContext.PdfFile.Remove(file);
        _applicationContext.SaveChangesAsync();
    }

    public async Task<List<PdfFile>> GetPDFsBySearch(string searchTerm, User user)
    {
        return _applicationContext.PdfFile
            .Where(p => p.User.Id == user.Id)
            .Where(p =>
                p.FileContentTsVector.Matches(EF.Functions.PhraseToTsQuery("portuguese", searchTerm))
            )
            .ToList();
    }
    
    public async Task SetFavoriteFileById(int id, User user)
    {
        PdfFile? file = _applicationContext.PdfFile.FirstOrDefault(p => p.Id == id && p.User.Id == user.Id);
        if (file is null)
        {
            throw new BusinessException("Arquivo PDF não encontrado.");
        }

        if (file.IsFavorite == true)
        {
            return;
        }

        file.IsFavorite = true;
        _applicationContext.SaveChangesAsync();
    }

    public async Task SetUnfavoriteFileById(int id, User user)
    {
        PdfFile? file = _applicationContext.PdfFile.FirstOrDefault(p => p.Id == id && p.User.Id == user.Id);
        if (file is null)
        {
            throw new BusinessException("Arquivo PDF não encontrado.");
        }

        if (file.IsFavorite == false)
        {
            return;
        }

        file.IsFavorite = false;
        _applicationContext.SaveChangesAsync();
    }

    public async Task<NpgsqlTsVector?> GetTsVectorByConcatString(params string[] stringFields)
    {
        string stringFieldConcatened = string.Join(" ", stringFields);
        var query = _applicationContext.PdfFile
            .Select(b2File => new 
            {
                TsVector = EF.Functions.ToTsVector(
                    "portuguese",
                    stringFieldConcatened
                )
            })
            .FirstOrDefault();
        return query?.TsVector;
    }
    
}
    