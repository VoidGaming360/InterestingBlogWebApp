namespace InterestingBlogWebApp.Domain.Entities
{
    public class Reaction
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;

        public int UserId { get; set; }
        public ApplicationUser? User { get; set; } //navigation property

        public int? BlogId { get; set; }
        public Blogs? Blogs { get; set; } //navigation property

        public int? CommentId { get; set; }
        public Comment? Comment { get; set; } //navigation property
    }
}
