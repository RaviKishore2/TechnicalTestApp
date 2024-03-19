using System.Net.Mail;
using System.Net;

namespace TechTestApp.Models
{
    public class EmailService : IEmailService
    {

        
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587))
            {
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("ravikishoredarimireddy.1999@gmail.com", "Vizag@2023");

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("ravikishoredarimireddy.1999@gmail.com"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
