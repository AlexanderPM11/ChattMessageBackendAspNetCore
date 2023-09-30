using crudSignalR.Core.Application.Dtos.EmailService;
using crudSignalR.Core.Application.Interface.Services;
using crudSignalR.Core.Domain.Settings;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;


namespace crudSegnalR.Infrastructure.Shared.Services
{
    public class EmailService: IEmailService
    {
        private MailSettings _mailSettings { get; }
        public EmailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public async Task SendAsync(EmailRequest request) 
        {
            try
            {
                MimeMessage email = new();
                email.Sender = MailboxAddress.Parse(_mailSettings.DisplayName + "<" + _mailSettings.EmailFrom + ">");
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;
                BodyBuilder builder = new();
                builder.HtmlBody = request.Body;
                email.Body = builder.ToMessageBody();

                SmtpClient smtpClient = new();

                smtpClient.Connect(_mailSettings.SmtpHost,
                    _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtpClient.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);

                await smtpClient.SendAsync(email);
                smtpClient.Disconnect(true);
            }
            catch (Exception)
            {

                throw;
            }
        }
            
    }
}
