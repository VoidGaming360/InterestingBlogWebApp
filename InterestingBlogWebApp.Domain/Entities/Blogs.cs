using System.ComponentModel.DataAnnotations.Schema;

namespace InterestingBlogWebApp.Domain.Entities
    {
        public class Blogs
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
            public Guid Image { get; set; }
            public DateTime CreatedAt { get; set; }
            public List<Comment> Comments { get; set; } = [];
            public List<CommentReaction> CommentReactions{ get; set; } = [];
            public List<BlogReaction> BlogReactions { get; set; } = [];
            public string? ApplicationUserId { get; set; }
            [ForeignKey("ApplicationUserId")]
            public ApplicationUser? ApplicationUser { get; set; }
            public int? Score { get; set; }
            public int? UpVoteCount { get; set; }
            public int? DownVoteCount { get; set; }
    }
    }
