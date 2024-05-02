using InterestingBlogWebApp.Application.Interfaces;
using InterestingBlogWebApp.Infrastructure.Persistence;


namespace InterestingBlogWebApp.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    private bool disposed = false;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }

    public IGenericRepositories<T> GenericRepositories<T>() where T : class
    {
        IGenericRepositories<T> repo = new GenericRepository<T>(_context);
        return repo;
    }

    public void Save()
    {
        _context.SaveChanges();
    }

    public async Task<int> SaveChangesAsync() // Implement this method
    {
        return await _context.SaveChangesAsync();
    }


}
