using System.Net;
using System.Net.Mail;
using Pelican.Application.Options;
using Pelican.Domain.Shared;

namespace Pelican.Application.Mail;
public class SmtpClientProvider
{
	public Result SendEmailWithSmtpClient(MailOptions mailOptions, MailMessage mailMessage)
	{
		SmtpClient client = new(mailOptions.Host, mailOptions.Port)
		{
			Credentials = new NetworkCredential(mailOptions.UserName, mailOptions.Password),
			EnableSsl = false
		};
		try
		{
			client.Send(mailMessage);
			return Result.Success();
		}
		catch
		{
			return Result.Failure(new Error("smtpClient.Error", "An error occured while sending an Email"));
		}
	}
}
