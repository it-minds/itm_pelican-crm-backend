using MediatR;
using Moq;
using Pelican.Application.Users.Commands.CreateAdmin;
using Pelican.Presentation.GraphQL.Users;
using Xunit;
using Pelican.Application.Users.Commands.UpdateUser;
using Pelican.Application.Users.Commands.UpdatePassword;
using Pelican.Application.Users.Commands.CreateStandardUser;
using Pelican.Application.Authentication;
using Pelican.Domain.Shared;

namespace Pelican.Presentation.GraphQL.Test;

public class UsersMutationTests
{
	private const string NAME = "name";
	private const string EMAIL = "email";
	private const string PASSWORD = "password";
	private readonly UsersMutation _uut = new();
	private readonly Mock<IMediator> _mediatorMock = new();

	[Fact]
	public async Task CreateStandardUser_MediatorReturnsError_IsFailureWithError()
	{
		// Arrange
		_mediatorMock
			.Setup(m => m.Send(
				It.IsAny<CreateStandardUserCommand>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Error.AlreadyExists);

		// Act
		var result = await _uut.CreateStandardUser(
			NAME,
			EMAIL,
			PASSWORD,
			_mediatorMock.Object,
			default);

		// Assert
		_mediatorMock.Verify(
			m => m.Send(It.Is<CreateStandardUserCommand>(
				x => x.Name == NAME && x.Email == EMAIL && x.Password == PASSWORD),
				default),
			Times.Once);

		Assert.False(result.IsSuccess);
		Assert.True(result.IsFailure);
		Assert.Null(result.Value);
		Assert.Equal(Error.AlreadyExists, result.Error);
	}

	[Fact]
	public async Task CreateStandardUser_MediatorReturnsValue_IsSuccessWithValue()
	{
		// Arrange
		UserDto returnValue = new() { Id = Guid.NewGuid() };

		_mediatorMock
			.Setup(m => m.Send(
				It.IsAny<CreateStandardUserCommand>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(returnValue);

		// Act
		var result = await _uut.CreateStandardUser(
			NAME,
			EMAIL,
			PASSWORD,
			_mediatorMock.Object,
			default);

		// Assert
		_mediatorMock.Verify(
			m => m.Send(It.Is<CreateStandardUserCommand>(
				x => x.Name == NAME && x.Email == EMAIL && x.Password == PASSWORD),
				default),
			Times.Once);

		Assert.True(result.IsSuccess);
		Assert.False(result.IsFailure);
		Assert.Equal(returnValue, result.Value);
		Assert.Equal(Error.None, result.Error);
	}

	[Fact]
	public async Task CreateAdmin_MediatorReturnsError_IsFailureWithError()
	{
		// Arrange
		_mediatorMock
			.Setup(m => m.Send(
				It.IsAny<CreateAdminCommand>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Error.AlreadyExists);

		// Act
		var result = await _uut.CreateAdmin(
			NAME,
			EMAIL,
			PASSWORD,
			_mediatorMock.Object,
			default);

		// Assert
		_mediatorMock.Verify(
			m => m.Send(It.Is<CreateAdminCommand>(
				x => x.Name == NAME && x.Email == EMAIL && x.Password == PASSWORD),
				default),
			Times.Once);

		Assert.False(result.IsSuccess);
		Assert.True(result.IsFailure);
		Assert.Null(result.Value);
		Assert.Equal(Error.AlreadyExists, result.Error);
	}

	[Fact]
	public async Task CreateAdmin_MediatorReturnsValue_IsSuccessWithValue()
	{
		// Arrange
		UserDto returnValue = new() { Id = Guid.NewGuid() };

		_mediatorMock
			.Setup(m => m.Send(
				It.IsAny<CreateAdminCommand>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(returnValue);

		// Act
		var result = await _uut.CreateAdmin(
			NAME,
			EMAIL,
			PASSWORD,
			_mediatorMock.Object,
			default);

		// Assert
		_mediatorMock.Verify(
			m => m.Send(It.Is<CreateAdminCommand>(
				x => x.Name == NAME && x.Email == EMAIL && x.Password == PASSWORD),
				default),
			Times.Once);

		Assert.True(result.IsSuccess);
		Assert.False(result.IsFailure);
		Assert.Equal(returnValue, result.Value);
		Assert.Equal(Error.None, result.Error);
	}

	[Fact]
	public async Task UpdateUser_MediatorReturnsError_IsFailureWithError()
	{
		// Arrange
		UserDto user = new();

		_mediatorMock
			.Setup(m => m.Send(
				It.IsAny<UpdateUserCommand>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Error.AlreadyExists);

		// Act
		var result = await _uut.UpdateUser(
			user,
			_mediatorMock.Object,
			default);

		// Assert
		_mediatorMock.Verify(
			m => m.Send(It.Is<UpdateUserCommand>(
				x => x.User == user),
				default),
			Times.Once);

		Assert.False(result.IsSuccess);
		Assert.True(result.IsFailure);
		Assert.Null(result.Value);
		Assert.Equal(Error.AlreadyExists, result.Error);
	}

	[Fact]
	public async Task UpdateUser_MediatorReturnsValue_IsSuccessWithValue()
	{
		// Arrange
		UserDto user = new();

		UserDto returnValue = new() { Id = Guid.NewGuid() };

		_mediatorMock
			.Setup(m => m.Send(
				It.IsAny<UpdateUserCommand>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(returnValue);

		// Act
		var result = await _uut.UpdateUser(
			user,
			_mediatorMock.Object,
			default);

		// Assert
		_mediatorMock.Verify(
			m => m.Send(It.Is<UpdateUserCommand>(
				x => x.User == user),
				default),
			Times.Once);

		Assert.True(result.IsSuccess);
		Assert.False(result.IsFailure);
		Assert.Equal(returnValue, result.Value);
		Assert.Equal(Error.None, result.Error);
	}

	[Fact]
	public async Task UpdatePassword_MediatorReturnsError_IsFailureWithError()
	{
		// Arrange
		_mediatorMock
			.Setup(m => m.Send(
				It.IsAny<UpdatePasswordCommand>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Error.AlreadyExists);

		// Act
		var result = await _uut.UpdatePassword(
			PASSWORD,
			_mediatorMock.Object,
			default);

		// Assert
		_mediatorMock.Verify(
			m => m.Send(It.Is<UpdatePasswordCommand>(
				x => x.Password == PASSWORD),
				default),
			Times.Once);

		Assert.False(result.IsSuccess);
		Assert.True(result.IsFailure);
		Assert.Null(result.Value);
		Assert.Equal(Error.AlreadyExists, result.Error);
	}

	[Fact]
	public async Task UpdatePassword_MediatorReturnsValue_IsSuccessWithValue()
	{
		// Arrange
		UserDto returnValue = new() { Id = Guid.NewGuid() };

		_mediatorMock
			.Setup(m => m.Send(
				It.IsAny<UpdatePasswordCommand>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(returnValue);

		// Act
		var result = await _uut.UpdatePassword(
			PASSWORD,
			_mediatorMock.Object,
			default);

		// Assert
		_mediatorMock.Verify(
			m => m.Send(It.Is<UpdatePasswordCommand>(
				x => x.Password == PASSWORD),
				default),
			Times.Once);

		Assert.True(result.IsSuccess);
		Assert.False(result.IsFailure);
		Assert.Equal(returnValue, result.Value);
		Assert.Equal(Error.None, result.Error);
	}
}
