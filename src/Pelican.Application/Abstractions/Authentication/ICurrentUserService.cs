namespace Pelican.Application.Abstractions.Authentication;
public interface ICurrentUserService
{
	string? UserId { get; }
}
