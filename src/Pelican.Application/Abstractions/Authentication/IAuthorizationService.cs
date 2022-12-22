using Pelican.Domain.Enums;

namespace Pelican.Application.Abstractions.Authentication;
public interface IAuthorizationService
{
	bool IsInRole(RoleEnum role);
}
