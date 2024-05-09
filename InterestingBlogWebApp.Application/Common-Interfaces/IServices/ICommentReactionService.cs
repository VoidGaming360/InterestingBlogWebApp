using InterestingBlogWebApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface ICommentReactionService
    {
        Task<List<CommentReactionDTO>> GetAll();
        Task<List<CommentReactionDTO>> GetCommentVotesByUserId(string id);
        Task<string> UpvoteComment(UpvoteCommentDTO commentvote, List<string> errors);
        Task<string> DownvoteComment(DownvoteCommentDTO commentvote, List<string> errors);
        Task<CommentReactionDTO> GetCommentVoteById(int voteId);
        Task<IEnumerable<CommentReactionDTO>> GetAllVotesForComment(int commentId);
        Task<CommentReactionDTO> GetCommentVote(int commentId, string userId);
    }
}
