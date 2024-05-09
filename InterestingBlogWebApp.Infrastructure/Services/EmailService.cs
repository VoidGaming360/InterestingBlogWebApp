using InterestingBlogWebApp.Application.Common.Interface.IRepositories;
using InterestingBlogWebApp.Application.Common.Interface.IServices;
using InterestingBlogWebApp.Application.Helpers;
using InterestingBlogWebApp.Domain.Auth;
using InterestingBlogWebApp.Domain.Entities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Net;

namespace InterestingBlogWebApp.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public EmailService(IOptions<EmailConfiguration> emailConfig, UserManager<User> userManager)
        {
            _emailConfig = emailConfig.Value;
            _userManager = userManager;
        }
        public Response SendEmail(EmailMessage message, List<string> errors)
        {
            var emails = message.To.Select(x => x.Address).ToList();

            if (emails.Count == 0)
            {
                errors.Add("Please enter your email.");
                return new Response(null, errors, HttpStatusCode.BadRequest);
            }


            foreach (var email in emails)
            {
                var user = _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    errors.Add($"User with email {email} does not exist.");
                    return new Response(null, errors, HttpStatusCode.BadRequest);
                }

                var emailMessage = CreateEmailMessage(message);
                Send(emailMessage);
            }

            return new Response($"Emails sent to {string.Join(", ", emails)}", null, HttpStatusCode.OK);
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder() { HtmlBody = message.Content };

            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        attachment.CopyTo(ms);
                        fileBytes = ms.ToArray();
                    }
                    bodyBuilder.Attachments.Add(attachment.FileName, fileBytes, ContentType.Parse(attachment.ContentType));
                }
            }
            emailMessage.Body = bodyBuilder.ToMessageBody();
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password);
                    await client.SendAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}

