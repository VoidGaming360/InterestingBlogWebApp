using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InterestingBlogWebApp.Infrastructures.Repositories
{
    public class BlogVoteRepository : RepositoryBase<BlogVote>, IBlogVoteRepository
    {
        private readonly AppDbContext _appDbContext;

        public BlogVoteRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<BlogVote>? GetVote(int blogId, string userId)
        {
            return await _appDbContext.BlogVotes
                    .Where(v => v.BlogId == blogId && v.UserId == userId)
                    .SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<BlogVote>> GetAllVotesForBlog(int blogId)
        {
            // Ensure this method returns all votes for the specified blog ID
            return await _appDbContext.BlogVotes
                .Where(v => v.BlogId == blogId) // Ensure filtering by blog ID
                .ToListAsync(); // Return as a collection
        }

        public async Task<BlogVote>? GetAllVotesByUserId(string userId)
        {
            return await _appDbContext.FindAsync<BlogVote>(userId);
        }
    }
}
