using MediatR;
using Moq;
using Pelican.Application.Users.Commands.CreateAdmin;
using Pelican.Presentation.GraphQL.Users;
using Xunit;
using Pelican.Application.Users.Commands.UpdateUser;
using Pelican.Application.Users.Commands.UpdatePassword;
using Pelican.Application.Users.Commands.CreateStandardUser;
using Pelican.Application.Authentication;
using Pelican.Application.Users.Commands.UpdateMe;

namespace Pelican.Presentation.GraphQL.Test;

public class UsersMutationTests
{
	private readonly UsersMutation _uut = new();

	[Fact]
	public async Task CreateStandardUser_VerifyCommandSend()
	{
		// Arrange
		const string NAME = "name";
		const string EMAIL = "email";
		const string PASSWORD = "password";

		Mock<IMediator> mediatorMock = new();

		// Act
		var result = await _uut.CreateStandardUser(
			NAME,
			EMAIL,
			PASSWORD,
			mediatorMock.Object,
			default);

		// Assert
		mediatorMock.Verify(
			m => m.Send(It.Is<CreateStandardUserCommand>(
				x => x.Name == NAME && x.Email == EMAIL && x.Password == PASSWORD),
				default),
			Times.Once);
	}

	[Fact]
	public async Task CreateAdmin_VerifyCommandSend()
	{
		// Arrange
		const string NAME = "name";
		const string EMAIL = "email";
		const string PASSWORD = "password";

		Mock<IMediator> mediatorMock = new();

		// Act
		var result = await _uut.CreateAdmin(
			NAME,
			EMAIL,
			PASSWORD,
			mediatorMock.Object,
			default);

		// Assert
		mediatorMock.Verify(
			m => m.Send(It.Is<CreateAdminCommand>(
				x => x.Name == NAME && x.Email == EMAIL && x.Password == PASSWORD),
				default),
			Times.Once);
	}

	[Fact]
	public async Task UpdateUser_VerifyCommandSend()
	{
		// Arrange
		UserDto user = new();

		Mock<IMediator> mediatorMock = new();

		// Act
		var result = await _uut.UpdateUser(
			user,
			mediatorMock.Object,
			default);

		// Assert
		mediatorMock.Verify(
			m => m.Send(It.Is<UpdateUserCommand>(
				x => x.User == user),
				default),
			Times.Once);
	}

	[Fact]
	public async Task UpdateMe_VerifyCommandSend()
	{
		// Arrange
		UserDto user = new();

		Mock<IMediator> mediatorMock = new();

		// Act
		var result = await _uut.UpdateMe(
			user,
			mediatorMock.Object,
			default);

		// Assert
		mediatorMock.Verify(
			m => m.Send(It.Is<UpdateMeCommand>(
				x => x.User == user),
				default),
			Times.Once);
	}

	[Fact]
	public async Task UpdatePassword_VerifyCommandSend()
	{
		// Arrange
		const string PASSWORD = "password";

		Mock<IMediator> mediatorMock = new();

		// Act
		var result = await _uut.UpdatePassword(
			PASSWORD,
			mediatorMock.Object,
			default);

		// Assert
		mediatorMock.Verify(
			m => m.Send(It.Is<UpdatePasswordCommand>(
				x => x.Password == PASSWORD),
				default),
			Times.Once);
	}
}
