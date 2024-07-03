using CourseWork.AuthorizationServer.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace CourseWork.AuthorizationServer.Services.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
