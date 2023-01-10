using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Mail;
using Pelican.Application.Mails.SendResetPassword;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Mails.SendResetPasswordMailCommand;
public class SendResetPasswordCommandHandlerUnitTest
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IMailService> _mailServiceMock = new();
	private readonly Mock<ITokenService> _tokenServiceMock = new();
	private readonly SendResetPasswordCommandHandler _uut;

	public SendResetPasswordCommandHandlerUnitTest()
	{
		_uut = new(
			_unitOfWorkMock.Object,
			_mailServiceMock.Object,
			_tokenServiceMock.Object);
	}

	[Fact]
	public void SendResetPasswordCommandHandler_UnitOfWorkNull_ThrowException()
	{
		// Act
		Exception exceptionResult = Record.Exception(() =>
			new SendResetPasswordCommandHandler(
				null!,
				_mailServiceMock.Object,
				_tokenServiceMock.Object));

		// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			exceptionResult.GetType());

		Assert.Equal(
			"Value cannot be null. (Parameter 'unitOfWork')",
			exceptionResult.Message);
	}

	[Fact]
	public void SendResetPasswordCommandHandler_MailServiceNull_ThrowException()
	{
		// Act
		Exception exceptionResult = Record.Exception(() =>
			new SendResetPasswordCommandHandler(
				_unitOfWorkMock.Object,
				null!,
				_tokenServiceMock.Object));

		// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			exceptionResult.GetType());

		Assert.Equal(
			"Value cannot be null. (Parameter 'mailService')",
			exceptionResult.Message);
	}

	[Fact]
	public void SendResetPasswordCommandHandler_TokenServiceNull_ThrowException()
	{
		// Act
		Exception exceptionResult = Record.Exception(() =>
			new SendResetPasswordCommandHandler(
				_unitOfWorkMock.Object,
				_mailServiceMock.Object,
				null!));

		// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			exceptionResult.GetType());

		Assert.Equal(
			"Value cannot be null. (Parameter 'tokenService')",
			exceptionResult.Message);
	}

	[Fact]
	public async void Handle_UserNotFound_ReturnsFailure()
	{
		// Arrange
		SendResetPasswordCommand command = new("testMail");

		_unitOfWorkMock
			.Setup(u => u
				.UserRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<User, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((User)null!);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equivalent(
			new Error("User.Null", "User with email: testMail was not found"),
			result.Error);

		_unitOfWorkMock.Verify(
			u => u.UserRepository
				.FirstOrDefaultAsync(
					x => x.Email == command.Email,
					default),
			Times.Once);
	}

	[Fact]
	public async void Handle_UserFound_ReturnsSuccess()
	{
		// Arrange
		SendResetPasswordCommand command = new("testMail");

		_unitOfWorkMock
			.Setup(u => u
				.UserRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<User, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(new StandardUser());

		_tokenServiceMock
			.Setup(x => x
				.CreateSSOToken(
					It.IsAny<User>()))
			.Returns(("testTokenId", "testToken"));

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsSuccess);

		_unitOfWorkMock.Verify(
			u => u.UserRepository
				.FirstOrDefaultAsync(
					x => x.Email == command.Email,
					default),
			Times.Once);

		_unitOfWorkMock.Verify(
			x => x.SaveAsync(default), Times.Once);

		_mailServiceMock
			.Verify(x => x
				.SendForgotPasswordEmailAsync(
					command.Email,
					"testToken"),
				Times.Once());
	}
}
