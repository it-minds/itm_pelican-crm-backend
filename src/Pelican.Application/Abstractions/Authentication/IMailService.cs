namespace Pelican.Application.Abstractions.Authentication;
public interface IMailService
{
	Task TestSendEmail();
	Task SendUserActivationEmail(string email, string token);
	Task SendForgotPasswordEmail(string email, string token);
	//TODO: Re add below line when email has been added.
	//Task<string> GeneratePreview(Email emailDto);
	Task SendInviteNewUserEmail(string email, string token);
	Task SendInviteExistingUserEmail(string email);
	Task SendReminderEmail(string email);
}
