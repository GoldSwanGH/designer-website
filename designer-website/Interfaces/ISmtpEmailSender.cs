using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace designer_website.Interfaces
{
    public enum EmailResult
    {
        SendSuccess,
        SendFail
    }
    public interface ISmtpEmailSender
    {
        public EmailResult TryToSendMail(MailboxAddress from, MailboxAddress to, string subject, MimeEntity body);
    }
}