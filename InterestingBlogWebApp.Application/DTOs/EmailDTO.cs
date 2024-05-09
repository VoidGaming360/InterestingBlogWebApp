namespace InterestingBlogWebApp.Application.DTOs
{
    public class EmailDTO
    {
        public string Email { get; set; }
    }

    public class PasswordResetDTO
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
    
}
