namespace InterestingBlogWebApp.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? BlogsId { get; set; }
        public Blogs? Blogs { get; set; }

        public int UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
