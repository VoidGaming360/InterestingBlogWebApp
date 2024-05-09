using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;

namespace InterestingBlogWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentReactionController : ControllerBase
    {
        private readonly ICommentVoteService _commentVoteService;

        public CommentReactionController(ICommentVoteService commentVoteService)
        {
            _commentVoteService = commentVoteService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCommentVotes()
        {
            var commentVotes = await _commentVoteService.GetAll();
            return Ok(commentVotes);
        }

        [Authorize]
        [HttpGet("get-by-user")]
        public async Task<IActionResult> GetCommentVotesByUserId()
        {
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            var commentVoteDTOs = await _commentVoteService.GetCommentVotesByUserId(userId);

            return Ok(commentVoteDTOs);
        }

        [Authorize]
        [HttpPost("upvote")]
        public async Task<IActionResult> UpvoteComment([FromBody] UpvoteCommentDTO upvoteCommentDTO)
        {
            var errors = new List<string>();
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            upvoteCommentDTO.UserId = userId;

            var response = await _commentVoteService.UpvoteComment(upvoteCommentDTO, errors);

            if (errors.Count > 0)
            {
                return BadRequest(new { errors });
            }

            return Ok(new { message = response });
        }

        [Authorize]
        [HttpPost("downvote")]
        public async Task<IActionResult> DownvoteComment([FromBody] DownvoteCommentDTO downvoteCommentDTO)
        {
            var errors = new List<string>();
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            downvoteCommentDTO.UserId = userId;

            var response = await _commentVoteService.DownvoteComment(downvoteCommentDTO, errors);

            if (errors.Count > 0)
            {
                return BadRequest(new { errors });
            }

            return Ok(new { message = response });
        }

        [Authorize]
        [HttpGet("get-vote/{commentId}")]
        public async Task<IActionResult> GetCommentVote(int commentId)
        {
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            var commentVoteDTO = await _commentVoteService.GetCommentVote(commentId, userId);

            if (commentVoteDTO == null)
            {
                return NotFound(new { message = "No vote found for the specified comment and user." });
            }

            return Ok(commentVoteDTO);
        }

        [Authorize]
        [HttpGet("get-all-votes/{commentId}")]
        public async Task<IActionResult> GetAllVotesForComment(int commentId)
        {
            var commentVoteDTOs = await _commentVoteService.GetAllVotesForComment(commentId);

            return Ok(commentVoteDTOs);
        }
    }
}
