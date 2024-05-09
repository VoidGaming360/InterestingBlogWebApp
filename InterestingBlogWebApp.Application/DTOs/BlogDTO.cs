using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.DTOs
{
    public class BlogDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsEdited { get; set; }
        public bool IsMine { get; set; }
        public List<string> Category { get; set; }
        public string Image { get; set; }
        public string UserId { get; set; }
        public int Score { get; set; }
        public int UpVoteCount { get; set; }
        public int DownVoteCount { get; set; }
        public string UserName { get; set; }

    }
    public class AddBlogDTO
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public List<string> Category { get; set; }
        public IFormFile? Image { get; set; }
        public string? UserId { get; set; }
    }

    public class UpdateBlogDTO 
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public List<string>? Category { get; set; }
        public IFormFile? Image { get; set; }
        public string? UserId { get; set; }
        public int BlogId { get; set; }
    }
}
