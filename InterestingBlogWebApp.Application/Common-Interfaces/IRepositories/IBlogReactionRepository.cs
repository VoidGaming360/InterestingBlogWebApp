using InterestingBlogWebApp.Domain.Entities;

namespace InterestingBlogWebApp.Application.Common.Interface.IRepositories
{
    public interface IBlogReactionRepository : IRepositoryBase<BlogReaction>
    {
        // Get all votes for a specific blog ID
        Task<IEnumerable<BlogReaction>> GetAllVotesForBlog(int blogId);

        // Get all votes by a specific user
        Task<BlogReaction> GetAllVotesByUserId(string userId);

        // Get a specific vote by blog and user ID
        Task<BlogReaction> GetVote(int blogId, string userId);


    }
}
