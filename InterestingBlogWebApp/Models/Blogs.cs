﻿namespace InterestingBlogWebApp.Models
{
    public class Blogs
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public List<Comment> Comments { get; set; } = [];

    }
}
