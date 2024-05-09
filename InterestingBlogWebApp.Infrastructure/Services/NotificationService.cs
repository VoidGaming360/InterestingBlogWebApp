using InterestingBlogWebApp.Application.Common_Interfaces.IServices;
using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace InterestingBlogWebApp.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly AppDbContext _context;

        public NotificationService(AppDbContext context)
        {
            _context = context;
        }
        public async Task SaveNotificationAsync(NotificationDTO notificationDto)
        {
            var notification = new Notification
            {
                SenderId = notificationDto.SenderId,
                ReceiverId = notificationDto.ReceiverId,
                Message = notificationDto.Message,
                Timestamp = DateTime.UtcNow,
                IsRead = false
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }
        public async Task<NotificationDTO> SaveNotificationDTOAsync(NotificationDTO notificationDto)
        {
            var notification = new Notification
            {
                SenderId = notificationDto.SenderId,
                ReceiverId = notificationDto.ReceiverId,
                Message = notificationDto.Message,
                Timestamp = DateTime.UtcNow, // Set server-side to ensure consistency
                IsRead = false // Default to false when initially saving
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();

            notificationDto.Id = notification.Id; // Ensure the DTO has the ID set from the database
            notificationDto.Timestamp = notification.Timestamp; // Set the exact timestamp from the database
            return notificationDto; // Return the updated DTO
        }

        public async Task<IEnumerable<NotificationDTO>> GetNotificationsForUserAsync(string userId)
        {
            var notifications = await _context.Notifications
                                              .Where(n => n.ReceiverId == userId)
                                              .Select(n => new NotificationDTO
                                              {
                                                  Id = n.Id,
                                                  SenderId = n.SenderId,
                                                  ReceiverId = n.ReceiverId,
                                                  Message = n.Message,
                                                  Timestamp = n.Timestamp,
                                                  IsRead = n.IsRead
                                              })
                                              .ToListAsync();
            return notifications;
        }

        public async Task<int> CountUnreadNotifications(string userId)
        {
            return await _context.Notifications
                                 .Where(n => n.ReceiverId == userId && !n.IsRead)
                                 .CountAsync();
        }

        public async Task MarkNotificationsAsRead(string userId)
        {
            var notifications = _context.Notifications
                                        .Where(n => n.ReceiverId == userId && !n.IsRead).ToList();

            notifications.ForEach(n => n.IsRead = true);
            await _context.SaveChangesAsync();
        }
    }
}
