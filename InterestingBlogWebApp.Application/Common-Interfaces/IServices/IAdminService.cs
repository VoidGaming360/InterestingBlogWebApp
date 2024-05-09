using BisleriumProject.Application.DTOs;
using InterestingBlogWebApp.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface IAdminDashboardService
    {
        Task<AdminDashboardDataDTO> GetCumulativeCount();
        Task<AdminDashboardDataDTO> GetMonthlyCount(int month, int year);
        Task<List<BlogDTO>> GetTopPosts(int month, int year);
        Task<List<UserDTO>> GetTopBloggers(int month, int year);
    }
}