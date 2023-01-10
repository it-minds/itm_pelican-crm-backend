using Pelican.Application.Mail;
using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.Mail;
public interface IMailService
{
	Task<Result> TestSendEmailAsync();
	Task<Result> SendUserActivationEmailAsync(string email, string token);
	Task<Result> SendForgotPasswordEmailAsync(string email, string token);
	Task<string> GeneratePreviewAsync(EmailDto emailDto);
}
