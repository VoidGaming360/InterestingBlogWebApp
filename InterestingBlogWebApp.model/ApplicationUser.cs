namespace InterestingBlogWebApp.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public string Nationality { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        public DateTime DOB { get; set; }

        public string PictureUri { get; set; } = string.Empty;

        public List<Blogs> Blogs { get; set; } = [];
}

namespace InterestingBlogWebApp.Models
{
    public enum Gender
    {
        Male, Female, Other
    }
}