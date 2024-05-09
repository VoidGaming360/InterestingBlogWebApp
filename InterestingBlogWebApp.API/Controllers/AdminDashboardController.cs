using InterestingBlogWebApp.Application.Common.Interface.IServices;
using Microsoft.AspNetCore.Mvc;

namespace BisleriumProject.Controllers
{
    [ApiController]
    [Route("api/admin-dashboard")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminDashboardController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }

        [HttpGet("cumulative-count")]
        public async Task<IActionResult> GetCumulativeCount()
        {
            var data = await _adminDashboardService.GetCumulativeCount();
            return Ok(data);
        }

        [HttpGet("monthly-count")]
        public async Task<IActionResult> GetMonthlyCount([FromQuery] int month, [FromQuery] int year)
        {
            var data = await _adminDashboardService.GetMonthlyCount(month, year);
            return Ok(data);
        }

        [HttpGet("top-posts")]
        public async Task<IActionResult> GetTopPosts([FromQuery] int month = 0, [FromQuery] int year = 0)
        {
            var data = await _adminDashboardService.GetTopPosts(month, year);
            return Ok(data);
        }

        [HttpGet("top-bloggers")]
        public async Task<IActionResult> GetTopBloggers([FromQuery] int month = 0, [FromQuery] int year = 0)
        {
            var data = await _adminDashboardService.GetTopBloggers(month, year);
            return Ok(data);
        }
    }
}