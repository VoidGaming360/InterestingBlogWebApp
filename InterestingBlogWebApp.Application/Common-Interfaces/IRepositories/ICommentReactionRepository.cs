using InterestingBlogWebApp.Domain.Entities;

namespace InterestingBlogWebApp.Application.Common.Interface.IRepositories
{
    public interface ICommentReactionRepository : IRepositoryBase<CommentReaction>
    {
        // Get all votes for a specific comment ID
        Task<IEnumerable<CommentReaction>> GetAllVotesForComment(int commentId);

        // Get all votes by a specific user
        Task<CommentReaction> GetAllVotesByUserId(string userId);

        // Get a specific vote by comment and user ID
        Task<CommentReaction> GetVote(int commentId, string userId);
    }
}
