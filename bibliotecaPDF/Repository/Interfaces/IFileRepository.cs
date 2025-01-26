using bibliotecaPDF.Models;
using NpgsqlTypes;

namespace bibliotecaPDF.Repository.Interfaces;

public interface IFileRepository
{
    Task<PdfFile?> GetFileById(int id, User user);

    Task<PdfFile?> GetFileByName(string name, User user);
    Task CreateFile(PdfFile file);
    Task<List<PdfFile>> GetFilesByUser(User user);
    Task DeleteFileByIdAndUser(int id, User user);
    Task SetFavoriteFileById(int id, User user);

    Task SetUnfavoriteFileById(int id, User user);
    Task<NpgsqlTsVector?> GetTsVectorByConcatString(params string[] stringFields);
    Task<List<PdfFile>> GetPDFsBySearch(string searchTerm, User user);
    Task<List<PdfFile>> GetPublicPDFsBySearch(string searchTerm);
}
