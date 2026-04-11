using SendGrid;
using SendGrid.Helpers.Mail;

namespace WebServer.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmail(string to, string subject, string body)
        {
            var apiKey = _config["SendGrid:ApiKey"];
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(
                "2000ctac@gmail.com"
            );

            var toEmail = new EmailAddress(to);

            var msg = MailHelper.CreateSingleEmail(
                from,
                toEmail,
                subject,
                plainTextContent: body,
                htmlContent: body
            );

            await client.SendEmailAsync(msg);
        }
    }
}

// Example of sending an email using SendGrid in C# in EU region

//using SendGrid;
//using SendGrid.Helpers.Mail;
//using System;
//using System.Threading.Tasks;

//namespace Example
//{
//    internal class Example
//    {
//        private static void Main()
//        {
//            Execute().Wait();
//        }

//        static async Task Execute()
//        {
//            var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
//            /* var options = new SendGridClientOptions
//            {
//                ApiKey = apiKey
//            };
//            options.SetDataResidency("eu"); 
//            var client = new SendGridClient(options); */
//            // uncomment the above 6 lines if you are sending mail using a regional EU subuser
//            // and remove the client declaration just below
//            var client = new SendGridClient(apiKey);
//            var from = new EmailAddress("geogrianstas@gmail.com", "Example User");
//            var subject = "Sending with SendGrid is Fun";
//            var to = new EmailAddress("test@example.com", "Example User");
//            var plainTextContent = "and easy to do anywhere, even with C#";
//            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
//            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
//            var response = await client.SendEmailAsync(msg);
//        }
//    }
//}

// My secend attempt at sending email using Mailtrap, but recivers are not getting the emails, it is just a sandbox for testing, though it works

/*
﻿using MailKit.Net.Smtp;
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
 */


// My first attempt at sending email using SmtpClient,
// but it seems that the service is not working,
// so I switched to SendGrid,
// which is a popular email service provider with a free tier for testing purposes.

//using System.Net;
//using System.Net.Mail;

//namespace WebServer.Services
//{
//    public class EmailService
//    {
//        private readonly IConfiguration _config;

//        public EmailService(IConfiguration config)
//        {
//            _config = config;
//        }

//        public async Task SendEmail(string to, string subject, string body)
//        {
//            using var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
//            {
//                Credentials = new NetworkCredential("958338f0774b4b", "d7b2d58309a17f"),
//                EnableSsl = false
//            };
//            await client.SendMailAsync("georgianstas@gmail.com", to, subject, body);
//            System.Console.WriteLine("Sent");
//        }
//    }
//}