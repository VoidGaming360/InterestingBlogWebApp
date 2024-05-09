using InterestingBlogWebApp.Domain.Shared;
using System.Linq.Expressions;

namespace InterestingBlogWebApp.Application.Helpers
{
    public class GetRequest<T> where T : class
    {
        public Expression<Func<T, bool>> Filter { get; set; }
        public Func<IQueryable<T>, IOrderedQueryable<T>> OrderBy { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }

    }
}
