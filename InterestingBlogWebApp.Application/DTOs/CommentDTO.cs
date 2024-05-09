using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; } // Comment ID
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; } // When the comment was created
        public bool IsEdited { get; set; }
        public bool IsMine { get; set; }
        public int BlogId { get; set; } // Blog to which this comment belongs
        public string? UserId { get; set; } // User who created the comment

        public string UserName { get; set; }


    }
    public class AddCommentDTO
    {
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Blog ID is required")]
        public int BlogId { get; set; }

        public string? UserId { get; set; }
    }

    public class UpdateCommentDTO
    {
        [Required(ErrorMessage = "Comment ID is required")]
        public int Id { get; set; } // Comment ID to update

        public string? Description { get; set; } // The new description for the comment

        public bool? IsEdited { get; set; } // Indicates if the comment is edited

        public string? UserId { get; set; }
    }
}
