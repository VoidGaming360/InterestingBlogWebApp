using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.DTOs
{
    public class CommentVoteDTO
    {
        [Key]
        public int Id { get; set; }
        public int CommentId { get; set; }
        public string UserId { get; set; }
        public bool? IsUpVote { get; set; }
        public bool? IsDownVote { get; set; }
        public DateTime CreatedDate { get; set; }

    }

    public class UpvoteCommentDTO
    {
        public int CommentId { get; set; }
        public string? UserId { get; set; }
        public bool IsUpVote { get; set; }
    }
    public class DownvoteCommentDTO
    {

        public int CommentId { get; set; }
        public string? UserId { get; set; }
        public bool IsDownVote { get; set; }
    }
}
