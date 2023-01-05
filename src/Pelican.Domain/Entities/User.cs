using Pelican.Domain.Enums;
using Pelican.Domain.Primitives;

namespace Pelican.Domain.Entities;
public abstract class User : Entity, ITimeTracked
{
	public string Email { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public RoleEnum Role { get; set; }
	public string SSOTokenId { get; set; } = string.Empty;
	public long CreatedAt { get; set; }
	public long? LastUpdatedAt { get; set; }
}
