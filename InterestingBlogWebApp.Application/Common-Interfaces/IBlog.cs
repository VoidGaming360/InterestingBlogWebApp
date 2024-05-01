using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Interfaces
{
    public interface IBlog
    {
        public void CreateBlog();

        public void DeleteBlog();

        public void UpdateBlog();
    }
}
