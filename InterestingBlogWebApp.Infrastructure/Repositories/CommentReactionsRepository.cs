using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InterestingBlogWebApp.Infrastructures.Repositories
{
    public class CommentVoteRepository : RepositoryBase<CommentVote>, ICommentVoteRepository
    {
        private readonly AppDbContext _appDbContext;

        public CommentVoteRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<CommentVote>? GetVote(int commentId, string userId)
        {
            return await _appDbContext.CommentVotes
                    .Where(v => v.CommentId == commentId && v.UserId == userId)
                    .SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<CommentVote>> GetAllVotesForComment(int commentId)
        {
            return await _appDbContext.CommentVotes
                .Where(v => v.CommentId == commentId) 
                .ToListAsync(); 
        }

        public async Task<CommentVote>? GetAllVotesByUserId(string userId)
        {
            return await _appDbContext.FindAsync<CommentVote>(userId);
        }
    }
}
