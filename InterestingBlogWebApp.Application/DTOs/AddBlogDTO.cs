using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.DTOs
{
    public class AddBlogDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Category { get; set; }
        public IFormFile? Image { get; set; }
        public string? ApplicationUserId { get; set; }
    }
}
