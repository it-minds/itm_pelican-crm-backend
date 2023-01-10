using System.Net.Mail;
using Pelican.Application.Options;
using Pelican.Domain.Shared;

namespace Pelican.Application.Abstractions.Mail;
public interface ISmtpClientProvider
{
	public Result SendEmailWithSmtpClient(MailOptions mailOptions, MailMessage mailMessage);
}
