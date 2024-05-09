namespace InterestingBlogWebApp.Domain.Entities
{
    public class CommentReaction
    {
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;

        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; } //navigation property

        public int? CommentId { get; set; }
        public Comment? Comment { get; set; } //navigation property
    }
}
