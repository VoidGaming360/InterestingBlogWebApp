using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Application.Helpers;

namespace InterestingBlogWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogReactionController : ControllerBase
    {
        private readonly IBlogVoteService _blogVoteService;

        public BlogReactionController(IBlogVoteService blogVoteService)
        {
            _blogVoteService = blogVoteService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBlogVotes()
        {
            var blogVotes = await _blogVoteService.GetAll();
            return Ok(blogVotes);
        }

        [Authorize]
        [HttpGet("get-by-user")]
        public async Task<IActionResult> GetBlogVotesByUserId()
        {
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            var blogVoteDTOs = await _blogVoteService.GetBlogVotesByUserId(userId);

            return Ok(blogVoteDTOs);
        }

        [Authorize]
        [HttpPost("upvote")]
        public async Task<IActionResult> UpvoteBlog([FromBody] UpvoteBlogDTO blogVoteDTO)
        {
            var errors = new List<string>();
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            blogVoteDTO.UserId = userId;

            var response = await _blogVoteService.UpvoteBlog(blogVoteDTO, errors);

            if (errors.Count > 0)
            {
                return BadRequest(new { errors });
            }

            return Ok(new { message = response });
        }

        [Authorize]
        [HttpPost("downvote")]
        public async Task<IActionResult> DownvoteBlog([FromBody] DownvoteBlogDTO blogVoteDTO)
        {
            var errors = new List<string>();
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            blogVoteDTO.UserId = userId;

            var response = await _blogVoteService.DownvoteBlog(blogVoteDTO, errors);

            if (errors.Count > 0)
            {
                return BadRequest(new { errors });
            }

            return Ok(new { message = response });
        }

        [Authorize]
        [HttpGet("get-vote/{blogId}")]
        public async Task<IActionResult> GetBlogVote(int blogId)
        {
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            var blogVoteDTO = await _blogVoteService.GetVote(blogId, userId);

            if (blogVoteDTO == null)
            {
                return NotFound(new { message = "No vote found for the specified blog and user." });
            }

            return Ok(blogVoteDTO);
        }

        [Authorize]
        [HttpGet("get-all-votes/{blogId}")]
        public async Task<IActionResult> GetAllVotesForBlog(int blogId)
        {
            var blogVoteDTOs = await _blogVoteService.GetAllVotesForBlog(blogId);

            return Ok(blogVoteDTOs);
        }
    }
}
