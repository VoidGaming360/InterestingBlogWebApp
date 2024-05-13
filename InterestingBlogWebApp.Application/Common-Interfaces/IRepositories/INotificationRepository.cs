using InterestingBlogWebApp.Domain.Entities;

namespace InterestingBlogWebApp.Application.Common.Interface.IRepositories
{
    public interface INotificationRepository : IRepositoryBase<Notification>
    {
        Task<List<Notification>> GetNotificationsByReceiverIdAsync(string receiverId);
        Task<List<Notification>> GetUnreadNotificationsByReceiverIdAsync(string receiverId);
        Task MarkNotificationAsReadAsync(int notificationId);
    }
}