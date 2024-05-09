using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InterestingBlogWebApp.Infrastructures.Repositories
{
    public class CommentReactionRepository : RepositoryBase<CommentReaction>, ICommentReactionRepository
    {
        private readonly AppDbContext _appDbContext;

        public CommentReactionRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<CommentReaction>? GetVote(int commentId, string userId)
        {
            return await _appDbContext.CommentVotes
                    .Where(v => v.CommentId == commentId && v.UserId == userId)
                    .SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<CommentReaction>> GetAllVotesForComment(int commentId)
        {
            return await _appDbContext.CommentVotes
                .Where(v => v.CommentId == commentId) 
                .ToListAsync(); 
        }

        public async Task<CommentReaction>? GetAllVotesByUserId(string userId)
        {
            return await _appDbContext.FindAsync<CommentReaction>(userId);
        }
    }
}
