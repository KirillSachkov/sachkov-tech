namespace SachkovTech.Emails;

public interface IEmailSender
{
    Task SendEmailAsync(EmailData emailData);
}