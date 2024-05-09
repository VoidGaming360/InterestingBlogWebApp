using InterestingBlogWebApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface ICommentLogsheetService
    {
        Task<List<CommentLogsheetDTO>> GetAll();
        Task<List<CommentLogsheetDTO>> GetCommentsLogsByUserId(string id);
        Task<CommentLogsheetDTO> GetCommentLogsheetById(int id);
        Task<List<CommentLogsheetDTO>> GetCommentLogsheetByBlog(int blogId);
        Task<List<CommentLogsheetDTO>> GetCommentLogsheetByComment(int commentId);

    }
}
