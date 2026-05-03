using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MyPortfolio.BusinessLayer.Abstract;
using MyPortfolio.BusinessLayer.Dtos.MessageDtos;
using MyPortfolio.BusinessLayer.Models;
using System;
using System.Threading.Tasks;

namespace MyPortfolio.BusinessLayer.Concrete
{
    public class MailManager : IMailService
    {
        private readonly MailSettings _mailSettings;

        // Kurumsal Standart: Tip güvenli Options Pattern.
        public MailManager(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value ?? throw new ArgumentNullException(nameof(mailSettings));
        }

        public async Task SendEmailAsync(MailRequestDto dto)
        {
            var email = new MimeMessage();

            // Senin modelindeki property isimlerine göre uyarlandı
            email.From.Add(new MailboxAddress("Portfolyo İletişim", _mailSettings.SenderEmail));
            email.To.Add(new MailboxAddress("Admin", _mailSettings.ReceiverEmail));
            email.ReplyTo.Add(new MailboxAddress(dto.NameSurname, dto.Email));

            email.Subject = $"Yeni Mesaj: {dto.Subject}";

            var builder = new BodyBuilder
            {
                TextBody = $"Gönderen: {dto.NameSurname}\nMail: {dto.Email}\n\nMesaj:\n{dto.MessageDetail}"
            };
            email.Body = builder.ToMessageBody();

            try
            {
                using var smtp = new SmtpClient();

                // Plesk/Hosting sertifika hatalarını bypass etmek için (Kurumsal Güvenlik Protokolü)
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

                // Modelindeki 'Server' ve 'Port' alanları kullanılıyor
                await smtp.ConnectAsync(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.SenderEmail, _mailSettings.Password);

                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Hatayı sadece konsola yazıp YUTMA. Üst katmana fırlat ki Controller bunu yakalayabilsin.
                Console.WriteLine($"SMTP Hatası: {ex.Message}");
                throw; // Kurumsal standart: Hatayı gizleme, delege et.
            }
        }
    }
}