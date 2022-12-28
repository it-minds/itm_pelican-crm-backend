using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;
using Pelican.Application.Security;
using Pelican.Domain.Enums;
using Xunit;

namespace Pelican.Application.Test.Security;

public class AuthorizationServiceTests
{
	private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
	private readonly AuthorizationService _uut;

	public AuthorizationServiceTests()
	{
		_uut = new(_httpContextAccessorMock.Object);
	}

	[Fact]

	public void IsInRole_HttpContextNull_ReturnsFalse()
	{
		// Arrange
		const RoleEnum role = RoleEnum.Admin;

		_httpContextAccessorMock
			.Setup(h => h.HttpContext)
			.Returns((HttpContext)null!);

		// Act
		var result = _uut.IsInRole(role);

		// Assert
		Assert.False(result);
	}

	[Fact]

	public void IsInRole_UserNull_ReturnsFalse()
	{
		// Arrange
		const RoleEnum role = RoleEnum.Admin;

		_httpContextAccessorMock
			.Setup(h => h.HttpContext.User)
			.Returns((ClaimsPrincipal)null!);

		// Act
		var result = _uut.IsInRole(role);

		// Assert
		Assert.False(result);
	}

	[Fact]

	public void IsInRole_ClaimNull_ReturnsFalse()
	{
		// Arrange
		const RoleEnum role = RoleEnum.Admin;

		_httpContextAccessorMock
			.Setup(h => h.HttpContext.User.FindFirst(It.IsAny<string>()))
			.Returns((Claim)null!);

		// Act
		var result = _uut.IsInRole(role);

		// Assert
		Assert.False(result);
	}

	[Fact]

	public void IsInRole_RoleNotEquals_ReturnsFalse()
	{
		// Arrange
		const RoleEnum role = RoleEnum.Admin;

		_httpContextAccessorMock
			.Setup(h => h.HttpContext.User.FindFirst(It.IsAny<string>()))
			.Returns(new Claim(ClaimTypes.Role, "roleValue"));

		// Act
		var result = _uut.IsInRole(role);

		// Assert
		Assert.False(result);
	}

	[Fact]

	public void IsInRole_RoleEquals_ReturnsTrue()
	{
		// Arrange
		const RoleEnum role = RoleEnum.Admin;

		_httpContextAccessorMock
			.Setup(h => h.HttpContext.User.FindFirst(It.IsAny<string>()))
			.Returns(new Claim(ClaimTypes.Role, nameof(RoleEnum.Admin)));

		// Act
		var result = _uut.IsInRole(role);

		// Assert
		Assert.False(result);
	}
}
