namespace AttaLog.Application.Interfaces;

public interface IEmailService
{
    Task SendInviteEmailAsync(string toEmail, string toName, string groupName, string inviteUrl);
}
