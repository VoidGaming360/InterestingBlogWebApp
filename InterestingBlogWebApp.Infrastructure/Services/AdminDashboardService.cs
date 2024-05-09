using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Infrastructure.Services
{
    public class AdminDashboardService : IAdminDashboardService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;

        public AdminDashboardService(
            IBlogRepository blogRepository,
            ICommentRepository commentRepository)
        {
            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
        }

        public async Task<AdminDashboardDataDTO> GetDashboardData(int month, int year)
        {
            var allBlogs = await _blogRepository.GetAll(null);

            var monthStart = new DateTime(year, month, 1);
            var monthEnd = monthStart.AddMonths(1).AddSeconds(-1);

            var monthBlogs = allBlogs
                .Where(b => b.CreatedDate >= monthStart && b.CreatedDate <= monthEnd)
                .ToList();

            var allTimeBlogCount = allBlogs.Count();
            var monthBlogCount = monthBlogs.Count();

            var allTimeUpvoteCount = allBlogs.Sum(b => b.UpVoteCount ?? 0);
            var monthUpvoteCount = monthBlogs.Sum(b => b.UpVoteCount ?? 0);

            var allTimeDownvoteCount = allBlogs.Sum(b => b.DownVoteCount ?? 0);
            var monthDownvoteCount = monthBlogs.Sum(b => b.DownVoteCount ?? 0);

            var allTimeCommentCount = await _commentRepository.GetAllCommentsCount();
            var monthCommentCount = await _commentRepository.GetCommentsCountForMonth(month, year);

            return new AdminDashboardDataDTO
            {
                AllTimeBlogCount = allTimeBlogCount,
                MonthBlogCount = monthBlogCount,
                AllTimeUpvoteCount = allTimeUpvoteCount,
                MonthUpvoteCount = monthUpvoteCount,
                AllTimeDownvoteCount = allTimeDownvoteCount,
                MonthDownvoteCount = monthDownvoteCount,
                AllTimeCommentCount = allTimeCommentCount,
                MonthCommentCount = monthCommentCount
            };
        }
    }
}
