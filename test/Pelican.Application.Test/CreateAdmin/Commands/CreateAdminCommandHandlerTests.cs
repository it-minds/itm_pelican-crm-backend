using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Users.Commands.CreateAdmin;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Xunit;

namespace Pelican.Application.Test.CreateAdmin.Commands;
public class CreateAdminCommandHandlerTests
{
	private readonly ICommandHandler<CreateAdminCommand, User> _uut;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IPasswordHasher> _passwordHasherMock = new();

	public CreateAdminCommandHandlerTests()
	{
		_uut = new CreateAdminCommandHandler(_unitOfWorkMock.Object, _passwordHasherMock.Object);
	}

	[Fact]
	public void Constructor_UnitOfWorkNull_ArgumentNullException()
	{
		//Act
		var result = Record.Exception(() => new CreateAdminCommandHandler(null!, _passwordHasherMock.Object));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"unitOfWork",
			result.Message);
	}

	[Fact]
	public void Constructor_PassWordHasherNull_ArgumentNullException()
	{
		//Act
		var result = Record.Exception(() => new CreateAdminCommandHandler(_unitOfWorkMock.Object, null!));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"passwordHasher",
			result.Message);

	}

	[Fact]
	public async void Handle_UserRepositoryAnyAsyncReturnsTrue_ReturnsFailure()
	{
		//Arrange
		CreateAdminCommand createAdminCommand = new("testName", "testEmail", "testPassword");

		_unitOfWorkMock
			.Setup(x => x
				.UserRepository
				.AnyAsync(
					It.IsAny<Expression<Func<User, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(true);

		AdminUser expectedUser = new()
		{
			Name = "testName",
			Password = "HashedPW",
			Email = "testEmail",
		};

		//Act
		var result = await _uut.Handle(createAdminCommand, default);

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.UserRepository
				.AnyAsync(x => x
					.Email == createAdminCommand.Email,
					default),
			Times.Once);

		Assert.True(result.IsFailure);
	}

	[Fact]
	public async void Handle_UserRepositoryAnyAsyncReturnsFalseSaveIsCalledCreateAsyncIsCalledWithExpectedUserPassWordHasherIsCalledWithExpectedPassword_ReturnsSuccess()
	{
		//Arrange
		CreateAdminCommand createAdminCommand = new("testName", "testEmail", "testPassword");

		_passwordHasherMock.Setup(x => x.Hash(It.IsAny<string>())).Returns("HashedPW");

		_unitOfWorkMock.Setup(x => x.UserRepository.CreateAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()));

		_unitOfWorkMock
			.Setup(x => x
				.UserRepository
				.AnyAsync(
					It.IsAny<Expression<Func<User, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(false);

		AdminUser expectedUser = new()
		{
			Name = "testName",
			Password = "HashedPW",
			Email = "testEmail",
		};

		//Act
		var result = await _uut.Handle(createAdminCommand, default);

		//Assert
		_unitOfWorkMock
			.Verify(x => x
				.UserRepository
				.AnyAsync(x => x
					.Email == createAdminCommand.Email,
					default),
			Times.Once);

		_unitOfWorkMock
			.Verify(x => x
				.UserRepository
				.CreateAsync(expectedUser, default),
			Times.Once);

		_unitOfWorkMock
			.Verify(x => x
				.SaveAsync(default),
			Times.Once);

		_passwordHasherMock
			.Verify(x => x
				.Hash("testPassword"),
			Times.Once);

		Assert.True(result.IsSuccess);

		Assert.Equal(expectedUser, result.Value);
	}
}
