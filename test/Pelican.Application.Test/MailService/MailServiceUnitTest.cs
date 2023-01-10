using System.Linq.Expressions;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Mail;
using Pelican.Application.Mail;
using Pelican.Application.Options;
using Pelican.Application.RazorEmails.Interfaces;
using Pelican.Application.RazorEmails.Views.CtaButtonEmail;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Mail;
public class MailServiceUnitTest
{
	private IMailService _uut;
	private Mock<IOptions<MailOptions>> _optionsMock = new();
	private Mock<IRazorViewToStringRenderer> _razorViewToStringRendererMock = new();
	private Mock<IUnitOfWork> _unitOfWorkMock = new();
	private Mock<ISmtpClientProvider> _smtpClientProviderMock = new();

	public MailServiceUnitTest()
	{
		_optionsMock.SetupGet(x => x.Value).Returns(new MailOptions()
		{
			Mail = "test@Mail.com",
			Host = "testHost",
			Port = 123,
			BaseUrl = "http://test.com"
		});
		_uut = new MailService(
			_optionsMock.Object,
			_razorViewToStringRendererMock.Object,
			_unitOfWorkMock.Object,
			_smtpClientProviderMock.Object);
	}
	[Fact]
	public void Constructor_OptionsIsNull_ReturnsArgumentNullException()
	{
		//Arrange
		_optionsMock.SetupGet(x => x.Value).Returns((MailOptions)null!);

		//Act
		var result = Record.Exception(()
			=> new MailService(
				_optionsMock.Object,
				_razorViewToStringRendererMock.Object,
				_unitOfWorkMock.Object,
				_smtpClientProviderMock.Object));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("mailOptions", result.Message);
	}

	[Fact]
	public void Constructor_RazorViewToStringRendererIsNull_ReturnsArgumentNullException()
	{
		//Arrange
		_optionsMock.SetupGet(x => x.Value).Returns(new MailOptions());

		//Act
		var result = Record.Exception(()
			=> new MailService(
				_optionsMock.Object,
				null!,
				_unitOfWorkMock.Object,
				_smtpClientProviderMock.Object));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("razorViewToStringRenderer", result.Message);
	}

	[Fact]
	public void Constructor_UnitOfWorkIsNull_ReturnsArgumentNullException()
	{
		//Arrange
		_optionsMock.SetupGet(x => x.Value).Returns(new MailOptions());

		//Act
		var result = Record.Exception(()
			=> new MailService(
				_optionsMock.Object,
				_razorViewToStringRendererMock.Object,
				null!,
				_smtpClientProviderMock.Object));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("unitOfWork", result.Message);
	}

	[Fact]
	public void Constructor_SmtpClientProviderIsNull_ReturnsArgumentNullException()
	{
		//Arrange
		_optionsMock.SetupGet(x => x.Value).Returns(new MailOptions());

		//Act
		var result = Record.Exception(()
			=> new MailService(
				_optionsMock.Object,
				_razorViewToStringRendererMock.Object,
				_unitOfWorkMock.Object,
				null!));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("smtpClientProvider", result.Message);
	}

	[Fact]
	public async void testAsync()
	{
		//Arrange
		_razorViewToStringRendererMock
			.Setup(x => x
				.RenderViewToStringAsync(
					It.IsAny<string>(),
					It.IsAny<CtaButtonEmailViewModel>()))
			.ReturnsAsync("testBody");

		_smtpClientProviderMock
			.Setup(x => x
				.SendEmailWithSmtpClient(
					It.IsAny<MailOptions>(),
					It.IsAny<MailMessage>()))
			.Returns(Result.Success());

		//Act
		var result = await _uut.TestSendEmailAsync();

		//Assert
		_razorViewToStringRendererMock
			.Verify(x => x
				.RenderViewToStringAsync(
					"/Views/CtaButtonEmail/CtaButtonEmail.cshtml",
					It.IsAny<CtaButtonEmailViewModel>()),
				Times.Once);

		_smtpClientProviderMock
			.Verify(x => x
				.SendEmailWithSmtpClient(
					_optionsMock.Object.Value,
					It.IsAny<MailMessage>()),
					Times.Once);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async void test1Async()
	{
		//Arrange
		_unitOfWorkMock
			.Setup(x => x
				.EmailRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Email, bool>>>(),
					It.IsAny<CancellationToken>()).Result)
			.Returns((Email)null!);

		//Act
		var result = await _uut.SendUserActivationEmailAsync("testEmail", "testToken");

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.EmailRepository
				.FirstOrDefaultAsync(
					x => x.EmailType == EmailTypeEnum.SendUserActivation,
					default),
				Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equivalent(
			new Error("EmailTemplate.NotFound", "No Template For Send User Activation Email has been set"),
			result.Error);
	}

	[Fact]
	public async void test2Async()
	{
		//Arrange
		_unitOfWorkMock
			.Setup(x => x
				.EmailRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Email, bool>>>(),
					It.IsAny<CancellationToken>()).Result)
			.Returns(new Email());

		_razorViewToStringRendererMock
			.Setup(x => x
				.RenderViewToStringAsync(
					It.IsAny<string>(),
					It.IsAny<CtaButtonEmailViewModel>()))
			.ReturnsAsync("testBody");

		_smtpClientProviderMock
			.Setup(x => x
				.SendEmailWithSmtpClient(
					It.IsAny<MailOptions>(),
					It.IsAny<MailMessage>()))
			.Returns(Result.Success());

		//Act
		var result = await _uut.SendUserActivationEmailAsync("test@Email.uk", "testToken");

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.EmailRepository
				.FirstOrDefaultAsync(
					x => x.EmailType == EmailTypeEnum.SendUserActivation,
					default),
				Times.Once);

		_razorViewToStringRendererMock
			.Verify(x => x
				.RenderViewToStringAsync(
					"/Views/CtaButtonEmail/CtaButtonEmail.cshtml",
					It.IsAny<CtaButtonEmailViewModel>()),
				Times.Once);

		_smtpClientProviderMock
			.Verify(x => x
				.SendEmailWithSmtpClient(
					_optionsMock.Object.Value,
					It.IsAny<MailMessage>()),
					Times.Once);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async void test3Async()
	{
		//Arrange
		_unitOfWorkMock
			.Setup(x => x
				.EmailRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Email, bool>>>(),
					It.IsAny<CancellationToken>()).Result)
			.Returns((Email)null!);

		//Act
		var result = await _uut.SendForgotPasswordEmailAsync("testEmail", "testToken");

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.EmailRepository
				.FirstOrDefaultAsync(
					x => x.EmailType == EmailTypeEnum.SendForgotPassword,
					default),
				Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equivalent(
			new Error("EmailTemplate.NotFound", "No Template For Forgot Password Email has been set"),
			result.Error);
	}

	[Fact]
	public async void test4Async()
	{
		//Arrange
		_unitOfWorkMock
			.Setup(x => x
				.EmailRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Email, bool>>>(),
					It.IsAny<CancellationToken>()).Result)
			.Returns(new Email());

		_razorViewToStringRendererMock
			.Setup(x => x
				.RenderViewToStringAsync(
					It.IsAny<string>(),
					It.IsAny<CtaButtonEmailViewModel>()))
			.ReturnsAsync("testBody");

		_smtpClientProviderMock
			.Setup(x => x
				.SendEmailWithSmtpClient(
					It.IsAny<MailOptions>(),
					It.IsAny<MailMessage>()))
			.Returns(Result.Success());

		//Act
		var result = await _uut.SendForgotPasswordEmailAsync("test@Email.uk", "testToken");

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.EmailRepository
				.FirstOrDefaultAsync(
					x => x.EmailType == EmailTypeEnum.SendForgotPassword,
					default),
				Times.Once);

		_razorViewToStringRendererMock
			.Verify(x => x
				.RenderViewToStringAsync(
					"/Views/CtaButtonEmail/CtaButtonEmail.cshtml",
					It.IsAny<CtaButtonEmailViewModel>()),
				Times.Once);

		_smtpClientProviderMock
			.Verify(x => x
				.SendEmailWithSmtpClient(
					_optionsMock.Object.Value,
					It.IsAny<MailMessage>()),
					Times.Once);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async void test5Async()
	{
		//Arrange
		_razorViewToStringRendererMock
			.Setup(x => x
				.RenderViewToStringAsync(
					It.IsAny<string>(),
					It.IsAny<CtaButtonEmailViewModel>()))
			.ReturnsAsync("testString:cid:Logo:cid:altLogo");

		EmailDto emailDto = new()
		{
			Id = 123,
			Heading1 = "testHeading1",
			Paragraph1 = "testParagraph1",
			Heading2 = "testHeading2",
			Paragraph2 = "testParagraph2",
			Heading3 = "testHeading3",
			Paragraph3 = "testParagraph3",
			CtaButtonText = "testCtaButtonText",
			Name = "testName",
			Subject = "testSubject",
		};

		//Act
		var result = await _uut.GeneratePreviewAsync(emailDto);

		//Assert
		_razorViewToStringRendererMock
			.Verify(x => x
				.RenderViewToStringAsync(
					"/Views/CtaButtonEmail/CtaButtonEmail.cshtml",
					It.IsAny<CtaButtonEmailViewModel>()),
				Times.Once);

		Assert.Equal(
			result,
			$"testString:{_optionsMock.Object.Value.BaseUrl}/images/logo.png:{_optionsMock.Object.Value.BaseUrl}/images/altlogo.png");
	}
}
