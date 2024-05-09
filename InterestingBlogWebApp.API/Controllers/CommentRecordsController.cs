using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;

namespace InterestingBlogWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentRecordsController : ControllerBase
    {
        private readonly ICommentLogsheetService _commentLogsheetService;

        public CommentRecordsController(ICommentLogsheetService commentLogsheetService)
        {
            _commentLogsheetService = commentLogsheetService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllCommentLogsheets()
        {
            try
            {
                var commentLogsheets = await _commentLogsheetService.GetAll();
                return Ok(commentLogsheets);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("get-by-user")]
        public async Task<IActionResult> GetCommentLogsheetsByUserId()
        {
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            var commentLogsheets = await _commentLogsheetService.GetCommentsLogsByUserId(userId);
            return Ok(commentLogsheets);
        }

        [Authorize]
        [HttpGet("get-by-blog/{blogId}")]
        public async Task<IActionResult> GetCommentLogsheetsByBlogId(int blogId)
        {
            var commentLogsheets = await _commentLogsheetService.GetCommentLogsheetByBlog(blogId);
            return Ok(commentLogsheets);
        }

        [Authorize]
        [HttpGet("get-by-comment/{commentId}")]
        public async Task<IActionResult> GetCommentLogsheetsByCommentId(int commentId)
        {
            var commentLogsheets = await _commentLogsheetService.GetCommentLogsheetByComment(commentId);
            return Ok(commentLogsheets);
        }

        [Authorize]
        [HttpGet("get-logsheet/{commentLogsheetId}")]
        public async Task<IActionResult> GetCommentLogsheetById(int commentLogsheetId)
        {
            try
            {
                var commentLogsheet = await _commentLogsheetService.GetCommentLogsheetById(commentLogsheetId);
                return Ok(commentLogsheet);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Comment logsheet not found." });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
    }
}
