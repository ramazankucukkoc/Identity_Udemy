using System.Net;
using System.Net.Mail;

namespace Identity_Udemy.Helper
{
    public static class PasswordReset
    {
        public static void PasswordResetSendEmail(string link,string email)
        {
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtpClient = new SmtpClient("mail.gmail.com");
            mailMessage.From = new MailAddress("ramazankucukkoc43@gmail.com");
            mailMessage.To.Add(email);
            mailMessage.Subject = $"www.bıdıbıdı.com::Şifre sıfırlama";
            mailMessage.Body = "<h2>Şifreniz yenilemek için lütfen aşagıdaki linke tıklayınız.</h2><hr/>";
            mailMessage.Body += $"<a href='{link}'>Şifre yenileme linki</a>";
            mailMessage.IsBodyHtml = true;
            smtpClient.Port = 587;
            smtpClient.Credentials = new NetworkCredential("ramazankucukkoc43@gmail.com", "42konya42");
            smtpClient.Send(mailMessage);

        }
        //ramokoc859 -->gmail
        //şifre-->Ramco42*
        //ramokoc4343@gmail.com şifre -->Konya4242
    }
}
