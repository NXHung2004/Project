using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using Project.Models;

namespace Project.Service
{
    public class EmailService
    {
        private readonly EmailSetting _emailSettings;
        public EmailService(IOptions<EmailSetting> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }
        public async Task SendEMailAsync(string toEmail,
            string subject, string htmlContent)
        {
            var fromAddress = new MailAddress(_emailSettings.UserName);
            var toAddress = new MailAddress(toEmail);
            var smtp = new SmtpClient
            {
                Host = _emailSettings.Host,
                Port = _emailSettings.Port,
                EnableSsl = _emailSettings.EnableSsl,
                //Network chỉ ra rằng là email sẽ được gửi thông qua mạng
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                //thông tin xác thực để kết nối đếm máy chủ smtp
                Credentials = new NetworkCredential(_emailSettings.UserName,
                _emailSettings.Password),
            };
            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true
            };
            await smtp.SendMailAsync(message);
        }
    }
}
