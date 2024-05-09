using InterestingBlogWebApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.DTOs
{
    
        public class AdminDashboardDataDTO
        {
            public int AllTimeBlogCount { get; set; }
            public int MonthBlogCount { get; set; }
            public int AllTimeCommentCount { get; set; }
            public int MonthCommentCount { get; set; }
            public int AllTimeUpvoteCount { get; set; }
            public int MonthUpvoteCount { get; set; }
            public int AllTimeDownvoteCount { get; set; }
            public int MonthDownvoteCount { get; set; }
            public List<Blog> Top10AllTimeBlogs { get; set; }
            public List<Blog> Top10MonthBlogs { get; set; }
        }

    
}
