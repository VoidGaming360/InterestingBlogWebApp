using cloudscribe.Pagination.Models;
using InterestingBlogWebApp.Application.DTOs;
using System.Collections;

namespace InterestingBlogWebApp.Application.Interfaces
{
    public interface IBlog
    {
        PagedResult<BlogDTO> GetAllForUser(string userId, int pageNumber, int pageSize);
        BlogDTO GetById(int BlogId);
        public void CreateBlog(BlogDTO blog);
        public void UpdateBlog(BlogDTO blog);
        public void DeleteBlog(int id);
        IEnumerable<BlogDTO> GetAll();
    }
}
