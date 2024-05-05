using InterestingBlogWebApp.Application.DTOs;
using System.Collections;

namespace InterestingBlogWebApp.Application.Interfaces
{
    public interface IBlog
    {
        BlogDTO GetById(int BlogId);
        public void CreateBlog(BlogDTO blog);
        public void UpdateBlog(BlogDTO blog);
        public void DeleteBlog(int id);
        IEnumerable<BlogDTO> GetAll();
    }
}
