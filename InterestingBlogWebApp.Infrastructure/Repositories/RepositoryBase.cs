using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Application.Helpers;
using InterestingBlogWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InterestingBlogWebApp.Infrastructures.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        //The context is added in Step 5.1
        private readonly AppDbContext _context;

        public RepositoryBase(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> Add(T entity)
        {
            var addedEntity = (await _context.AddAsync(entity)).Entity;
            _context.SaveChanges();
            return addedEntity;
        }

        public async Task Delete(T entity)
        {
            if (entity != null)
            {
                _context.Remove(entity); // Remove the entity
                await _context.SaveChangesAsync(); // Commit asynchronously
            }
        }

        public async Task<IEnumerable<T>> GetAll(GetRequest<T> request)
        {
            IQueryable<T> query = _context.Set<T>();

            if (request != null)
            {
                if (request.Filter != null)
                {
                    query = query.Where(request.Filter);
                }

                if (request.OrderBy != null)
                {
                    query = request.OrderBy(query);
                }

                if (request.Skip.HasValue)
                {
                    query = query.Skip(request.Skip.Value);
                }

                if (request.Take.HasValue)
                {
                    query = query.Take(request.Take.Value);
                }
            }

            return await query.ToListAsync(); // Use asynchronous database operations
        }


        public async Task<T>? GetById(object entityId)
        {
            return await _context.FindAsync<T>(entityId);
        }

        

        public async Task<T> Update(T entity)
        {
            var updatedEntity = _context.Update(entity).Entity;
            await _context.SaveChangesAsync();
            return updatedEntity;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync(); // Commit changes to the database
        }
    }

}
