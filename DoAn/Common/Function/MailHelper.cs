using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace DoAn.Common.Function
{
    public class MailHelper
    {
        public IConfiguration Configuration;
        public void SendMail(string toEmailAddress, string subject, string content)
        {
            //var fromEmailAdrress =  Configuration.GetSection("MySettings").GetSection("FromEmailAddress").Value;
            //var fromEmailDisplayName = Configuration.GetSection("MySettings").GetSection("FromEmailDisplayName").Value;
            //var fromEmailPassword = Configuration.GetSection("MySettings").GetSection("FromEmailPassword").Value;
            //var smtpHost = Configuration.GetSection("MySettings").GetSection("SMTPHost").Value;
            //var smtpPost = Configuration.GetSection("MySettings").GetSection("SMTPPost").Value;

            //bool enabledSsl = bool.Parse(Configuration.GetSection("MySettings").GetSection("EnabledSSL").Value);

            //string body = content;

            //MailMessage message = new MailMessage(new MailAddress(fromEmailAdrress, fromEmailDisplayName), new MailAddress(toEmailAddress));


            //message.Subject = subject;
            //message.IsBodyHtml = true;
            //message.Body = body;

            //var client = new SmtpClient
            //{
            //    UseDefaultCredentials = false,
            //    DeliveryMethod = SmtpDeliveryMethod.Network,
            //    Credentials = new NetworkCredential(fromEmailAdrress, fromEmailPassword),
            //    Host = smtpHost,
            //    EnableSsl = enabledSsl,
            //    Port = !string.IsNullOrEmpty(smtpPost) ? Convert.ToInt32(smtpPost) : 0,
            //    Timeout = 20000
            //};
            //client.Send(message);
            var fromAddress = new MailAddress("khoitedu99@gmail.com", "Tin nhắn hệ thống Gongcha");
            var toAddress = new MailAddress(toEmailAddress, "Mr Test");
            const string fromPassword = "Fizz1999";
            string body = content;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}