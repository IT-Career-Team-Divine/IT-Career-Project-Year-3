using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using The_Gram.Data.Models;

namespace The_Gram.Services
{
    public class EmailSenderService : IEmailSender
    {
        UserManager<User> userManager;
        public EmailSenderService(UserManager<User> _usermanager)
        {
            this.userManager = _usermanager;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("The Gram ","the.it.career.gram@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain") { Text = message };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls).ConfigureAwait(true);
               await client.AuthenticateAsync("the.it.career.gram@gmail.com", "roiivfmuaivujnpw").ConfigureAwait(true);
                await client.SendAsync(emailMessage).ConfigureAwait(true);
                await client.DisconnectAsync(true).ConfigureAwait(true);
            }
        }
      
    }
}
