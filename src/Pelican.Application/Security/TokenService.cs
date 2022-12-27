using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Options;
using Pelican.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Pelican.Application.Security;

public class TokenService : ITokenService
{
	private const double EXPIRE_HOURS = 24.0;
	private const double SSO_EXPIRE_DAYS = 28.0;
	private readonly TokenOptions _options;
	public TokenService(IOptions<TokenOptions> options)
	{
		_options = options.Value;
	}

	public string CreateToken(User user)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, user.Email),
			new Claim(ClaimTypes.Role, user.Role.ToString())
		};

		var key = Encoding.ASCII.GetBytes(_options.Secret);
		var tokenHandler = new JwtSecurityTokenHandler();
		var descriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddHours(EXPIRE_HOURS),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(descriptor);
		return tokenHandler.WriteToken(token);
	}

	public (string, string) CreateSSOToken(User user)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, user.Email),
			//Give the token a unique identifier
			new Claim("jti", Guid.NewGuid().ToString())
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(_options.Secret);
		var descriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddDays(SSO_EXPIRE_DAYS),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(descriptor);
		var writtenToken = tokenHandler.WriteToken(token);

		return (token.Id, writtenToken);
	}

	public (string, string) ValidateSSOToken(string token)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(_options.Secret);

		var claims = tokenHandler.ValidateToken(token, new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(key),
			ValidateIssuer = false,
			ValidateAudience = false
		}, out SecurityToken validatedToken);

		var userEmail = claims.FindFirst(ClaimTypes.NameIdentifier);

		return (userEmail?.ToString() ?? string.Empty, validatedToken.Id);
	}
}
