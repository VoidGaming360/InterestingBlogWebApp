using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common_Interfaces.IServices
{
    public interface IEmailServices
    {
        Task SendEmailAsync(string to, string toDisplayName, string subject, string htmlBody);
    }
}
