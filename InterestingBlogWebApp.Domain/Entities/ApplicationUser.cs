using Microsoft.AspNetCore.Identity;

namespace InterestingBlogWebApp.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public DateTime DOB { get; set; }

        public string PictureUri { get; set; } = string.Empty;


        //relation mapping
        public List<Blogs> Blogs { get; set; } = [];
        public List<Comment> Comments { get; set; } = [];
        public List<Reaction> Reactions { get; set; } = [];

    }
}

namespace InterestingBlogWebApp.Domain.Entities
{
    public enum Gender
    {
        Male, Female, Other
    }
}