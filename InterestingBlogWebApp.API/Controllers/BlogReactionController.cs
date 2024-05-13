using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using Microsoft.AspNetCore.SignalR;
using InterestingBlogWebApp.Application.Common_Interfaces.IServices;
using InterestingBlogWebApp.Domain.Shared;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Domain.Payloads;
using InterestingBlogWebApp.Domain.Enums;

namespace InterestingBlogWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogReactionController : ControllerBase
    {
        private readonly IBlogReactionService _blogVoteService;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IBlogService _blogService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public BlogReactionController(IBlogReactionService blogVoteService, IHubContext<NotificationHub> notificationHub,
            IBlogService blogService, INotificationService notificationService, IUserService userService)
        {
            _blogVoteService = blogVoteService;
            _notificationHub = notificationHub;
            _blogService = blogService;
            _notificationService = notificationService;
            _userService = userService;

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

            var blog = await _blogService.GetBlogById(blogVoteDTO.BlogId);
            if (blog == null)
            {
                return NotFound(new { message = "Blog not found." });
            }

            var authorId = blog.UserId;
            var upvoterName = await _userService.GetUserNameById(userId);

            if (userId != authorId)
            {
                var payload = new BlogRelatedPayload
                {
                    Username = upvoterName,
                    BlogTitle = blog.Title
                };

                await _notificationHub.Clients.User(authorId).SendAsync(
                    "ReceiveNotification",
                    new Notification<BlogRelatedPayload>
                    {
                        NotificationType = NotificationType.BLOG_LIKE,
                        Payload = payload
                    }
                );

                var notificationDto = new NotificationDTO
                {
                    SenderId = userId,
                    ReceiverId = authorId,
                    Message = $"{upvoterName} upvoted your blog '{blog.Title}'.",
                    IsRead = false,
                    Timestamp = DateTime.UtcNow
                };
                await _notificationService.SaveNotificationAsync(notificationDto);
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
