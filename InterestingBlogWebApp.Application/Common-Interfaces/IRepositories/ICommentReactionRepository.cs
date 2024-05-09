using InterestingBlogWebApp.Domain.Entities;

namespace InterestingBlogWebApp.Application.Common.Interface.IRepositories
{
    public interface ICommentReactionRepository : IRepositoryBase<CommentRecord>
    {
        // Get all votes for a specific comment ID
        Task<IEnumerable<CommentRecord>> GetAllVotesForComment(int commentId);

        // Get all votes by a specific user
        Task<CommentRecord> GetAllVotesByUserId(string userId);

        // Get a specific vote by comment and user ID
        Task<CommentRecord> GetVote(int commentId, string userId);
    }
}
