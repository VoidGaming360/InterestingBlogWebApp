using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using InterestingBlogWebApp.Domain.Entities;

namespace InterestingBlogWebApp.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Blogs> Blog {  get; set; }
        public DbSet<BlogReaction> BlogReactions { get; set; }
        public DbSet<Comment>   Comments { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }


        
    }
}
