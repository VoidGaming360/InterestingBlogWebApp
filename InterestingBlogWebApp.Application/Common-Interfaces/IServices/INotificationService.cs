using InterestingBlogWebApp.Application.DTOs;
using InterestingBlogWebApp.Domain.Entities;

namespace InterestingBlogWebApp.Application.Common_Interfaces.IServices
{
    public interface INotificationService
    {
        Task SaveNotificationAsync(NotificationDTO notification);
        Task<NotificationDTO> SaveNotificationDTOAsync(NotificationDTO notificationDto);
        Task<IEnumerable<NotificationDTO>> GetNotificationsForUserAsync(string userId);
    }

}
