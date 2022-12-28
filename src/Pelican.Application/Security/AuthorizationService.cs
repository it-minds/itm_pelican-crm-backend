using Pelican.Application.Abstractions.Authentication;
using Pelican.Domain.Enums;

namespace Pelican.Application.Security;

public class AuthorizationService : IAuthorizationService
{
	private readonly ICurrentUserService _currentUserService;

	public AuthorizationService(ICurrentUserService currentUserService)
	{
		_currentUserService = currentUserService;
	}

	public bool IsInRole(RoleEnum role) => role
		.ToString()
		.Equals(_currentUserService.Role);
}
