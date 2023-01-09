using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Options;
using Pelican.Domain.Entities;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Pelican.Application.Mail;
public class MailService : IMailService
{
	private readonly MailOptions _mailOptions;
	private readonly IUnitOfWork _unitOfWork;
	public MailService(IOptions<MailOptions> mailOptions, IUnitOfWork unitOfWork)
	{
		_mailOptions = mailOptions.Value ?? throw new ArgumentNullException(nameof(mailOptions));
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Response> SendEmailAsync(MailRequestDto mailRequest, CancellationToken cancellationToken = default)
	{
		var client = new SendGridClient(_mailOptions.ApiKey);
		var from = new EmailAddress(_mailOptions.Mail, _mailOptions.DisplayName);
		var subject = mailRequest.Subject;
		var to = new EmailAddress(mailRequest.ToEmail, mailRequest.ToName);
		var plainTextContent = mailRequest.Body;
		var htmlContent = "";
		var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
		var response = await client.SendEmailAsync(msg, cancellationToken);
		return response;
	}

	public async Task<Response> TestSendEmail()
	{
		var mail = new MailRequestDto
		{
			ToEmail = _mailOptions.Mail,
			Subject = "Test mail from Pelican",
			Body = "TestBody",
		};

		return await SendEmailAsync(mail);
	}

	public Task<Response> SendUserActivationEmail(string email, string token)
	{
		var emailEntity = _unitOfWork.EmailRepository.FirstOrDefaultAsync(x => x.);
	}

	public async Task<Response> SendForgotPasswordEmail(string email, string token)
	{

	}

	public Task<string> GeneratePreview(Email emailDto)
	{
		throw new NotImplementedException();
	}

	public Task<Response> SendInviteExistingUserEmail(string email)
	{
		throw new NotImplementedException();
	}

	public Task<Response> SendInviteNewUserEmail(string email, string token)
	{
		throw new NotImplementedException();
	}

	public Task<Response> SendReminderEmail(string email)
	{
		throw new NotImplementedException();
	}

}
