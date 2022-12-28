using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using Pelican.Application.Security;
using Xunit;

namespace Pelican.Application.Test.Security;

public class CurrentUserServiceTests
{

	private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
	private readonly CurrentUserService _uut;

	public CurrentUserServiceTests()
	{
		_uut = new(_httpContextAccessorMock.Object);
	}

	[Fact]
	public void UserId_HttpContextNull_ReturnsNull()
	{
		// Arrange
		_httpContextAccessorMock
			.Setup(h => h.HttpContext)
			.Returns((HttpContext)null!);

		// Act
		var result = _uut.UserId;

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void UserId_UserNull_ReturnsNull()
	{
		// Arrange
		_httpContextAccessorMock
			.Setup(h => h.HttpContext.User)
			.Returns((ClaimsPrincipal)null!);

		// Act
		var result = _uut.UserId;

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void UserId_ClaimNull_ReturnsNull()
	{
		// Arrange
		_httpContextAccessorMock
			.Setup(h => h.HttpContext.User.FindFirst(It.IsAny<string>()))
			.Returns((Claim)null!);

		// Act
		var result = _uut.UserId;

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void UserId_RoleEquals_ReturnsTrue()
	{
		// Arrange
		_httpContextAccessorMock
			.Setup(h => h.HttpContext.User.FindFirst(It.IsAny<string>()))
			.Returns(new Claim(ClaimTypes.NameIdentifier, "roleValue"));

		// Act
		var result = _uut.UserId;

		// Assert
		Assert.Equal(
			"roleValue",
			result);
	}
}
