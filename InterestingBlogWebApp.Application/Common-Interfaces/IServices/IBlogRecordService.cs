using InterestingBlogWebApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface IBlogRecordService
    {
        Task<List<BlogRecordDTO>> GetAll();
        Task<List<BlogRecordDTO>> GetBlogsLogsByUserId(string id);
        Task<BlogRecordDTO> GetBlogLogsheetById(int id);
        Task<List<BlogRecordDTO>> GetBlogLogsheetByBlog(int blogId);
    }
}
