using Microsoft.AspNetCore.Identity;

namespace InterestingBlogWebApp.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PictureUri { get; set; } = string.Empty;

        //relation mapping
        public List<Blogs> Blogs { get; set; } = [];
        public List<BlogReaction> BlogReactions { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];
        public List<CommentReaction> CommentReactions { get; set; } = [];

    }
}
