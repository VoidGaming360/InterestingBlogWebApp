using InterestingBlogWebApp.Domain.Entities;

namespace InterestingBlogWebApp.Application.Common.Interface.IRepositories
{
    public interface IBlogVoteRepository : IRepositoryBase<BlogVote>
    {
        // Get all votes for a specific blog ID
        Task<IEnumerable<BlogVote>> GetAllVotesForBlog(int blogId);

        // Get all votes by a specific user
        Task<BlogVote> GetAllVotesByUserId(string userId);

        // Get a specific vote by blog and user ID
        Task<BlogVote> GetVote(int blogId, string userId);


    }
}
