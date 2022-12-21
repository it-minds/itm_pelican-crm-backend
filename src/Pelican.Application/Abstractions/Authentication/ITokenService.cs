namespace Pelican.Application.Abstractions.Authentication;
public interface ITokenService
{
	//TODO: Re add below lines when this branch has been rebased on dev after User has been added
	//string CreateToken(User user);
	//Task<(string, string)> CreateSSOToken(User user);
	Task<(string, string)> ValidateSSOToken(string token);
}
