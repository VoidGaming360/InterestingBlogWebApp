using cloudscribe.Pagination.Models;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.Collections;

namespace InterestingBlogWebApp.Application.Interfaces
{
    public interface IBlog
    {
        Task<List<BlogDTO>> GetAll();
        Task<List<BlogDTO>> GetBlogsByUserId(string id);
        Task<string> AddBlog(AddBlogDTO blog, List<string> errors);
        Task<string> DeleteBlog(int id, List<string> errors);
        Task<string> UpdateBlog(UpdateBlogDTO updateBlogDTO, List<string> errors);
        Task<BlogDTO> GetBlogById(int blogId);

    }
}
