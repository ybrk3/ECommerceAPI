using ECommerceAPI.Application.Abstractions.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Infrastructure.Services
{
    public sealed class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        //being called where mail to send
        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var to in tos) mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new("info@ngAkademi.com", "NGT", System.Text.Encoding.UTF8); //Uyumsuz karakterlere karşı encoding
            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential(_configuration["Mail:Username"], _configuration["Mail:Password"]);
            smtp.Port = int.Parse(_configuration["Mail:Port"]);
            smtp.EnableSsl = true;
            smtp.Host = _configuration["Mail:Host"];
            await smtp.SendMailAsync(mail);
        }

        //to send mail to single user
        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendPasswordResetMailAsync(string to, string userId, string resetToken)
        {
            //For the mail body inclusive of link which directs user to reset password
            StringBuilder mail = new();
            //target=_blank  => open thge link in the new tab
            mail.AppendLine("Merhaba<br>Eğer yeni şifre talebinde dulunduysanız aşağıdaki linkten şifrenizi yeniliyebilirsiniz.<br><a target=\"_blank\" href=\"");
            mail.AppendLine(_configuration["AngularClientUrl"]);
            mail.AppendLine("/update-password/"); //Route in UI
            mail.AppendLine(userId);
            mail.AppendLine("/");
            mail.AppendLine(resetToken);
            mail.AppendLine("\">Yeni şifre talebi için tıklayınız...</a></strong><br><br><span style=\"font-size:12px;\">NOT : Eğer ki bu talep tarafınızca gerçekleştirilmemişse lütfen bu maili ciddiye almayınız.</span><br>Saygılarımızla...<br><br><br>BY - E-CommerceApp");

            await SendMailAsync(to, "Şifre Yenileme Talebi", mail.ToString());
        }
        public async Task SendCompletedOrderMailAsync(string to, string orderCode, DateTime orderDate, string userNameSurname)
        {
            string mail = $"Sayın {userNameSurname} Merhaba<br>" +
                $"{orderDate} tarihinde vermiş olduğunuz {orderCode} kodlu siparişiniz tamamlanmış ve kargo firmasına verilmiştir.<br>Hayrını görünüz efendim...";

            await SendMailAsync(to, $"{orderCode} Sipariş Numaralı Siparişiniz Tamamlandı", mail);

        }

    }
}
