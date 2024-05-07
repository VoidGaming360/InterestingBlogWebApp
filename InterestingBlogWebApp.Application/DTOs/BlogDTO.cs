
using InterestingBlogWebApp.Domain.Entities;

namespace InterestingBlogWebApp.Application.DTOs
{
    public class BlogDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageURI { get; set; } = string.Empty;
        public List<Comment> Comments { get; set; } = [];
        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
