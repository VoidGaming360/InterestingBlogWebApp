namespace InterestingBlogWebApp.Domain.Entities
{
    public class Blogs
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageURI { get; set; } = string.Empty;

        public List<Comment> Comments { get; set; } = [];
        public List<CommentReaction> CommentReactions{ get; set; } = [];
        public List<BlogReaction> BlogReactions { get; set; } = [];
        public string? ApplicationUserId { get; set; }
        public ApplicationUser? ApplicationUser { get; set; }
    }
}
