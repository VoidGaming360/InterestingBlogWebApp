using BisleriumProject.Application.DTOs;
using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Application.Helpers;
using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BisleriumProject.Infrastructures.Services
{
    public class AdminService : IAdminDashboardService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public AdminService(
            IBlogRepository blogRepository,
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            UserManager<User> userManager)
        {
            _blogRepository = blogRepository;
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<AdminDashboardDataDTO> GetCumulativeCount()
        {
            var allBlogs = await _blogRepository.GetAll(null);
            var allComments = await _commentRepository.GetAll(null);

            var cumulativeCountData = new AdminDashboardDataDTO
            {
                TotalBlogPosts = allBlogs.Count(),
                TotalUpvotes = allBlogs.Sum(b => b.UpVoteCount ?? 0),
                TotalDownvotes = allBlogs.Sum(b => b.DownVoteCount ?? 0),
                TotalComments = allComments.Count()
            };

            return cumulativeCountData;
        }

        public async Task<AdminDashboardDataDTO> GetMonthlyCount(int month, int year)
        {
            var monthStart = new DateTime(year, month, 1).ToUniversalTime();
            var monthEnd = monthStart.AddMonths(1).AddSeconds(-1).ToUniversalTime();

            var monthBlogsRequest = new GetRequest<Blog>
            {
                Filter = b => b.CreatedDate >= monthStart && b.CreatedDate <= monthEnd
            };
            var monthBlogs = await _blogRepository.GetAll(monthBlogsRequest);

            var monthCommentsRequest = new GetRequest<Comment>
            {
                Filter = c => c.CreatedDate >= monthStart && c.CreatedDate <= monthEnd
            };
            var monthComments = await _commentRepository.GetAll(monthCommentsRequest);

            var monthlyCountData = new AdminDashboardDataDTO
            {
                TotalBlogPosts = monthBlogs.Count(),
                TotalUpvotes = monthBlogs.Sum(b => b.UpVoteCount ?? 0),
                TotalDownvotes = monthBlogs.Sum(b => b.DownVoteCount ?? 0),
                TotalComments = monthComments.Count()
            };

            return monthlyCountData;
        }

        public async Task<List<BlogDTO>> GetTopPosts(int month, int year)
        {
            var blogs = await _blogRepository.GetAll(null);

            if (month != 0 && year != 0)
            {
                var monthStart = new DateTime(year, month, 1).ToUniversalTime();
                var monthEnd = monthStart.AddMonths(1).AddSeconds(-1).ToUniversalTime();
                blogs = blogs.Where(b => b.CreatedDate >= monthStart && b.CreatedDate <= monthEnd);
            }

            var topPosts = blogs.OrderByDescending(b => b.Score).Take(10).ToList();

            return topPosts.Select(blog => new BlogDTO
            {
                Id = blog.Id,
                Description = blog.Description,
                CreatedDate = blog.CreatedDate,
                IsEdited = blog.IsEdited,
                Category = blog.Category,
                Image = blog.Image.ToString(),
                Title = blog.Title,
                Score = blog.Score ?? 0,
                UpVoteCount = blog.UpVoteCount ?? 0,
                DownVoteCount = blog.DownVoteCount ?? 0
            }).ToList();
        }

        public async Task<List<UserDTO>> GetTopBloggers(int month, int year)
        {
            var blogs = await _blogRepository.GetAll(null);

            if (month != 0 && year != 0)
            {
                var monthStart = new DateTime(year, month, 1).ToUniversalTime();
                var monthEnd = monthStart.AddMonths(1).AddSeconds(-1).ToUniversalTime();
                blogs = blogs.Where(b => b.CreatedDate >= monthStart && b.CreatedDate <= monthEnd);
            }

            var bloggerScores = blogs.GroupBy(b => b.UserId)
                                     .Select(g => new
                                     {
                                         UserId = g.Key,
                                         TotalScore = g.Sum(b => b.Score ?? 0)
                                     });

            var topBloggers = bloggerScores.OrderByDescending(b => b.TotalScore)
                                           .Take(10)
                                           .ToList();

            var topBloggerIds = topBloggers.Select(b => b.UserId).ToList();
            var topBloggerUsers = await _userManager.Users
                                                    .Where(u => topBloggerIds.Contains(u.Id))
                                                    .ToListAsync();

            return topBloggerUsers.Select(u => new UserDTO
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            }).ToList();
        }
    }
}