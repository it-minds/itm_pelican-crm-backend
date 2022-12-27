using Pelican.Domain.Enums;

namespace Pelican.Application.Authentication;

public sealed class UserDto
{
	public Guid Id { get; set; }
	public string Email { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public RoleEnum Role { get; set; }
}
