﻿using Core.Application.Dtos.Email;
using Core.Application.Interfaces.Services;
using Core.Domain.Settings;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Infrastructure.Shared.Services
{
    public class EmailService : IEmailService
    {
        public MailSetting _mailSetting { get; }
        public EmailService(IOptions<MailSetting> options)
        {
            _mailSetting = options.Value;
        }

        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                MimeMessage message = new();
                message.Sender = MailboxAddress.Parse($"{_mailSetting.DisplayName} <{_mailSetting.EmailFrom}>");
                message.To.Add(MailboxAddress.Parse(request.To));
                message.Subject = request.Subject;
                message.From.Add(MailboxAddress.Parse(request.From));

                BodyBuilder builder = new BodyBuilder();
                builder.HtmlBody = request.Body;
                message.Body = builder.ToMessageBody();

                SmtpClient smtp = new();
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                smtp.Connect(_mailSetting.SmptHost, _mailSetting.SmptPort, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSetting.SmptUser, _mailSetting.SmptPass);
                await smtp.SendAsync(message);
                smtp.Disconnect(true);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error While Sending Email." + ex);
            }
        }
    }
}
