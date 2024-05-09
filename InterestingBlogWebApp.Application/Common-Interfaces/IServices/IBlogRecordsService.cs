using InterestingBlogWebApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface IBlogLogsheetService
    {
        Task<List<BlogLogsheetDTO>> GetAll();
        Task<List<BlogLogsheetDTO>> GetBlogsLogsByUserId(string id);
        Task<BlogLogsheetDTO> GetBlogLogsheetById(int id);
        Task<List<BlogLogsheetDTO>> GetBlogLogsheetByBlog(int blogId);
    }
}
