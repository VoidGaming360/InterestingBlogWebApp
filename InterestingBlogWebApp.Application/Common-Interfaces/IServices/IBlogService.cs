using InterestingBlogWebApp.Application.DTOs;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface IBlogService
    {
        Task<List<BlogDTO>> GetAll();
        Task<List<BlogDTO>> GetBlogsByUserId(string id);
        Task<string> AddBlog(AddBlogDTO blog, List<string> errors);
        Task<string> DeleteBlog(int id,List<string> errors);
        Task<string> UpdateBlog(UpdateBlogDTO updateBlogDTO, List<string> errors);
        Task<BlogDTO> GetBlogById(int blogId);
        Task<string> GetUserNameById(string userId);


    }
}
