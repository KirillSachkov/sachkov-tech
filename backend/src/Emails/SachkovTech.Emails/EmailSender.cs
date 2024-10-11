using MailKit.Net.Smtp;
using MimeKit;

namespace SachkovTech.Emails;

public class EmailSender : IEmailSender
{
    public async Task SendEmailAsync(EmailData emailData)
    {
        var emailMessage = new MimeMessage();

        emailMessage.From.Add(new MailboxAddress("Администрация сайта", "kirillvirtul@yandex.ru"));
        emailMessage.To.Add(new MailboxAddress("", emailData.Email));
        emailMessage.Subject = emailData.Subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = emailData.Message
        };

        using var client = new SmtpClient();

        await client.ConnectAsync("smtp.yandex.ru", 465, true);
        await client.AuthenticateAsync("", "");
        await client.SendAsync(emailMessage);

        await client.DisconnectAsync(true);
    }
}