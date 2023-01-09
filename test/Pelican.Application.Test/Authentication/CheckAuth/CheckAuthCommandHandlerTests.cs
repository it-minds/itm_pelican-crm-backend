using System.Data.Common;
using System.Linq.Expressions;
using AutoMapper;
using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Authentication;
using Pelican.Application.Authentication.CheckAuth;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Pelican.Domain.Enums;
using Pelican.Domain.Shared;
using Xunit;

public class CheckAuthCommandHandlerTests
{
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
	private readonly Mock<IMapper> _mapperMock = new();
	private readonly CheckAuthCommandHandler _uut;

	public CheckAuthCommandHandlerTests()
	{
		_uut = new(
			_unitOfWorkMock.Object,
			_currentUserServiceMock.Object,
			_mapperMock.Object);
	}

	[Fact]
	public void CheckAuthCommandHandler_UnitOfWorkNull_ThrowException()
	{
		// Act
		Exception exceptionResult = Record.Exception(() =>
			new CheckAuthCommandHandler(
				null!,
				_currentUserServiceMock.Object,
				_mapperMock.Object));

		// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			exceptionResult.GetType());

		Assert.Equal(
			"Value cannot be null. (Parameter 'unitOfWork')",
			exceptionResult.Message);
	}

	[Fact]
	public void CheckAuthCommandHandler_CurrentUserServiceNull_ThrowException()
	{
		// Act
		Exception exceptionResult = Record.Exception(() =>
			new CheckAuthCommandHandler(
				_unitOfWorkMock.Object,
				null!,
				_mapperMock.Object));

		// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			exceptionResult.GetType());

		Assert.Equal(
			"Value cannot be null. (Parameter 'currentUserService')",
			exceptionResult.Message);
	}

	[Fact]
	public void CheckAuthCommandHandler_MapperNull_ThrowException()
	{
		// Act
		Exception exceptionResult = Record.Exception(() =>
			new CheckAuthCommandHandler(
				_unitOfWorkMock.Object,
				_currentUserServiceMock.Object,
				null!));

		// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			exceptionResult.GetType());

		Assert.Equal(
			"Value cannot be null. (Parameter 'mapper')",
			exceptionResult.Message);
	}

	[Fact]
	public async void Handle_UserNotFound_ReturnsFailure()
	{
		// Arrange
		CheckAuthCommand command = new();

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

		Assert.Equal(
			new Error(
				"Auth.InvalidCredentials",
				"Invalid credentials"),
			result.Error);
	}

	[Fact]
	public async void Handle_UserFound_ReturnsSuccess()
	{
		// Arrange
		CheckAuthCommand command = new();

		StandardUser user = new()
		{
			Id = Guid.NewGuid(),
			Name = "name",
			Email = "email",
			Password = "psw",
			SSOTokenId = "ssoToken",
		};

		UserDto userDto = new()
		{
			Id = user.Id,
			Name = user.Name,
			Email = user.Email,
			Role = user.Role,
		};

		_unitOfWorkMock
			.Setup(u => u
				.UserRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<User, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(user);

		_mapperMock
			.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
			.Returns(userDto);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsSuccess);

		Assert.Equal(userDto, result.Value);
	}
}
