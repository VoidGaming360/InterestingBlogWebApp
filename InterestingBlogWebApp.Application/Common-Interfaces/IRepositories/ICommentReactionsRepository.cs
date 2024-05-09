using InterestingBlogWebApp.Domain.Entities;

namespace InterestingBlogWebApp.Application.Common.Interface.IRepositories
{
    public interface ICommentVoteRepository : IRepositoryBase<CommentVote>
    {
        // Get all votes for a specific comment ID
        Task<IEnumerable<CommentVote>> GetAllVotesForComment(int commentId);

        // Get all votes by a specific user
        Task<CommentVote> GetAllVotesByUserId(string userId);

        // Get a specific vote by comment and user ID
        Task<CommentVote> GetVote(int commentId, string userId);
    }
}
