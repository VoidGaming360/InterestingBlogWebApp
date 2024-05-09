using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructure.Persistence;

namespace InterestingBlogWebApp.Infrastructures.Repositories
{
    public class BlogRepository : RepositoryBase<Blog>, IBlogRepository
    {
        private readonly AppDbContext _appDbContext;

        public BlogRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }
    }
}
