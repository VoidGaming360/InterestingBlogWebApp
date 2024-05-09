using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.DTOs;
using System;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Controllers
{
    //[Authorize(Roles = "Admin")] // Only admins should have access
    [Route("api/admin-dashboard")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _adminDashboardService;

        public AdminDashboardController(IAdminDashboardService adminDashboardService)
        {
            _adminDashboardService = adminDashboardService;
        }

        [HttpGet("get-dashboard-data")]
        public async Task<IActionResult> GetDashboardData([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                // Fetch the dashboard data for the specified month and year
                var dashboardData = await _adminDashboardService.GetDashboardData(month, year);

                return Ok(dashboardData); // Return the data with a 200 status code
            }
            catch (Exception ex)
            {
                // Handle errors
                return StatusCode(500, new { message = $"An error occurred while fetching dashboard data: {ex.Message}" });
            }
        }
    }
}
