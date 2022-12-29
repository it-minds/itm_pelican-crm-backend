

using System.Security.Claims;
using Microsoft.Extensions.Options;
using Moq;
using Pelican.Application.Options;
using Pelican.Application.Security;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Xunit;

namespace Pelican.Application.Test.Security;

public class TokenServiceTests
{
	private readonly Mock<IOptions<TokenOptions>> _optionsMock = new();
	private readonly Mock<SecurityTokenHandler> _tokenHandlerMock = new();
	private readonly TokenService _uut;

	public TokenServiceTests()
	{
		_optionsMock
			.Setup(o => o.Value)
			.Returns(new TokenOptions()
			{
				ExpireHours = 8,
				Secret = "very_secret",
			});

		_uut = new(
			_optionsMock.Object,
			_tokenHandlerMock.Object);
	}

	[Fact]
	public void CreateToken()
	{
		// Arrange
		var user = new StandardUser();

		var token = new JwtSecurityToken();

		_tokenHandlerMock
			.Setup(t => t.CreateToken(It.IsAny<SecurityTokenDescriptor>()))
			.Returns(token);

		_tokenHandlerMock
			.Setup(t => t.WriteToken(It.IsAny<SecurityToken>()))
			.Returns(token.ToString());

		// Act
		var result = _uut.CreateToken(user);

		// Assert
		Assert.Equal(
			token.ToString(),
			result
		);
	}

	[Fact]
	public void CreateSSOToken()
	{
		// Arrange
		var user = new StandardUser();

		var token = new JwtSecurityToken();

		_tokenHandlerMock
			.Setup(t => t.CreateToken(It.IsAny<SecurityTokenDescriptor>()))
			.Returns(token);

		_tokenHandlerMock
			.Setup(t => t.WriteToken(It.IsAny<SecurityToken>()))
			.Returns(token.ToString());

		// Act
		var (result0, result1) = _uut.CreateSSOToken(user);

		// Assert
		Assert.Null(result0);

		Assert.Equal(
			token.ToString(),
			result1
		);
	}

	[Fact]
	public void ValidateSSOToken()
	{
		// Arrange
		var userEmail = new Claim(ClaimTypes.NameIdentifier, "UserEmail");

		var claimsMock = new Mock<ClaimsPrincipal>();

		var guid = Guid.NewGuid().ToString();

		SecurityToken validationToken = new JwtSecurityToken(
			claims: new List<Claim>
			{
				new Claim("jti",guid)
			});

		_tokenHandlerMock
			.Setup(t => t.ValidateToken(
				It.IsAny<string>(),
				It.IsAny<TokenValidationParameters>(),
				out validationToken))
			.Returns(claimsMock.Object);

		claimsMock
			.Setup(t => t.FindFirst(It.IsAny<string>()))
			.Returns(userEmail);

		// Act
		var (result0, result1) = _uut.ValidateSSOToken("token");

		// Assert
		Assert.Equal(
			userEmail.Value,
			result0);

		Assert.Equal(
			guid,
			result1
		);
	}

	[Fact]
	public void ValidateSSOToken_NoEmailClaim()
	{
		// Arrange
		var claimsMock = new Mock<ClaimsPrincipal>();

		var guid = Guid.NewGuid().ToString();

		SecurityToken validationToken = new JwtSecurityToken(
			claims: new List<Claim>
			{
				new Claim("jti",guid)
			});

		_tokenHandlerMock
			.Setup(t => t.ValidateToken(
				It.IsAny<string>(),
				It.IsAny<TokenValidationParameters>(),
				out validationToken))
			.Returns(claimsMock.Object);

		claimsMock
			.Setup(t => t.FindFirst(It.IsAny<string>()))
			.Returns((Claim)null!);

		// Act
		var (result0, result1) = _uut.ValidateSSOToken("token");

		// Assert
		Assert.Equal(
			string.Empty,
			result0);

		Assert.Equal(
			guid,
			result1
		);
	}
}
