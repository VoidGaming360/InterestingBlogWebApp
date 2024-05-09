using InterestingBlogWebApp.Application.Helpers;
using InterestingBlogWebApp.Domain.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace InterestingBlogWebApp.Application.Common.Interface.IServices
{
    public interface IEmailService
    {
        Response SendEmail(EmailMessage message, List<string> errors);
        Task SendEmailAsync(EmailMessage message);
    }
}
