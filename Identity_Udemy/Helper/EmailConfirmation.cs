using System.Net.Mail;
using System.Net;

namespace Identity_Udemy.Helper
{
    public class EmailConfirmation
    {
        public static void SendEmail(string link, string email)
        {
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.gmail.com");

            mailMessage.From = new MailAddress("ramazankucukkoc43@gmail.com");

            mailMessage.To.Add(email);

            mailMessage.Subject = $"www.bıdıbıdı.com::Email sıfırlama";

            mailMessage.Body = "<h2>Email yenilemek için lütfen aşagıdaki linke tıklayınız.</h2><hr/>";

            mailMessage.Body += $"<a href='{link}'>Email yenileme linki</a>";
            mailMessage.IsBodyHtml = true;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("ramazankucukkoc43@gmail.com", "42konya42");
            smtpClient.Send(mailMessage);

        }
    }
}
