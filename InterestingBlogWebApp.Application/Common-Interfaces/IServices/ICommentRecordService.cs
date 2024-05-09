using InterestingBlogWebApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface ICommentRecordService
    {
        Task<List<CommentRecordDTO>> GetAll();
        Task<List<CommentRecordDTO>> GetCommentsLogsByUserId(string id);
        Task<CommentRecordDTO> GetCommentLogsheetById(int id);
        Task<List<CommentRecordDTO>> GetCommentLogsheetByBlog(int blogId);
        Task<List<CommentRecordDTO>> GetCommentLogsheetByComment(int commentId);

    }
}
