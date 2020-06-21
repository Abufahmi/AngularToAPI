using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AngularToAPI.Services
{
    public static class SendGridAPI
    {
        public static async Task<bool> Execute(string userEmail, string userName, string plainTextContent,
            string htmlContent, string subject)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("test@example.com", "Abufahmi");
            var to = new EmailAddress(userEmail, userName);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
            return await Task.FromResult(true);
        }
    }
}
