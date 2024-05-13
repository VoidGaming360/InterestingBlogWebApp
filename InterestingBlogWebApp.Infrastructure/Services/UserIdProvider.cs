using Microsoft.AspNetCore.SignalR;

namespace InterestingBlogWebApp.Infrastructure.Services
{
    public class UserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.FindFirst("userId")?.Value;
        }
    }
}
