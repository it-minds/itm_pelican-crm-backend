using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.Authentication;
public interface IMailService
{
	Task<Result> TestSendEmail();
	Task<Result> SendUserActivationEmail(string email, string token);
	Task<Result> SendForgotPasswordEmail(string email, string token);
	Task<string> GeneratePreview(Email emailDto);
}
