using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.DTOs
{
    public class NotificationDTO
    {
        public int Id { get; set; } // Unique identifier for the notification
        public string SenderId { get; set; } // The ID of the user who sent the notification
        public string ReceiverId { get; set; } // The ID of the user who is the intended recipient of the notification
        public string Message { get; set; } // The message content of the notification
        public DateTime Timestamp { get; set; } // The time at which the notification was sent
        public bool IsRead { get; set; } // Flag to determine if the notification has been read
    }
}
