using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace InterestingBlogWebApp.Infrastructures.Repositories
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        private readonly AppDbContext _appDbContext;

        public CommentRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<int> GetAllCommentsCount()
        {
            return await _appDbContext.Comments.CountAsync();
        }

        public async Task<int> GetCommentsCountForMonth(int month, int year)
        {
            return await _appDbContext.Comments
                .Where(c => c.CreatedDate.Month == month && c.CreatedDate.Year == year)
                .CountAsync();
        }
    }
}
