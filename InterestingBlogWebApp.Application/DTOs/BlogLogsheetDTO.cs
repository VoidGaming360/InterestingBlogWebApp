using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.DTOs
{
    public class BlogLogsheetDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int BlogId { get; set; }
        public DateTime UpdatedAt { get; set; } 
        public List<string> Category { get; set; } 
        public string? UserId { get; set; } 


    }
    public class AddBlogLogsheetDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int BlogId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Category { get; set; }
        public string? UserId { get; set; }
    }
}
