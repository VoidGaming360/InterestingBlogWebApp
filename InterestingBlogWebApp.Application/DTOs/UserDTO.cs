using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.DTOs
{
    public class UserDTO
    {
        
        public string Id { get; set; } // User ID
        public string UserName { get; set; }
        public string Email { get; set; }
    }


    public class UpdateDTO
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Current password is required")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match")]
        public string ConfirmPassword { get; set; } // Optional: password confirmation
    }
    
}
