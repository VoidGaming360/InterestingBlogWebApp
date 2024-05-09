using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Domain.Entities
{
    public class BlogLogsheet
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> Category { get; set; }
        public Guid Image { get; set; }
        public int BlogId { get; set; }
        [ForeignKey(nameof(BlogId))]
        public virtual Blog blog { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual User user { get; set; }

    }
}
