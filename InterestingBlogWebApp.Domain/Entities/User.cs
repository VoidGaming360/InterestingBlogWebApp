using InterestingBlogWebApp.Domain.Shared;
using Microsoft.AspNetCore.Identity;

namespace InterestingBlogWebApp.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

    }
}
