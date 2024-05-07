using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Application.Interfaces;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructure.Persistence;
using InterestingBlogWebApp.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InterestingBlogWebApp.API.Controllers
{
    public class BlogsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlog _blog;
        private readonly ApplicationDbContext _context;

        public BlogsController(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork, IBlog blog, ApplicationDbContext context)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _blog = blog;
            _context = context;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var userId = _userManager.GetUserId(User);

            var blogs = _blog.GetAllForUser(userId, pageNumber, pageSize);

            return View(blogs);
        }


        //[HttpGet]
        //[Route("/all-blogs")]
        //public IActionResult GetAll()
        //{
        //    return View(_blog);
        //}

        [HttpGet]
        [Route("/all-blogs")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id < 1)
                return BadRequest();
            var product = await _context.Blog.FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
                return NotFound();
            return Ok(product);

        }


        [HttpPost]
        //[Authorize]
        [Route("/add-blog")]
        public async Task<IActionResult> Create(BlogDTO blogDTO)
        {
            //string userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //if (string.IsNullOrEmpty(userId))
            //{
            //    return NotFound("User Not Found");
            //}

            //blogDTO.ApplicationUserId = userId;
            

            // Create the blog using the assigned user ID
            _blog.CreateBlog(blogDTO);
            await _unitOfWork.SaveChangesAsync();
            
            // Return a response indicating success
            return CreatedAtAction(nameof(GetById), _blog);
        }


    }
}
