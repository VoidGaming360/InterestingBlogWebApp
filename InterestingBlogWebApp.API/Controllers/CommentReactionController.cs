using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Application.Common_Interfaces.IServices;
using Microsoft.AspNetCore.SignalR;

namespace InterestingBlogWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentReactionController : ControllerBase
    {
        private readonly ICommentReactionService _commentVoteService;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly ICommentService _commentService; // To get comment details, assuming this exists
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;


        public CommentReactionController(ICommentReactionService commentVoteService,
                        IHubContext<NotificationHub> notificationHub,
                        ICommentService commentService, // Ensure this service can fetch comment details
                        INotificationService notificationService,
                        IUserService userService)
        {
            _commentVoteService = commentVoteService;
            _notificationHub = notificationHub;
            _commentService = commentService;
            _notificationService = notificationService;
            _userService = userService;
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

            var comment = await _commentService.GetCommentById(upvoteCommentDTO.CommentId);
            if (comment == null)
            {
                return NotFound(new { message = "Comment not found." });
            }

            var authorId = comment.UserId;
            var upvoterName = await _userService.GetUserNameById(userId); // Correct service used here

            if (userId != authorId)
            {
                var notificationMessage = $"{upvoterName} upvoted your comment.";
                await _notificationHub.Clients.User(authorId).SendAsync("ReceiveNotification", notificationMessage);

                var notificationDto = new NotificationDTO
                {
                    SenderId = userId,
                    ReceiverId = authorId,
                    Message = notificationMessage,
                    IsRead = false,
                    Timestamp = DateTime.UtcNow
                };

                await _notificationService.SaveNotificationAsync(notificationDto);
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
