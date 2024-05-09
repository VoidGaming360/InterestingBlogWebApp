using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InterestingBlogWebApp.Domain.Entities
{
    public class CommentReaction
    {
        [Key]
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int BlogId { get; set; }
        [ForeignKey(nameof(BlogId))]
        public virtual Blog Blog { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        public int CommentId { get; set; }
        [ForeignKey(nameof(CommentId))]
        public virtual Comment Comment { get; set; }
    }

}
