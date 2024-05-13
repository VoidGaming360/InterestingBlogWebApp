using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Application.Helpers;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using System.Reflection.Metadata;

namespace InterestingBlogWebApp.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly IBlogService _blogService;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private const long MaxFileSizeInBytes = 3 * 1024 * 1024; // 3 Megabytes in bytes


        public BlogController(IBlogService blogService, IHubContext<NotificationHub> notificationHub)
        {
            _blogService = blogService;
            _notificationHub = notificationHub;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var blogs = await _blogService.GetAll();
                var userId = User.FindFirst("userId")?.Value;


                // Fetch user name for each blog
                foreach (var blog in blogs)
                {
                    var userName = await _blogService.GetUserNameById(blog.UserId);
                    blog.UserName = userName;
                    blog.IsMine = blog.UserId == userId;
                }

                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message); // Handle internal server errors
            }
        }

        [HttpGet("sorted")]
        public async Task<IActionResult> GetAllSorted(string sortBy = "random", int pageNumber = 1, int pageSize = 2)
        {
            var (blogs, totalPages, totalCount) = await _blogService.GetAllSorted(sortBy, pageNumber, pageSize);
            var userId = User.FindFirst("userId")?.Value;

            foreach (var blog in blogs)
            {
                var userName = await _blogService.GetUserNameById(blog.UserId);
                blog.UserName = userName;
                blog.IsMine = blog.UserId == userId;
            }

            var response = new
            {
                Blogs = blogs,
                TotalPages = totalPages,
                TotalCount = totalCount,
                CurrentPage = pageNumber
            };
            return Ok(response);
        }


        [AllowAnonymous]
        [HttpGet("get-by-id/{blogId}")]
        public async Task<IActionResult> GetBlogById(int blogId) // Get blog by ID
        {
            try
            {
                var blogDTO = await _blogService.GetBlogById(blogId); // Call the service method
                var userId = User.FindFirst("userId")?.Value;

                // Fetch user name for the blog
                var userName = await _blogService.GetUserNameById(blogDTO.UserId);
                blogDTO.IsMine = blogDTO.UserId == userId;
                blogDTO.UserName = userName;

                return Ok(blogDTO); // Return the blog DTO
            }
            catch (KeyNotFoundException ex) // Handle case where the blog doesn't exist
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex) // Handle unexpected errors
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpGet("get-by-user")]
        public async Task<IActionResult> GetBlogsByUserId()
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;

                var blogDTOs = await _blogService.GetBlogsByUserId(userId);

                // Fetch user name for each blog
                foreach (var blog in blogDTOs)
                {
                    var userName = await _blogService.GetUserNameById(blog.UserId);
                    blog.UserName = userName;
                    blog.IsMine = blog.UserId == userId;
                }

                return Ok(blogDTOs);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message }); // Handle cases where the user or blogs aren't found
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddBlog([FromForm] AddBlogDTO blogDTO)
        {
            var errors = new List<string>();
            try
            {
                var userId = User.FindFirst("userId")?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token." });
                }

                blogDTO.UserId = userId;
                if (blogDTO.Image != null && blogDTO.Image.Length > MaxFileSizeInBytes)
                {
                    return BadRequest(new { message = "Image too large to upload." });
                }

                var response = await _blogService.AddBlog(blogDTO, errors);
                if (errors.Count > 0)
                {
                    return BadRequest(new { errors });
                }

                // Fetch username for a more personalized notification
                var userName = await _blogService.GetUserNameById(userId);
                var notificationMessage = $"{blogDTO.Title} created by {userName} successfully.";

                try
                {
                    await _notificationHub.Clients.All.SendAsync("ReceiveNotification", notificationMessage);
                    return Ok(new { message = response, notification = notificationMessage });
                }
                catch (Exception notifEx)
                {
                    // Return a response including the notification attempt error
                    return Ok(new { message = response, notificationError = notifEx.Message, notificationAttempted = notificationMessage });
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { message = ex.Message });
            }
        }


        [Authorize]
        [HttpPut("update-blog")]
        public async Task<IActionResult> UpdateBlog([FromForm] UpdateBlogDTO updateBlogDTO)
        {
            try
            {
                var userId = User.FindFirst("userId")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token." });
                }

                var blog = await _blogService.GetBlogById(updateBlogDTO.BlogId);

                if (blog.UserId != userId) // Only allow the blog author to update
                {
                    return StatusCode(403, new { message = "Only the blog author can update this post." });
                }
                // Check the image file size during update
                if (updateBlogDTO.Image != null && updateBlogDTO.Image.Length > MaxFileSizeInBytes)
                {
                    return BadRequest(new { message = "Image size exceeds the 3 MB limit." });
                }
                var errors = new List<string>();
                var response = await _blogService.UpdateBlog(updateBlogDTO, errors);

                if (errors.Count > 0)
                {
                    return BadRequest(new { errors });
                }

                return Ok(new { message = response });
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
        [HttpDelete("delete/{blogId}")]
        public async Task<IActionResult> DeleteBlog(int blogId)
        {
            var errors = new List<string>();
            try
            {
                var userId = User.FindFirst("userId")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User ID not found in token." });
                }

                var blog = await _blogService.GetBlogById(blogId);

                if (blog.UserId != userId) // Only allow the blog author to delete
                {
                    return StatusCode(403, new { message = "Only the blog author can delete this post." });
                }

                var response = await _blogService.DeleteBlog(blogId, errors);

                if (errors.Count > 0)
                {
                    return BadRequest(new { errors });
                }

                return Ok(new { message = response });
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
    }
}