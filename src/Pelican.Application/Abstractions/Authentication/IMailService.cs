using Pelican.Domain.Entities;
using SendGrid;

namespace Pelican.Application.Abstractions.Authentication;
public interface IMailService
{
	Task<Response> TestSendEmail();
	Task<Response> SendUserActivationEmail(string email, string token);
	Task<Response> SendForgotPasswordEmail(string email, string token);
	Task<string> GeneratePreview(Email emailDto);
	Task<Response> SendInviteNewUserEmail(string email, string token);
	Task<Response> SendInviteExistingUserEmail(string email);
	Task<Response> SendReminderEmail(string email);
}
