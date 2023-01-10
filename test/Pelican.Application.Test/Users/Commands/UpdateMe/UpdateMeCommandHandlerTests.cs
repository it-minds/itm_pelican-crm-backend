using System.Linq.Expressions;
using AutoMapper;
using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Application.Users.Commands.UpdateMe;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Pelican.Domain.Enums;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Users.Commands.UpdateMe;

public class UpdateMeCommandHandlerTest
{
	private readonly ICommandHandler<UpdateMeCommand, UserDto> _uut;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
	private readonly Mock<IMapper> _mapperMock = new();
	public UpdateMeCommandHandlerTest()
	{
		_uut = new UpdateMeCommandHandler(
			_unitOfWorkMock.Object,
			_currentUserServiceMock.Object,
			_mapperMock.Object);
	}

	[Fact]
	public void UpdateMeCommandHandler_UnitOfWorkNull_ThrowError()
	{
		// Act
		var result = Record.Exception(() => new UpdateMeCommandHandler(
			null!,
			_currentUserServiceMock.Object,
			_mapperMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"unitOfWork",
			result.Message);
	}

	[Fact]
	public void UpdateMeCommandHandler_CurrentUserServiceNull_ThrowError()
	{
		// Act
		var result = Record.Exception(() => new UpdateMeCommandHandler(
			_unitOfWorkMock.Object,
			null!,
			_mapperMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"currentUserService",
			result.Message);
	}

	[Fact]
	public void UpdateMeCommandHandler_MapperNull_ThrowError()
	{
		// Act
		var result = Record.Exception(() => new UpdateMeCommandHandler(
			_unitOfWorkMock.Object,
			_currentUserServiceMock.Object,
			null!));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"mapper",
			result.Message);
	}

	[Fact]
	public async void Handle_UserNotFound_ReturnFailure()
	{
		// Arrange
		UpdateMeCommand command = new(
			new()
			{
				Id = Guid.NewGuid(),
				Name = "newName",
				Email = "newEmail",
				Role = RoleEnum.Standard,
			}
		);

		User? foundUser = null;

		_unitOfWorkMock
			.Setup(u => u.UserRepository.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(foundUser);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(
			new Error(
				"User.NotFound",
				$"User with Id: {command.User.Id} was not found"),
			result.Error);
	}

	[Fact]
	public async void Handle_EmailNotFound_ReturnFailure()
	{
		// Arrange
		UpdateMeCommand command = new(
			new()
			{
				Id = Guid.NewGuid(),
				Name = "newName",
				Email = "newEmail",
				Role = RoleEnum.Standard,
			}
		);

		User? foundUser = new StandardUser()
		{
			Id = command.User.Id,
			Name = "oldName",
			Email = "oldEmail",
		};

		_unitOfWorkMock
			.Setup(u => u.UserRepository.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(foundUser);

		_currentUserServiceMock
			.Setup(u => u.UserId)
			.Returns("anotherEmail");

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(
			new Error(
				"User.EmailNotFound",
				$"Email: anotherEmail was not found on user with Id: {command.User.Id}"),
			result.Error);
	}

	[Fact]
	public async void Handle_EmailInUse_ReturnFailure()
	{
		// Arrange
		UpdateMeCommand command = new(
			new()
			{
				Id = Guid.NewGuid(),
				Name = "newName",
				Email = "newEmail",
				Role = RoleEnum.Standard,
			}
		);

		User? foundUser = new StandardUser()
		{
			Id = command.User.Id,
			Name = "oldName",
			Email = "oldEmail",
		};

		_unitOfWorkMock
			.Setup(u => u.UserRepository.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(foundUser);

		_currentUserServiceMock
			.Setup(u => u.UserId)
			.Returns("oldEmail");

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
			new Error(
				"Email.InUse",
				"Email already in use"),
			result.Error);
	}

	[Fact]
	public async void Handle_NameUpdated_ReturnSuccess()
	{
		// Arrange
		UpdateMeCommand command = new(
			new()
			{
				Id = Guid.NewGuid(),
				Name = "newName",
				Email = "oldEmail",
				Role = RoleEnum.Standard,
			}
		);

		User? foundUser = new StandardUser()
		{
			Id = command.User.Id,
			Name = "oldName",
			Email = "oldEmail",
		};

		UserDto returnUserDto = command.User;

		_unitOfWorkMock
			.Setup(u => u.UserRepository.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(foundUser);

		_currentUserServiceMock
			.Setup(u => u.UserId)
			.Returns("oldEmail");

		_unitOfWorkMock
			.Setup(u => u.UserRepository.AnyAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(false);

		_mapperMock
			.Setup(u => u.Map<UserDto>(It.IsAny<User>()))
			.Returns(returnUserDto);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsSuccess);

		Assert.Equal(
			returnUserDto,
			result.Value);

		_unitOfWorkMock.Verify(
			u => u.UserRepository.Attach(foundUser),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);
	}

	[Fact]
	public async void Handle_NameAndEmailUpdated_ReturnSuccess()
	{
		// Arrange
		UpdateMeCommand command = new(
			new()
			{
				Id = Guid.NewGuid(),
				Name = "newName",
				Email = "newEmail",
				Role = RoleEnum.Standard,
			}
		);

		User? foundUser = new StandardUser()
		{
			Id = command.User.Id,
			Name = "oldName",
			Email = "oldEmail",
		};

		UserDto returnUserDto = command.User;

		_unitOfWorkMock
			.Setup(u => u.UserRepository.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(foundUser);

		_currentUserServiceMock
			.Setup(u => u.UserId)
			.Returns("oldEmail");

		_unitOfWorkMock
			.Setup(u => u.UserRepository.AnyAsync(
				It.IsAny<Expression<Func<User, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(false);

		_mapperMock
			.Setup(u => u.Map<UserDto>(It.IsAny<User>()))
			.Returns(returnUserDto);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsSuccess);

		Assert.Equal(
			returnUserDto,
			result.Value);

		_unitOfWorkMock.Verify(
			u => u.UserRepository.Attach(foundUser),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);
	}
}
