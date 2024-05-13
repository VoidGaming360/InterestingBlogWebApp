using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Domain.Payloads
{
    public class CommentRelatedPayload
    {
        public string Username { get; set; }
        public string BlogTitle { get; set; }
        public string CommentContent { get; set; }
    }
}
