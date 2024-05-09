using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructures.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Infrastructure.Services
{
    public class CommentReactionService : ICommentReactionService
    {
        private readonly ICommentReactionRepository _commentVoteRepository;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public CommentReactionService(ICommentReactionRepository commentVoteRepository, ICommentService commentService, UserManager<User> userManager)
        {
            _commentVoteRepository = commentVoteRepository;
            _commentService = commentService;
            _userManager = userManager;
        }

        public async Task<List<CommentReactionDTO>> GetAll()
        {
            var commentVotes = await _commentVoteRepository.GetAll(null);

            return commentVotes.Select(vote => new CommentReactionDTO
            {
                Id = vote.Id,
                CommentId = vote.CommentId,
                UserId = vote.UserId,
                IsUpVote = vote.IsUpVote,
                IsDownVote = vote.IsDownVote
            }).ToList();
        }

        public async Task<List<CommentReactionDTO>> GetCommentVotesByUserId(string userId)
        {
            var votes = await _commentVoteRepository.GetAll(null);

            var userCommentVotes = votes.Where(vote => vote.UserId == userId).ToList();

            var commentVoteDTOs = new List<CommentReactionDTO>();

            foreach (var commentVote in userCommentVotes)
            {
                var commentVoteDTO = new CommentReactionDTO
                {
                    Id = commentVote.Id,
                    CommentId = commentVote.CommentId,
                    UserId = commentVote.UserId,
                    IsUpVote = commentVote.IsUpVote,
                    IsDownVote = commentVote.IsDownVote
                };

                commentVoteDTOs.Add(commentVoteDTO);
            }

            return commentVoteDTOs.OrderByDescending(v => v.CreatedDate).ToList();
        }

        public async Task<CommentReactionDTO> GetCommentVote(int commentId, string userId)
        {
            var commentVote = await _commentVoteRepository.GetVote(commentId, userId);

            if (commentVote == null)
            {
                return null; // No vote found for the specified comment and user
            }

            return new CommentReactionDTO
            {
                Id = commentVote.Id,
                CommentId = commentVote.CommentId,
                UserId = commentVote.UserId,
                IsUpVote = commentVote.IsUpVote,
                IsDownVote = commentVote.IsDownVote
            };
        }

        public async Task<CommentReactionDTO> GetCommentVoteById(int voteId)
        {
            var commentVote = await _commentVoteRepository.GetById(voteId);

            return new CommentReactionDTO
            {
                Id = commentVote.Id,
                CommentId = commentVote.CommentId,
                UserId = commentVote.UserId,
                IsUpVote = commentVote.IsUpVote,
                IsDownVote = commentVote.IsDownVote
            };
        }

        public async Task<IEnumerable<CommentReactionDTO>> GetAllVotesForComment(int commentId)
        {
            var votes = await _commentVoteRepository.GetAll(null);

            var commentVotes = votes.Where(vote => vote.CommentId == commentId).ToList();

            var commentVoteDTOs = new List<CommentReactionDTO>();

            foreach (var vote in commentVotes)
            {
                var commentVoteDTO = new CommentReactionDTO
                {
                    Id = vote.Id,
                    CreatedDate = vote.CreatedDate,
                    CommentId = vote.CommentId,
                    UserId = vote.UserId,
                    IsUpVote = vote.IsUpVote,
                    IsDownVote = vote.IsDownVote
                };

                commentVoteDTOs.Add(commentVoteDTO);
            }
            return commentVoteDTOs.OrderByDescending(c => c.CreatedDate).ToList();
        }

        public async Task<string> UpvoteComment(UpvoteCommentDTO commentVoteDTO, List<string> errors)
        {
            try
            {
                var existingVote = await _commentVoteRepository.GetVote(commentVoteDTO.CommentId, commentVoteDTO.UserId);

                if (existingVote != null)
                {
                    if (existingVote.IsUpVote == true)
                    {
                        existingVote.IsUpVote = false;
                        existingVote.IsDownVote = false;
                        await _commentVoteRepository.SaveChangesAsync(); // Save changes

                        return "Upvote removed.";
                    }

                    existingVote.IsUpVote = true;
                    existingVote.IsDownVote = false;
                }
                else
                {
                    var newVote = new CommentRecord
                    {
                        CommentId = commentVoteDTO.CommentId,
                        UserId = commentVoteDTO.UserId,
                        CreatedDate = DateTime.UtcNow,
                        IsUpVote = true,
                        IsDownVote = false
                    };

                    await _commentVoteRepository.Add(newVote);
                }

                await _commentVoteRepository.SaveChangesAsync(); // Save changes

                return "Upvoted successfully.";
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                throw;
            }
        }

        public async Task<string> DownvoteComment(DownvoteCommentDTO commentVoteDTO, List<string> errors)
        {
            try
            {
                var existingVote = await _commentVoteRepository.GetVote(commentVoteDTO.CommentId, commentVoteDTO.UserId);

                if (existingVote != null)
                {
                    if (existingVote.IsDownVote == true)
                    {
                        existingVote.IsDownVote = false;
                        existingVote.IsUpVote = false;
                        await _commentVoteRepository.SaveChangesAsync(); // Save changes

                        return "Downvote removed.";
                    }

                    existingVote.IsDownVote = true;
                    existingVote.IsUpVote = false;
                }
                else
                {
                    var newVote = new CommentRecord
                    {
                        CommentId = commentVoteDTO.CommentId,
                        UserId = commentVoteDTO.UserId,
                        IsDownVote = true,
                        IsUpVote = false,
                        CreatedDate = DateTime.UtcNow
                    };

                    await _commentVoteRepository.Add(newVote);
                }

                await _commentVoteRepository.SaveChangesAsync(); // Save changes

                return "Downvoted successfully.";
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                throw;
            }
        }
    }
}
