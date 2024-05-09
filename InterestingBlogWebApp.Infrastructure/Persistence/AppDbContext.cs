using InterestingBlogWebApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InterestingBlogWebApp.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {


        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<BlogVote> BlogVotes { get; set; }
        public DbSet<CommentVote> CommentVotes { get; set; }
        public DbSet<BlogLogsheet> BlogLogsheets { get; set; }
        public DbSet<CommentLogsheet> CommentLogsheets { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }

}
