using MailKit.Net.Smtp;
using MimeKit;
using System.IO;

public static class EmailService
{
    public static async Task SendTransactionEmailAsync(string toEmail, string subject, string body, string attachmentPath)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse("yourbank@example.com"));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        var builder = new BodyBuilder { TextBody = body };

        if (!string.IsNullOrEmpty(attachmentPath))
            builder.Attachments.Add(attachmentPath);

        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, false);
        await smtp.AuthenticateAsync("yourbank@example.com", "your-email-password");
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
