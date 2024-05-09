using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace InterestingBlogWebApp.Application.DTOs
{
    public class BlogDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Image { get; set; }
        public string ApplicationUserId { get; set; }
        public int Score { get; set; }
        public int UpVoteCount { get; set; }
        public int DownVoteCount { get; set; }
    }
}
