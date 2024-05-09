using InterestingBlogWebApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface ICommentService
    {
        Task<List<CommentDTO>> GetAll();
        Task<List<CommentDTO>> GetCommentsByBlogId(int id);
        Task<List<CommentDTO>> GetCommentsByUserId(string id);
        Task<string> AddComment(AddCommentDTO comment, List<string> errors);
        Task<string> DeleteComment(int id, List<string> errors);
        Task<string> UpdateComment(UpdateCommentDTO updateCommentDTO, List<string> errors);
        Task<CommentDTO> GetCommentById(int commentId);
    }
}
