using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Application.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var comments = await _commentService.GetAll();
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
            var commentDTOs = await _commentService.GetCommentsByBlogId(blogId);
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
