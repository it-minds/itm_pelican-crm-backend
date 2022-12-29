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
	private readonly TokenOptions _options;
	private readonly SecurityTokenHandler _tokenHandler;

	public TokenService(
		IOptions<TokenOptions> options,
		SecurityTokenHandler tokenHandler)
	{
		_options = options.Value;
		_tokenHandler = tokenHandler;
	}

	public string CreateToken(User user)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, user.Email),
			new Claim(ClaimTypes.Role, user.Role.ToString())
		};

		var key = Encoding.ASCII.GetBytes(_options.Secret);

		var descriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddHours(_options.ExpireHours),
			SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256Signature)
		};

		var token = _tokenHandler.CreateToken(descriptor);

		return _tokenHandler.WriteToken(token);
	}

	public (string, string) CreateSSOToken(User user)
	{
		var claims = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, user.Email),
			//Give the token a unique identifier
			new Claim("jti", Guid.NewGuid().ToString())
		};

		var key = Encoding.ASCII.GetBytes(_options.Secret);
		var descriptor = new SecurityTokenDescriptor
		{
			Subject = new ClaimsIdentity(claims),
			Expires = DateTime.UtcNow.AddDays(_options.SsoExpireDays),
			SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256Signature)
		};
		var token = _tokenHandler.CreateToken(descriptor);
		var writtenToken = _tokenHandler.WriteToken(token);

		return (token.Id, writtenToken);
	}

	public (string, string) ValidateSSOToken(string token)
	{
		var key = Encoding.ASCII.GetBytes(_options.Secret);

		var claims = _tokenHandler.ValidateToken(
			token,
			new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(key),
				ValidateIssuer = false,
				ValidateAudience = false
			},
			out SecurityToken validatedToken);

		var userEmail = claims.FindFirst(ClaimTypes.NameIdentifier);

		return (userEmail?.Value ?? string.Empty, validatedToken.Id);
	}
}
