namespace InterestingBlogWebApp.Application.Common_Interfaces
{
    public interface IUserAccessor
    {
        string GetCurrentUserId();
        string GetCurrentUserName();
    }
}
