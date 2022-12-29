using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication.Login;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Login.Commands;

public class LoginCommandHandlerTests
{

	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
	private readonly Mock<ITokenService> _tokenServiceMock = new();
	private readonly ICommandHandler<LoginCommand> _uut;

	public LoginCommandHandlerTests()
	{
		_uut = new LoginCommandHandler(
			_unitOfWorkMock.Object,
			_passwordHasherMock.Object,
			_tokenServiceMock.Object);
	}

	[Fact]
	public async void Handle_UserNotFound_ReturnsFailure()
	{
		// Arrange
		var command = new LoginCommand("email", "password");

		_unitOfWorkMock
			.Setup(u => u.UserRepository.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((User)null!);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(
			new Error("User.NotFound", "The user with the specified email was not found."),
			result.Error);
	}

	[Fact]
	public async void Handle_PasswordNotVerified_ReturnsFailure()
	{
		// Arrange
		var command = new LoginCommand("email", "password");

		var user = new StandardUser();

		_unitOfWorkMock
			.Setup(u => u.UserRepository.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(user);

		_passwordHasherMock
			.Setup(p => p.Check(
				It.IsAny<string>(),
				It.IsAny<string>()))
			.Returns((false, false));

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(
			new Error("Authentication.InvalidEmailOrPassword", "The specified email or password are incorrect."),
			result.Error);
	}

	// [Fact]
	// public async void Handle_PasswordNotVerified_ReturnsFailure()
	// {
	// 	// Arrange
	// 	var command = new LoginCommand("email", "password");

	// 	var user = new StandardUser();

	// 	_unitOfWorkMock
	// 		.Setup(u => u.UserRepository.FirstOrDefaultAsync(
	// 			It.IsAny<Expression<Func<User, bool>>>(),
	// 			It.IsAny<CancellationToken>()))
	// 		.ReturnsAsync(user);

	// 	_passwordHasherMock
	// 		.Setup(p => p.Check(
	// 			It.IsAny<string>(),
	// 			It.IsAny<string>()))
	// 		.Returns((true, true));

	// 	// Act
	// 	var result = await _uut.Handle(command, default);

	// 	// Assert
	// 	Assert.True(result.IsFailure);

	// 	Assert.Equal(
	// 		new Error("Authentication.InvalidEmailOrPassword", "The specified email or password are incorrect."),
	// 		result.Error);
	// }
}
