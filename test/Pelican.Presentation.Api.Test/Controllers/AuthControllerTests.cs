using MediatR;
using Moq;
using Pelican.Application.Authentication.Login;
using Pelican.Presentation.Api.Controllers;
using Xunit;
using Pelican.Domain.Shared;
using Pelican.Application.Authentication;
using Microsoft.AspNetCore.Mvc;
using Pelican.Application.Authentication.CheckAuthCommand;

namespace Pelican.Presentation.Api.Test.Controllers;

public class AuthControllerTests
{
	private readonly Mock<ISender> _senderMock = new();
	private readonly AuthController _uut;

	public AuthControllerTests()
	{
		_uut = new(_senderMock.Object);
	}

	[Fact]
	public async void Login_CommandReturnsFailure_ReturnsBadRequest()
	{
		// Arrange
		const string EMAIL = "email";
		const string PASSWORD = "password";

		var commandResult = Result.Failure<UserTokenDto>(Error.NullValue);

		_senderMock
			.Setup(s => s.Send(It.IsAny<LoginCommand>(), default))
			.ReturnsAsync(commandResult);

		// Act
		var result = await _uut.Login(EMAIL, PASSWORD);

		// Then
		_senderMock.Verify(
			s => s.Send(It.Is<LoginCommand>(
				x => x.Email == EMAIL && x.Password == PASSWORD),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result.Result);
	}

	[Fact]
	public async void Login_CommandReturnsSuccess_ReturnsUserTokenDto()
	{
		// Arrange
		const string EMAIL = "email";
		const string PASSWORD = "password";

		var userTokenDto = new UserTokenDto();
		var commandResult = Result.Success(userTokenDto);

		_senderMock
			.Setup(s => s.Send(It.IsAny<LoginCommand>(), default))
			.ReturnsAsync(commandResult);

		// Act
		var result = await _uut.Login(EMAIL, PASSWORD);

		// Then
		_senderMock.Verify(
			s => s.Send(It.Is<LoginCommand>(
				x => x.Email == EMAIL && x.Password == PASSWORD),
				default),
			Times.Once);

		Assert.Equal(
			userTokenDto,
			result.Value);
	}

	[Fact]
	public async void CheckAuth_CommandReturnsFailure_ReturnsBadRequest()
	{
		// Arrange
		var commandResult = Result.Failure<UserDto>(Error.NullValue);

		_senderMock
			.Setup(s => s.Send(It.IsAny<CheckAuthCommand>(), default))
			.ReturnsAsync(commandResult);

		// Act
		var result = await _uut.CheckAuth();

		// Then
		_senderMock.Verify(
			s => s.Send(It.IsAny<CheckAuthCommand>(),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result.Result);
	}

	[Fact]
	public async void CheckAuth_CommandReturnsSuccess_ReturnsUserDto()
	{
		// Arrange
		var userDto = new UserDto();
		var commandResult = Result.Success(userDto);

		_senderMock
			.Setup(s => s.Send(It.IsAny<CheckAuthCommand>(), default))
			.ReturnsAsync(commandResult);

		// Act
		var result = await _uut.CheckAuth();

		// Then
		_senderMock.Verify(
			s => s.Send(It.IsAny<CheckAuthCommand>(),
				default),
			Times.Once);

		Assert.Equal(
			userDto,
			result.Value);
	}
}
