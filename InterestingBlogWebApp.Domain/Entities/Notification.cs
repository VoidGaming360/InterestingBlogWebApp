﻿using InterestingBlogWebApp.Domain.Enums;

namespace InterestingBlogWebApp.Domain.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }

    public class Notification<T> : INotification
    {
        public NotificationType NotificationType { get; set; }
        public T Payload { get; set; }
    }
}
