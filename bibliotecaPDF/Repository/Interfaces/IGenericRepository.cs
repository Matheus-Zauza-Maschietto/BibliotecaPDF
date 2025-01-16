namespace bibliotecaPDF.Repository.Interfaces;

public interface IGenericRepository
{
    T Add<T>(T objectToAdd) where T : class;
    T Update<T>(T objectToAdd) where T : class;
    T Delete<T>(T objectToAdd) where T : class;
    T? Get<T>(int id) where T : class;
}