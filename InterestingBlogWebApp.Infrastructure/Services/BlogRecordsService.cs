using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Domain.Entities;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Infrastructure.Services
{
    public class BlogLogsheetService : IBlogLogsheetService
    {
        private readonly IBlogLogsheetRepository _blogLogsheetRepository;
        private readonly UserManager<User> _userManager;

        public BlogLogsheetService(IBlogLogsheetRepository blogsLogsheetRepository, UserManager<User> userManager)
        {
            
            _blogLogsheetRepository = blogsLogsheetRepository;
            _userManager = userManager;
        }

        public async Task<List<BlogLogsheetDTO>> GetAll()
        {
            // Retrieve all blog logs associated with the userId
            var blogLogsheets = await _blogLogsheetRepository.GetAll(null); // Get all blog logs


            var blogLogsheetDTOs = new List<BlogLogsheetDTO>();

            foreach (var blogLog in blogLogsheets)
            {
                var user = await _userManager.FindByIdAsync(blogLog.UserId); // Use await to avoid blocking

                var blogLogDTO = new BlogLogsheetDTO
                {
                    Id = blogLog.Id,
                    Description = blogLog.Description,
                    UpdatedAt = blogLog.UpdatedAt,
                    Category = blogLog.Category,
                    Image = blogLog.Image.ToString(),
                    Title = blogLog.Title,
                    BlogId = blogLog.BlogId,
                    UserId = user.Id  // Ensure UserId is passed to the DTO
                };

                blogLogsheetDTOs.Add(blogLogDTO);
            }

            return blogLogsheetDTOs.OrderByDescending(r => r.UpdatedAt).ToList();
        }

        public async Task<BlogLogsheetDTO> GetBlogLogsheetById(int blogLogsheetId)
        {
            var blogLog = await _blogLogsheetRepository.GetById(blogLogsheetId);

            if (blogLog == null)
            {
                throw new KeyNotFoundException("Blog logsheet not found."); // Handle case where blog doesn't exist
            }

            return new BlogLogsheetDTO
            {
                Id = blogLog.Id,
                Title = blogLog.Title,
                Description = blogLog.Description,
                UpdatedAt = blogLog.UpdatedAt,
                Category = blogLog.Category,
                Image = blogLog.Image.ToString(),
                BlogId = blogLog.BlogId,
                UserId = blogLog.UserId
            };
        }

        public async Task<List<BlogLogsheetDTO>> GetBlogsLogsByUserId(string userId)
        {
            var blogLogsheets = await _blogLogsheetRepository.GetAll(null);

            var userBlogLogsheets = blogLogsheets.Where(blog => blog.UserId == userId).ToList();

            var blogLogsheetDTOs = new List<BlogLogsheetDTO>();

            foreach (var blogLog in userBlogLogsheets)
            {
                var user = await _userManager.FindByIdAsync(blogLog.UserId);

                var blogLogsheetDTO = new BlogLogsheetDTO
                {
                    Id = blogLog.Id,
                    Title = blogLog.Title,
                    Description = blogLog.Description,
                    UpdatedAt = blogLog.UpdatedAt,
                    Category = blogLog.Category,
                    Image = blogLog.Image.ToString(),
                    BlogId = blogLog.BlogId,
                    UserId = blogLog.UserId
                };

                blogLogsheetDTOs.Add(blogLogsheetDTO);
            }

            return blogLogsheetDTOs.OrderByDescending(r => r.UpdatedAt).ToList();
        }
        public async Task<List<BlogLogsheetDTO>> GetBlogLogsheetByBlog(int blogId)
        {
            var blogLogsheets = await _blogLogsheetRepository.GetAll(null);

            var blogIdLogsheets = blogLogsheets.Where(log => log.BlogId == blogId).ToList();

            var blogLogsheetDTOs = new List<BlogLogsheetDTO>();

            foreach (var blogLog in blogIdLogsheets)
            {
                var user = await _userManager.FindByIdAsync(blogLog.UserId);

                var blogLogsheetDTO = new BlogLogsheetDTO
                {
                    Id = blogLog.Id,
                    Title = blogLog.Title,
                    Description = blogLog.Description,
                    UpdatedAt = blogLog.UpdatedAt,
                    Category = blogLog.Category,
                    Image = blogLog.Image.ToString(),
                    BlogId = blogLog.BlogId,
                    UserId = blogLog.UserId
                };

                blogLogsheetDTOs.Add(blogLogsheetDTO);
            }

            return blogLogsheetDTOs.OrderByDescending(r => r.UpdatedAt).ToList();
        }


    }
}
