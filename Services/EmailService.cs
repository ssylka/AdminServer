using MailKit.Net.Smtp;
using MimeKit;
using Org.BouncyCastle.Crypto.Macs;

namespace WebServer.Services
{
    public class EmailService
    {
        public async Task SendEmail(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("WebApp", "your_email@gmail.com"));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, false);

            await client.AuthenticateAsync("georgianstas@gmail.com", "aznsxxlybetehcjx"); // azns xxly bete hcjx - Пароль приложения для вашего устройства

            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
