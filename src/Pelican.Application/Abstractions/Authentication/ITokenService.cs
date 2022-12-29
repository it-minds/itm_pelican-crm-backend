using Pelican.Domain.Entities;

namespace Pelican.Application.Abstractions.Authentication;
public interface ITokenService
{
	string CreateToken(User user);
	(string, string) CreateSSOToken(User user);
	(string, string) ValidateSSOToken(string token);
}
