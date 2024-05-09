using InterestingBlogWebApp.Application.Common_Interfaces;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace InterestingBlogWebApp.Infrastructure.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly ApplicationDbContext _context;

        public BlogRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Blogs> GetById(int id) // Update return type to Task<Blog>
        {
            return await _context.Blog.FindAsync(id);
        }

        public async Task<List<Blogs>> GetAll(string userId)
        {
            return await _context.Blog
                .Where(blog => blog.ApplicationUserId == userId) // Filter by user ID if needed
                .ToListAsync();
        }

        public async Task Add(Blogs blog)
        {
            await _context.Blog.AddAsync(blog);
        }

        public async Task Update(Blogs blog)
        {
            _context.Entry(blog).State = EntityState.Modified;
            await _context.SaveChangesAsync(); // Save changes to the database
        }

        public async Task Delete(Blogs blog)
        {
            _context.Blog.Remove(blog);
            await _context.SaveChangesAsync(); // Save changes to the database
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
