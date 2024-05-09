using InterestingBlogWebApp.Application.Common.Interface.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace InterestingBlogWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogRecordsController : ControllerBase
    {
        private readonly IBlogLogsheetService _blogLogsheetService;

        public BlogRecordsController(IBlogLogsheetService blogLogsheetService)
        {
            _blogLogsheetService = blogLogsheetService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllBlogLogsheets()
        {
            var blogLogsheets = await _blogLogsheetService.GetAll();
            return Ok(blogLogsheets);
        }

        [Authorize]
        [HttpGet("get-by-user")]
        public async Task<IActionResult> GetBlogLogsheetsByUserId()
        {
            var userId = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { message = "User ID not found in token." });
            }

            var blogLogsheets = await _blogLogsheetService.GetBlogsLogsByUserId(userId);
            return Ok(blogLogsheets);
        }

        [Authorize]
        [HttpGet("get-by-blog/{blogId}")]
        public async Task<IActionResult> GetBlogLogsheetsByBlogId(int blogId)
        {
            var blogLogsheets = await _blogLogsheetService.GetBlogLogsheetByBlog(blogId);
            return Ok(blogLogsheets);
        }

        [Authorize]
        [HttpGet("get-by-id/{blogLogsheetId}")]
        public async Task<IActionResult> GetBlogLogsheetById(int blogLogsheetId)
        {
            try
            {
                var blogLogsheet = await _blogLogsheetService.GetBlogLogsheetById(blogLogsheetId);

                if (blogLogsheet == null)
                {
                    return NotFound(new { message = "Blog logsheet not found." });
                }

                return Ok(blogLogsheet);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }
    }
}
