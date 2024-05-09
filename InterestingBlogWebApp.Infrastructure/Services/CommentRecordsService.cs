using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using Microsoft.AspNetCore.Identity;
using InterestingBlogWebApp.Domain.Entities;

namespace InterestingBlogWebApp.Infrastructure.Services
{
    public class CommentLogsheetService : ICommentLogsheetService
    {
        private readonly ICommentLogsheetRepository _commentLogsheetRepository;
        private readonly UserManager<User> _userManager;

        public CommentLogsheetService(ICommentLogsheetRepository commentLogsheetRepository, UserManager<User> userManager)
        {
            _commentLogsheetRepository = commentLogsheetRepository;
            _userManager = userManager;
        }

        public async Task<List<CommentLogsheetDTO>> GetAll()
        {
            var commentLogsheets = await _commentLogsheetRepository.GetAll(null);

            var commentLogsheetDTOs = new List<CommentLogsheetDTO>();

            foreach (var commentLog in commentLogsheets)
            {
                var user = await _userManager.FindByIdAsync(commentLog.UserId);

                var commentLogsheetDTO = new CommentLogsheetDTO
                {
                    Id = commentLog.Id,
                    Description = commentLog.Description,
                    UpdatedAt = commentLog.UpdatedAt,
                    CommentId = commentLog.CommentId,
                    BlogId = commentLog.BlogId,
                    UserId = user.Id
                };

                commentLogsheetDTOs.Add(commentLogsheetDTO);
            }

            return commentLogsheetDTOs.OrderByDescending(c => c.UpdatedAt).ToList();
        }

        public async Task<CommentLogsheetDTO> GetCommentLogsheetById(int commentLogsheetId)
        {
            var commentLog = await _commentLogsheetRepository.GetById(commentLogsheetId);

            if (commentLog == null)
            {
                throw new KeyNotFoundException("Comment logsheet not found.");
            }

            return new CommentLogsheetDTO
            {
                Id = commentLog.Id,
                Description = commentLog.Description,
                UpdatedAt = commentLog.UpdatedAt,
                CommentId = commentLog.CommentId,
                BlogId = commentLog.BlogId,
                UserId = commentLog.UserId
            };
        }

        public async Task<List<CommentLogsheetDTO>> GetCommentsLogsByUserId(string userId)
        {
            var commentLogsheets = await _commentLogsheetRepository.GetAll(null);

            var userCommentLogsheets = commentLogsheets.Where(c => c.UserId == userId).ToList();

            var commentLogsheetDTOs = new List<CommentLogsheetDTO>();

            foreach (var commentLog in userCommentLogsheets)
            {
                var user = await _userManager.FindByIdAsync(commentLog.UserId);

                var commentLogsheetDTO = new CommentLogsheetDTO
                {
                    Id = commentLog.Id,
                    Description = commentLog.Description,
                    UpdatedAt = commentLog.UpdatedAt,
                    CommentId = commentLog.CommentId,
                    BlogId = commentLog.BlogId,
                    UserId = commentLog.UserId
                };

                commentLogsheetDTOs.Add(commentLogsheetDTO);
            }

            return commentLogsheetDTOs.OrderByDescending(c => c.UpdatedAt).ToList();
        }

        public async Task<List<CommentLogsheetDTO>> GetCommentLogsheetByBlog(int blogId)
        {
            var commentLogsheets = await _commentLogsheetRepository.GetAll(null);

            var blogIdLogsheets = commentLogsheets.Where(c => c.BlogId == blogId).ToList();

            var commentLogsheetDTOs = new List<CommentLogsheetDTO>();

            foreach (var commentLog in blogIdLogsheets)
            {
                var user = await _userManager.FindByIdAsync(commentLog.UserId);

                var commentLogsheetDTO = new CommentLogsheetDTO
                {
                    Id = commentLog.Id,
                    Description = commentLog.Description,
                    UpdatedAt = commentLog.UpdatedAt,
                    CommentId = commentLog.CommentId,
                    BlogId = commentLog.BlogId,
                    UserId = commentLog.UserId
                };

                commentLogsheetDTOs.Add(commentLogsheetDTO);
            }

            return commentLogsheetDTOs.OrderByDescending(c => c.UpdatedAt).ToList();
        }

        public async Task<List<CommentLogsheetDTO>> GetCommentLogsheetByComment(int commentId)
        {
            var commentLogsheets = await _commentLogsheetRepository.GetAll(null);

            var commentIdLogsheets = commentLogsheets.Where(c => c.CommentId == commentId).ToList();

            var commentLogsheetDTOs = new List<CommentLogsheetDTO>();

            foreach (var commentLog in commentIdLogsheets)
            {
                var user = await _userManager.FindByIdAsync(commentLog.UserId);

                var commentLogsheetDTO = new CommentLogsheetDTO
                {
                    Id = commentLog.Id,
                    Description = commentLog.Description,
                    UpdatedAt = commentLog.UpdatedAt,
                    CommentId = commentLog.CommentId,
                    BlogId = commentLog.BlogId,
                    UserId = commentLog.UserId
                };

                commentLogsheetDTOs.Add(commentLogsheetDTO);
            }

            return commentLogsheetDTOs.OrderByDescending(c => c.UpdatedAt).ToList();
        }
    }
}
