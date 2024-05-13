using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Domain.Entities;
using InterestingBlogWebApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Infrastructures.Repositories
{
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        private readonly AppDbContext _dbContext;

        public NotificationRepository(AppDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Notification>> GetNotificationsByReceiverIdAsync(string receiverId)
        {
            return await _dbContext.Notifications
                .Where(n => n.ReceiverId == receiverId)
                .OrderByDescending(n => n.Timestamp)
                .ToListAsync();
        }

        public async Task<List<Notification>> GetUnreadNotificationsByReceiverIdAsync(string receiverId)
        {
            return await _dbContext.Notifications
                .Where(n => n.ReceiverId == receiverId && !n.IsRead)
                .OrderByDescending(n => n.Timestamp)
                .ToListAsync();
        }

        public async Task MarkNotificationAsReadAsync(int notificationId)
        {
            var notification = await _dbContext.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}