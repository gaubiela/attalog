using AttaLog.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AttaLog.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SendInviteEmailAsync(string toEmail, string toName, string groupName, string inviteUrl)
    {
        var apiKey = _configuration["SendGrid:ApiKey"];

        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogInformation("SendGrid API key not configured. Invite URL: {InviteUrl}", inviteUrl);
            return;
        }

        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_configuration["SendGrid:FromEmail"], _configuration["SendGrid:FromName"]);
        var to = new EmailAddress(toEmail, toName);
        var subject = $"You've been invited to join {groupName} on AttaLog";
        var htmlContent = $"""
            <h2>Group Invite</h2>
            <p>You've been invited to join <strong>{groupName}</strong> on AttaLog.</p>
            <p><a href="{inviteUrl}">Click here to accept the invite</a></p>
            <p>This invite will expire soon.</p>
            """;

        var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
        var response = await client.SendEmailAsync(msg);

        if (!response.IsSuccessStatusCode)
            _logger.LogWarning("Failed to send invite email to {Email}. Status: {Status}", toEmail, response.StatusCode);
    }
}
