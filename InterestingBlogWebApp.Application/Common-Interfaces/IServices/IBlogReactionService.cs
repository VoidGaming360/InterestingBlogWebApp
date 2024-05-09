using InterestingBlogWebApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface IBlogReactionService
    {
        Task<List<BlogReactionDTO>> GetAll();
        Task<List<BlogReactionDTO>> GetBlogVotesByUserId(string id);
        Task<string> UpvoteBlog(UpvoteBlogDTO blogvote, List<string> errors);
        Task<string> DownvoteBlog(DownvoteBlogDTO blogvote, List<string> errors);
        Task<BlogReactionDTO> GetVoteById(int voteId);
        Task<IEnumerable<BlogReactionDTO>> GetAllVotesForBlog(int blogId); 
        Task<int> CalculateBlogPopularity(int blogId);
        Task<BlogReactionDTO> GetVote(int blogId, string userId); 

    }
}
