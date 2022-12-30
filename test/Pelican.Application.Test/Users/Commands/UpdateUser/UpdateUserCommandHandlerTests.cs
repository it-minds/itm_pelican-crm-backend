using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Authentication;
using Pelican.Application.Users.Commands.UpdateUser;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Users.Commands.UpdateUser;

public class UpdateUserCommandHandlerTests
{
	private readonly Mock<IUnitOfWork> _unitOfWork = new();
	private readonly UpdateUserCommandHandler _uut;

	public UpdateUserCommandHandlerTests()
	{
		_uut = new(_unitOfWork.Object);
	}

	[Fact]
	public void UpdateUserCommandHandler_UnitOfWorkNull_ThrowException()
	{
		// Act
		Exception exceptionResult = Record.Exception(() =>
			new UpdateUserCommandHandler(null!));

		// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			exceptionResult.GetType());

		Assert.Equal(
			"Value cannot be null. (Parameter 'unitOfWork')",
			exceptionResult.Message);
	}

	[Fact]
	public async void Handle_UserNotFound_ReturnFailure()
	{
		// Arrange
		UserDto user = new();

		UpdateUserCommand command = new(user);

		_unitOfWork
			.Setup(u => u.UserRepository.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((User)null!);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(
			new Error("User.NotFound", $"{command.User.Id} was not found"),
			result.Error);

		_unitOfWork.Verify(
			expression: x => x
				.UserRepository
				.FirstOrDefaultAsync(
					u => u.Id == command.User.Id,
					default),
			times: Times.Once);
	}

	[Fact]
	public async void Handle_EmailInUse_ReturnFailure()
	{
		// Arrange
		UserDto user = new()
		{
			Email = "email@email.com",
		};

		UpdateUserCommand command = new(user);

		User standardUser = new StandardUser();

		_unitOfWork
			.Setup(u => u.UserRepository.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(standardUser);

		_unitOfWork
			.Setup(u => u.UserRepository.AnyAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(true);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(
			new Error("Email.InUse", "Email already in use"),
			result.Error);

		_unitOfWork.Verify(
			expression: x => x
				.UserRepository
				.FirstOrDefaultAsync(
					u => u.Id == command.User.Id,
					default),
			times: Times.Once);

		_unitOfWork.Verify(
			expression: x => x
				.UserRepository
				.AnyAsync(
					u => u.Email == command.User.Email,
					default),
			times: Times.Once);
	}

	[Fact]
	public async void Handle_UserUpdated_ReturnFailure()
	{
		// Arrange
		UserDto user = new()
		{
			Email = "email@email.com",
		};

		UpdateUserCommand command = new(user);

		User standardUser = new StandardUser();

		_unitOfWork
			.Setup(u => u.UserRepository.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(standardUser);

		_unitOfWork
			.Setup(u => u.UserRepository.AnyAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(false);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsSuccess);

		_unitOfWork.Verify(
			expression: x => x
				.UserRepository
				.FirstOrDefaultAsync(
					u => u.Id == command.User.Id,
					default),
			times: Times.Once);

		_unitOfWork.Verify(
			expression: x => x
				.UserRepository
				.AnyAsync(
					u => u.Email == command.User.Email,
					default),
			times: Times.Once);

		_unitOfWork.Verify(
			expression: x => x
				.UserRepository
				.Update(standardUser),
			times: Times.Once);

		_unitOfWork.Verify(
			expression: x => x
				.SaveAsync(default),
			times: Times.Once);
	}
}
