using bibliotecaPDF.Context;
using bibliotecaPDF.Models;
using bibliotecaPDF.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace bibliotecaPDF.Repository;

public class GenericRepository : IGenericRepository
{
    private readonly ApplicationDbContext _context;

    public GenericRepository(ApplicationDbContext context)
    {
        _context = context; 
    }

    public T Add<T>(T objectToAdd) where T : class
    {
        return _context.Set<T>().Add(objectToAdd).Entity;
    }
    
    public T Update<T>(T objectToAdd) where T : class
    {
        return _context.Set<T>().Update(objectToAdd).Entity;
    }
    
    public T Delete<T>(T objectToAdd) where T : class
    {
        return _context.Set<T>().Remove(objectToAdd).Entity;
    }
    
    public T? Get<T>(int id) where T : class
    {
        return _context.Set<T>().Find(id);
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() > 0;
    }
}