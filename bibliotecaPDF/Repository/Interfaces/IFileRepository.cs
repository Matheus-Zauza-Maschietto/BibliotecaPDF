using bibliotecaPDF.Models;

namespace bibliotecaPDF.Repository.Interfaces;

public interface IFileRepository
{
    Task<PdfFile?> GetFileByName(string name, User user);
    Task CreateFile(PdfFile file);
    Task<List<PdfFile>> GetFilesByUser(User user);
    Task DeleteFileByNameAndUser(string fileName, User user);
    Task SetFavoriteFileByName(string name, User user);

    Task SetUnfavoriteFileByName(string name, User user);
}
