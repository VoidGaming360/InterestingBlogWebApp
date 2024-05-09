using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.Common_Interfaces.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;
    private readonly IHubContext<NotificationHub> _notificationHub;
    private readonly IBlogService _blogService;
    private readonly INotificationService _notificationService;

    public CommentController(ICommentService commentService,
                         IHubContext<NotificationHub> notificationHub,
                         IBlogService blogService,
                         INotificationService notificationService)
    {
        _commentService = commentService;
        _notificationHub = notificationHub;
        _blogService = blogService;
        _notificationService = notificationService;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var userId = User.FindFirst("userId")?.Value;
            var comments = await _commentService.GetAll();

            foreach (var comment in comments)
            {
                var userName = await _blogService.GetUserNameById(comment.UserId);
                comment.UserName = userName;
                comment.IsMine = comment.UserId == userId;
            }
            return Ok(comments);
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpGet("by-user")]
    public async Task<IActionResult> GetCommentsByUserId()
    {
        try
        {
            var userId = User.FindFirst("userId")?.Value; // Retrieve user ID from JWT token

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }
         
            var commentDTOs = await _commentService.GetCommentsByUserId(userId);

            foreach (var comment in commentDTOs)
            {
                var userName = await _blogService.GetUserNameById(comment.UserId);
                comment.UserName = userName;
                comment.IsMine = comment.UserId == userId;
            }
            return Ok(commentDTOs);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpGet("by-blog/{blogId}")]
    public async Task<IActionResult> GetCommentsByBlogId(int blogId)
    {
        try
        {
            var userId = User.FindFirst("userId")?.Value; 

            var commentDTOs = await _commentService.GetCommentsByBlogId(blogId);
            foreach (var comment in commentDTOs)
            {
                var userName = await _blogService.GetUserNameById(comment.UserId);
                comment.UserName = userName;
                comment.IsMine = comment.UserId == userId;
            }
            return Ok(commentDTOs);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> AddComment([FromBody] AddCommentDTO createCommentDTO)
    {
        var errors = new List<string>();
        try
        {
            var userId = User.FindFirst("userId")?.Value; // Retrieve user ID from JWT token
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            createCommentDTO.UserId = userId; // Set the user ID in the DTO
            var response = await _commentService.AddComment(createCommentDTO, errors);
            if (errors.Count > 0)
            {
                return BadRequest(new { errors });
            }

            // Fetch the blog to get the blog author's ID
            var blog = await _blogService.GetBlogById(createCommentDTO.BlogId);
            if (blog == null)
            {
                return NotFound(new { message = "Blog not found." });
            }

            var authorId = blog.UserId;
            var commentAuthorName = await _blogService.GetUserNameById(userId); // Assuming this method exists

            // Check if the commenter is not the author
            if (userId != authorId )
            {
                var notificationMessage = $"{commentAuthorName} added a comment on your blog.";
                var notificationDto = new NotificationDTO
                {
                    SenderId = userId,
                    ReceiverId = authorId,
                    Message = notificationMessage,
                    IsRead = false,
                    Timestamp = DateTime.UtcNow
                };

                // Send real-time notification
                await _notificationHub.Clients.User(authorId).SendAsync("ReceiveNotification", notificationMessage);

                // Save the notification in the database
                await _notificationService.SaveNotificationAsync(notificationDto);
            }

            return Ok(new { message = response });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
        }
    }




    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentDTO updateCommentDTO)
    {
        var errors = new List<string>();
        try
        {
            var userId = User.FindFirst("userId")?.Value; // Retrieve user ID from JWT token

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            var comment = await _commentService.GetCommentById(updateCommentDTO.Id);

            if (comment.UserId != userId)
            {
                return StatusCode(403, new Response(null, new List<string> { "Only the comment author can update this comment." }, HttpStatusCode.Forbidden));
            }

            updateCommentDTO.UserId = userId; // Set the user ID in the DTO

            var response = await _commentService.UpdateComment(updateCommentDTO, errors);

            if (errors.Count > 0)
            {
                return BadRequest(new { errors });
            }

            return Ok(new { message = response });
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpDelete("delete/{commentId}")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var errors = new List<string>();
        try
        {
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new Response(null, new List<string> { "User ID not found in token." }, HttpStatusCode.Unauthorized));
            }

            var comment = await _commentService.GetCommentById(commentId);

            if (comment.UserId != userId)
            {
                return StatusCode(403, new Response(null, new List<string> { "Only the comment author can delete this comment." }, HttpStatusCode.Forbidden));
            }

            var response = await _commentService.DeleteComment(commentId, errors);

            if (errors.Count > 0)
            {
                return BadRequest(new Response(null, errors, HttpStatusCode.BadRequest));
            }

            return Ok(new Response(response, new List<string>(), HttpStatusCode.OK));
        }
        catch (Exception ex)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
        }
    }
}
