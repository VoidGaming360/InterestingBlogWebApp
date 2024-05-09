namespace InterestingBlogWebApp.Application.DTOs
{
    public class CommentLogsheetDTO
    {
        public int Id { get; set; } 
        public string Description { get; set; }
        public DateTime UpdatedAt { get; set; } 
        public int CommentId { get; set; } 
        public int BlogId { get; set; }
        public string? UserId { get; set; } 

    }
}
