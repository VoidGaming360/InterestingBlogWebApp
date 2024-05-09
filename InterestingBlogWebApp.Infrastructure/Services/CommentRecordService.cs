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
    public class CommentRecordService : ICommentRecordService
    {
        private readonly ICommentRecordRepository _commentLogsheetRepository;
        private readonly UserManager<User> _userManager;

        public CommentRecordService(ICommentRecordRepository commentLogsheetRepository, UserManager<User> userManager)
        {
            _commentLogsheetRepository = commentLogsheetRepository;
            _userManager = userManager;
        }

        public async Task<List<CommentRecordDTO>> GetAll()
        {
            var commentLogsheets = await _commentLogsheetRepository.GetAll(null);

            var commentLogsheetDTOs = new List<CommentRecordDTO>();

            foreach (var commentLog in commentLogsheets)
            {
                var user = await _userManager.FindByIdAsync(commentLog.UserId);

                var commentLogsheetDTO = new CommentRecordDTO
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

        public async Task<CommentRecordDTO> GetCommentLogsheetById(int commentLogsheetId)
        {
            var commentLog = await _commentLogsheetRepository.GetById(commentLogsheetId);

            if (commentLog == null)
            {
                throw new KeyNotFoundException("Comment logsheet not found.");
            }

            return new CommentRecordDTO
            {
                Id = commentLog.Id,
                Description = commentLog.Description,
                UpdatedAt = commentLog.UpdatedAt,
                CommentId = commentLog.CommentId,
                BlogId = commentLog.BlogId,
                UserId = commentLog.UserId
            };
        }

        public async Task<List<CommentRecordDTO>> GetCommentsLogsByUserId(string userId)
        {
            var commentLogsheets = await _commentLogsheetRepository.GetAll(null);

            var userCommentLogsheets = commentLogsheets.Where(c => c.UserId == userId).ToList();

            var commentLogsheetDTOs = new List<CommentRecordDTO>();

            foreach (var commentLog in userCommentLogsheets)
            {
                var user = await _userManager.FindByIdAsync(commentLog.UserId);

                var commentLogsheetDTO = new CommentRecordDTO
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

        public async Task<List<CommentRecordDTO>> GetCommentLogsheetByBlog(int blogId)
        {
            var commentLogsheets = await _commentLogsheetRepository.GetAll(null);

            var blogIdLogsheets = commentLogsheets.Where(c => c.BlogId == blogId).ToList();

            var commentLogsheetDTOs = new List<CommentRecordDTO>();

            foreach (var commentLog in blogIdLogsheets)
            {
                var user = await _userManager.FindByIdAsync(commentLog.UserId);

                var commentLogsheetDTO = new CommentRecordDTO
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

        public async Task<List<CommentRecordDTO>> GetCommentLogsheetByComment(int commentId)
        {
            var commentLogsheets = await _commentLogsheetRepository.GetAll(null);

            var commentIdLogsheets = commentLogsheets.Where(c => c.CommentId == commentId).ToList();

            var commentLogsheetDTOs = new List<CommentRecordDTO>();

            foreach (var commentLog in commentIdLogsheets)
            {
                var user = await _userManager.FindByIdAsync(commentLog.UserId);

                var commentLogsheetDTO = new CommentRecordDTO
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
