using InterestingBlogWebApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface IBlogVoteService
    {
        Task<List<BlogVoteDTO>> GetAll();
        Task<List<BlogVoteDTO>> GetBlogVotesByUserId(string id);
        Task<string> UpvoteBlog(UpvoteBlogDTO blogvote, List<string> errors);
        Task<string> DownvoteBlog(DownvoteBlogDTO blogvote, List<string> errors);
        Task<BlogVoteDTO> GetVoteById(int voteId);
        Task<IEnumerable<BlogVoteDTO>> GetAllVotesForBlog(int blogId); 
        Task<int> CalculateBlogPopularity(int blogId);
        Task<BlogVoteDTO> GetVote(int blogId, string userId); 

    }
}
