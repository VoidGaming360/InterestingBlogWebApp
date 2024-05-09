using InterestingBlogWebApp.Application.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface IAdminDashboardService
    {
    Task<AdminDashboardDataDTO> GetDashboardData(int month, int year);

    }
}
