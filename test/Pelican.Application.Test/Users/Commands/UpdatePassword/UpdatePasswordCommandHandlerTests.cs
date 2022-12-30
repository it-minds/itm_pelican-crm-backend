using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Users.Commands.UpdatePassword;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Users.Commands.UpdatePassword;

public class UpdatePasswordCommandHandlerTests
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
	private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
	private readonly UpdatePasswordCommandHandler _uut;

	public UpdatePasswordCommandHandlerTests()
	{
		_uut = new(
			_unitOfWorkMock.Object,
			_currentUserServiceMock.Object,
			_passwordHasherMock.Object);
	}

	[Fact]
	public async void Handle_UserNotFound_ReturnsFailure()
	{
		// Arrange
		UpdatePasswordCommand command = new("password");

		_unitOfWorkMock
			.Setup(u => u
				.UserRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<User, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((User)null!);

		_currentUserServiceMock
			.Setup(c => c.UserId)
			.Returns("id");

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(
			new Error("User.NotFound", "id was not found"),
			result.Error);
	}

	[Fact]
	public async void Handle_UserFoundAndUpdated_ReturnsSuccess()
	{
		// Arrange
		UpdatePasswordCommand command = new("password");

		_unitOfWorkMock
			.Setup(u => u
				.UserRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<User, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((User)null!);

		_currentUserServiceMock
			.Setup(c => c.UserId)
			.Returns("id");

		_passwordHasherMock
			.Setup(p => p.Hash(It.IsAny<string>()))
			.Returns("hashedPassword");

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsSuccess);
	}
}
