namespace InterestingBlogWebApp.Domain.Entities
{
    public class Comment
    {
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? UpdatedOn { get; set; }

        public int? BlogsId { get; set; }
        public Blogs? Blogs { get; set; }

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
