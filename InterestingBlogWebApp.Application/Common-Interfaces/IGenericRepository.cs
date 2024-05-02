using System.Linq.Expressions;

namespace InterestingBlogWebApp.Application.Interfaces
{
    public interface IGenericRepositories<T> : IDisposable
    {
        IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "");
        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        void Add(T entity);
        Task<T> AddAsync(T entity);
        void Update(T entity);
        Task<T> DeleteAsync(T entity);
        void Delete(T entity);
        int Count(Expression<Func<T, bool>> predicate = null);
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        Task SaveChangesAsync();
    }
}
