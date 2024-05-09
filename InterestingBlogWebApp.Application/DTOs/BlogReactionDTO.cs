using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.DTOs
{
    public class BlogVoteDTO
    {
        [Key]
        public int Id { get; set; }
        public int BlogId { get; set; }
        public string UserId { get; set; }
        public bool? IsUpVote { get; set; }
        public bool? IsDownVote { get; set; }
        public DateTime CreatedDate { get; set; }

    }

    public class UpvoteBlogDTO
    {
        public int BlogId { get; set; }
        public string? UserId { get; set; }
        public bool IsUpVote { get; set; }
    }
    public class DownvoteBlogDTO
    {
        
        public int BlogId { get; set; }
        public string? UserId { get; set; }
        public bool IsDownVote { get; set; }
    }
}
