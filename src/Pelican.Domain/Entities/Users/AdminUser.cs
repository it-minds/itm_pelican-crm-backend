using Pelican.Domain.Enums;

namespace Pelican.Domain.Entities.Users;
public class AdminUser : User
{
	public override RoleEnum Role { get; set; } = RoleEnum.Admin;
}
