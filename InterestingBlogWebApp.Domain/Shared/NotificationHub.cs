using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace InterestingBlogWebApp.Domain.Shared

{
    [Authorize]
    public class NotificationHub : Hub { }
}
