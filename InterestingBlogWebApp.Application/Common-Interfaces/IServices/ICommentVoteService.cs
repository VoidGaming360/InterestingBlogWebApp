using InterestingBlogWebApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface ICommentVoteService
    {
        Task<List<CommentVoteDTO>> GetAll();
        Task<List<CommentVoteDTO>> GetCommentVotesByUserId(string id);
        Task<string> UpvoteComment(UpvoteCommentDTO commentvote, List<string> errors);
        Task<string> DownvoteComment(DownvoteCommentDTO commentvote, List<string> errors);
        Task<CommentVoteDTO> GetCommentVoteById(int voteId);
        Task<IEnumerable<CommentVoteDTO>> GetAllVotesForComment(int commentId);
        Task<CommentVoteDTO> GetCommentVote(int commentId, string userId);
    }
}
