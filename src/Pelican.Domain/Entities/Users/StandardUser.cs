using Pelican.Domain.Enums;

namespace Pelican.Domain.Entities.Users;
public class StandardUser : User
{
	public new RoleEnum Role { get; set; } = RoleEnum.Standard;
}
