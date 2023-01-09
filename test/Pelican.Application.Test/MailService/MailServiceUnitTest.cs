using Microsoft.Extensions.Options;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Mail;
using Pelican.Application.Options;
using Pelican.Application.RazorEmails.Interfaces;

namespace Pelican.Application.Test.Mail;
public class MailServiceUnitTest
{
	private MailService _uut;
	private Mock<IOptions<MailOptions>> _optionsMock = new();
	private Mock<IRazorViewToStringRenderer> _razorViewToStringRendererMock = new();
	private Mock<IUnitOfWork> _unitOfWorkMock = new();

	public MailServiceUnitTest()
	{
		_uut = new MailService(
			_optionsMock.Object,
			_razorViewToStringRendererMock.Object,
			_unitOfWorkMock.Object);
	}
}
