using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Domain.Entities
{
    public class BlogReaction
    {
        [Key]
        public int Id { get; set; }
        public int BlogId { get; set; }
        [ForeignKey(nameof(BlogId))]
        public virtual Blog blog { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User user { get; set; }
        public bool? IsUpVote { get; set; }
        public bool? IsDownVote { get; set; }
        public DateTime CreatedDate { get; set; }


    }
}
