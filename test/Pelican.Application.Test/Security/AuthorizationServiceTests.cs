using Moq;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Security;
using Pelican.Domain.Enums;
using Xunit;

namespace Pelican.Application.Test.Security;

public class AuthorizationServiceTests
{
	private readonly Mock<ICurrentUserService> _currentUserServiceMock = new();
	private readonly AuthorizationService _uut;

	public AuthorizationServiceTests()
	{
		_uut = new(_currentUserServiceMock.Object);
	}

	[Fact]
	public void IsInRole_CurrentUserServiceReturnsNull_ReturnsFalse()
	{
		// Arrange
		const RoleEnum role = RoleEnum.Admin;

		_currentUserServiceMock
			.Setup(h => h.Role)
			.Returns((string)null!);

		// Act
		var result = _uut.IsInRole(role);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public void IsInRole_CurrentUserServiceReturnsNotMatchingString_ReturnsFalse()
	{
		// Arrange
		const RoleEnum role = RoleEnum.Admin;

		_currentUserServiceMock
			.Setup(h => h.Role)
			.Returns(string.Empty);

		// Act
		var result = _uut.IsInRole(role);

		// Assert
		Assert.False(result);
	}

	[Fact]
	public void IsInRole_CurrentUserServiceReturnsMatchingString_ReturnsFalse()
	{
		// Arrange
		const RoleEnum role = RoleEnum.Admin;

		_currentUserServiceMock
			.Setup(h => h.Role)
			.Returns(nameof(RoleEnum.Admin));

		// Act
		var result = _uut.IsInRole(role);

		// Assert
		Assert.True(result);
	}
}
