using InterestingBlogWebApp.Domain.Entities;

namespace InterestingBlogWebApp.Application.Common_Interfaces
{
    public interface IBlogRepository
    {
        Task<Blogs> GetById(int id);

        Task<List<Blogs>> GetAll(string userId);

        Task Add(Blogs blog);

        Task Update(Blogs blog);

        Task Delete(Blogs blog);

        Task SaveChangesAsync();
    }
}
