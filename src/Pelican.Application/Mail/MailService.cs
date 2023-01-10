using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Options;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using RazorEmails.Interfaces;
using RazorEmails.Views.Emails.CtaButtonEmail;

namespace Pelican.Application.Mail;
public sealed class MailService : IMailService
{
	private readonly MailOptions _mailOptions;
	private readonly IRazorViewToStringRenderer _razorViewToStringRenderer;
	private readonly IUnitOfWork _unitOfWork;
	public MailService(
		IOptions<MailOptions> mailOptions,
		IRazorViewToStringRenderer razorViewToStringRenderer,
		IUnitOfWork unitOfWork)
	{
		_mailOptions = mailOptions.Value ?? throw new ArgumentNullException(nameof(mailOptions));
		_razorViewToStringRenderer = razorViewToStringRenderer ?? throw new ArgumentNullException(nameof(razorViewToStringRenderer));
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result> SendEmailAsync(MailRequestDto mailRequest, CancellationToken cancellationToken = default)
	{
		MailAddress to = new(mailRequest.ToEmail, mailRequest.ToName);
		MailAddress from = new(_mailOptions.Mail, _mailOptions.DisplayName);

		MailMessage message = new MailMessage(from, to);
		message.Subject = mailRequest.Subject;
		message.Body = mailRequest.Body;
		message.IsBodyHtml = true;
		AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mailRequest.Body, null, "text/html");

		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
		SmtpClient client = new(_mailOptions.Host, _mailOptions.Port)
		{
			Credentials = new NetworkCredential(_mailOptions.UserName, _mailOptions.Password),
			EnableSsl = false
		};
		client.Send(message);
		return Result.Success();
	}

	public async Task<Result> SendTestEmail()
	{
		var emailModel = new CtaButtonEmailViewModel
		{
			Heading1 = "Lorem",
			paragraph1 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In tempus adipiscing felis, sit amet blandit ipsum volutpat sed. Morbi porttitor, eget accumsan dictum, nisi libero ultricies ipsum, in posuere mauris neque at erat.",
			Heading2 = "Lorem",
			paragraph2 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In tempus adipiscing felis, sit amet blandit ipsum volutpat sed. Morbi porttitor, eget accumsan dictum, nisi libero ultricies ipsum, in posuere mauris neque at erat.",
			Heading3 = "Lorem",
			paragraph3 = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. In tempus adipiscing felis, sit amet blandit ipsum volutpat sed. Morbi porttitor, eget accumsan dictum, nisi libero ultricies ipsum, in posuere mauris neque at erat.",
			CtaButtonText = "Lorem ipsum",
			CtaButtonUrl = "https://pelican-linux-frontend-app.azurewebsites.net/"
		};

		MailRequestDto mail = new()
		{
			ToEmail = "ame@it-minds.dk",
			Subject = "Test mail From Pelican Support",
			Body = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/CtaButtonEmail/CtaButtonEmail.cshtml", emailModel),
		};

		return await SendEmailAsync(mail);
	}

	public async Task<Result> SendUserActivationEmail(string email, string token)
	{
		Email? emailEntity = _unitOfWork.EmailRepository
			.FirstOrDefaultAsync(x => x.EmailType == Domain.Enums.EmailTypeEnum.SendUserActivation).Result;
		if (emailEntity is null)
		{
			return Result.Failure(new Error("EmailTemplate.NotFound", "No Template For Send User Activation Email has been set"));
		}

		CtaButtonEmailViewModel emailModel = new()
		{
			Heading1 = emailEntity.Heading1,
			paragraph1 = emailEntity.Paragraph1,
			Heading2 = emailEntity.Heading2,
			paragraph2 = emailEntity.Paragraph2,
			Heading3 = emailEntity.Heading3,
			paragraph3 = emailEntity.Paragraph3,
			CtaButtonText = emailEntity.CtaButtonText,
			CtaButtonUrl = _mailOptions.baseUrl + "/changepassword?token=" + token
		};

		MailRequestDto mail = new()
		{
			ToEmail = email,
			Subject = emailEntity.Subject,
			Body = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/CtaButtonEmail/CtaButtonEmail.cshtml", emailModel)
		};
		return await SendEmailAsync(mail);
	}

	public async Task<Result> SendForgotPasswordEmail(string email, string token)
	{
		Email? emailEntity = _unitOfWork.EmailRepository
			.FirstOrDefaultAsync(x => x.EmailType == Domain.Enums.EmailTypeEnum.SendForgotPassword).Result;
		if (emailEntity is null)
		{
			return Result.Failure(new Error("EmailTemplate.NotFound", "No Template For Forgot Password Email has been set"));
		}

		CtaButtonEmailViewModel emailModel = new()
		{
			Heading1 = emailEntity.Heading1,
			paragraph1 = emailEntity.Paragraph1,
			Heading2 = emailEntity.Heading2,
			paragraph2 = emailEntity.Paragraph2,
			Heading3 = emailEntity.Heading3,
			paragraph3 = emailEntity.Paragraph3,
			CtaButtonText = emailEntity.CtaButtonText,
			CtaButtonUrl = _mailOptions.baseUrl + "/changepassword?token=" + token
		};

		MailRequestDto mail = new()
		{
			ToEmail = email,
			Subject = emailEntity.Subject,
			Body = await _razorViewToStringRenderer.RenderViewToStringAsync("/Views/Emails/CtaButtonEmail/CtaButtonEmail.cshtml", emailModel)
		};
		return await SendEmailAsync(mail);
	}

	public async Task<string> GeneratePreview(Email emailDto)
	{
		var emailModel = new CtaButtonEmailViewModel()
		{
			Heading1 = emailDto.Heading1,
			paragraph1 = emailDto.Paragraph1,
			Heading2 = emailDto.Heading2,
			paragraph2 = emailDto.Paragraph2,
			Heading3 = emailDto.Heading3,
			paragraph3 = emailDto.Paragraph3,
			CtaButtonText = emailDto.CtaButtonText,
		};

		var htmlString = await _razorViewToStringRenderer.RenderViewToStringAsync(
		  "/Views/Emails/CtaButtonEmail/CtaButtonEmail.cshtml", emailModel);

		var hostedLogo = _mailOptions.baseUrl + "/images/logo.png";
		var hostedAltLogo = _mailOptions.baseUrl + "/images/altlogo.png";

		var imgReplacedHtml = htmlString
		  .Replace("cid:Logo", hostedLogo)
		  .Replace("cid:altLogo", hostedAltLogo);

		return imgReplacedHtml;
	}
}
