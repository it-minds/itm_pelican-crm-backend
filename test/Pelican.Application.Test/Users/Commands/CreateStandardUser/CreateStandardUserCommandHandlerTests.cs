using System.Linq.Expressions;
using AutoMapper;
using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Application.Users.Commands.CreateStandardUser;
using Pelican.Domain.Entities;
using Pelican.Domain.Enums;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Users.Commands.CreateStandardUser;

public class CreateStandardUserCommandHandlerTests
{

	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
	private readonly Mock<IMapper> _mapperMock = new();
	private readonly ICommandHandler<CreateStandardUserCommand, UserDto> _uut;

	public CreateStandardUserCommandHandlerTests()
	{
		_uut = new CreateStandardUserCommandHandler(
			_unitOfWorkMock.Object,
			_passwordHasherMock.Object,
			_mapperMock.Object);
	}

	[Fact]
	public void CreateStandardUserCommandHandler_UnitOfWorkNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new CreateStandardUserCommandHandler(
			null!,
			_passwordHasherMock.Object,
			_mapperMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("unitOfWork", result.Message);
	}

	[Fact]
	public void CreateStandardUserCommandHandler_PasswordHasherNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new CreateStandardUserCommandHandler(
			_unitOfWorkMock.Object,
			null!,
			_mapperMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("passwordHasher", result.Message);
	}

	[Fact]
	public void CreateStandardUserCommandHandler_MapperNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new CreateStandardUserCommandHandler(
			_unitOfWorkMock.Object,
			_passwordHasherMock.Object,
			null!));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains("mapper", result.Message);
	}

	[Fact]
	public async void Handle_UserAlreadyExists_ReturnsFailure()
	{
		// Arrange
		CreateStandardUserCommand command = new(
			"newName",
			"newEmail",
			"newPassword");

		_unitOfWorkMock
			.Setup(u => u.UserRepository.AnyAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(true);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(
			Error.AlreadyExists,
			result.Error);

		_unitOfWorkMock.Verify(
			u => u.UserRepository.AnyAsync(x => x.Email == command.Email, default),
			Times.Once);
	}

	[Fact]
	public async void Handle_NewUser_ReturnsSuccess()
	{
		// Arrange
		CreateStandardUserCommand command = new(
			"newName",
			"newEmail",
			"newPassword");

		_unitOfWorkMock
			.Setup(u => u.UserRepository.AnyAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(false);

		_passwordHasherMock
			.Setup(p => p.Hash(It.IsAny<string>()))
			.Returns("hashedPassword");

		UserDto userDto = new()
		{
			Name = command.Name,
			Email = command.Email,
			Role = RoleEnum.Standard,
		};

		_mapperMock
			.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
			.Returns(userDto);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsSuccess);

		Assert.Equal(
			userDto,
			result.Value);

		_unitOfWorkMock.Verify(
			u => u.UserRepository.AnyAsync(x => x.Email == command.Email, default),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.UserRepository.CreateAsync(
				It.Is<User>(x => x.Name == command.Name
					&& x.Email == command.Email
					&& x.Password == "hashedPassword"
					&& x.Role == RoleEnum.Standard),
				default),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);

		_mapperMock.Verify(
			m => m.Map<UserDto>(It.Is<User>(x => x.Name == command.Name
				&& x.Email == command.Email
				&& x.Password == "hashedPassword"
				&& x.Role == RoleEnum.Standard)),
			Times.Once);
	}
}
