using Pelican.Domain.Enums;

namespace Pelican.Domain.Entities.Users;
public class StandardUser : User
{
	public override RoleEnum Role { get; set; } = RoleEnum.Standard;
}
