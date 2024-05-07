using Microsoft.AspNetCore.Identity;

namespace InterestingBlogWebApp.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public new string UserName { get; set; } = string.Empty;
        public string PictureUri { get; set; } = string.Empty;

        //relation mapping
        public List<Blogs> Blogs { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];
        public List<Reaction> Reactions { get; set; } = [];

    }
}
