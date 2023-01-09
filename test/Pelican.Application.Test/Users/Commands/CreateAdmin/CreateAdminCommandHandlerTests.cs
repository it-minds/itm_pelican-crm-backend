using System.Linq.Expressions;
using AutoMapper;
using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Application.Users.Commands.CreateAdmin;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Pelican.Domain.Enums;
using Xunit;

namespace Pelican.Application.Test.Users.Commands.CreateAdmin;
public class CreateAdminCommandHandlerTests
{
	private readonly ICommandHandler<CreateAdminCommand, UserDto> _uut;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
	private readonly Mock<IMapper> _mapperMock = new();

	public CreateAdminCommandHandlerTests()
	{
		_uut = new CreateAdminCommandHandler(
			_unitOfWorkMock.Object,
			_passwordHasherMock.Object,
			_mapperMock.Object);
	}

	[Fact]
	public void Constructor_UnitOfWorkNull_ArgumentNullException()
	{
		//Act
		var result = Record.Exception(() => new CreateAdminCommandHandler(
			null!,
			_passwordHasherMock.Object,
			_mapperMock.Object));

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
		var result = Record.Exception(() => new CreateAdminCommandHandler(
			_unitOfWorkMock.Object,
			null!,
			_mapperMock.Object));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"passwordHasher",
			result.Message);
	}

	[Fact]
	public void Constructor_MapperNull_ArgumentNullException()
	{
		//Act
		var result = Record.Exception(() => new CreateAdminCommandHandler(
			_unitOfWorkMock.Object,
			_passwordHasherMock.Object,
			null!));

		//Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"mapper",
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

		UserDto expectedUser = new()
		{
			Name = "testName",
			Email = "testEmail",
			Role = RoleEnum.Admin
		};

		_mapperMock
			.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
			.Returns(expectedUser);

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
				.CreateAsync(
					It.Is<AdminUser>(u => u.Name == expectedUser.Name
						&& u.Email == expectedUser.Email
						&& u.Role == expectedUser.Role),
					default),
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

		Assert.Equal(expectedUser.Name, result.Value.Name);
		Assert.Equal(expectedUser.Email, result.Value.Email);
		Assert.Equal(expectedUser.Role, result.Value.Role);
	}
}
