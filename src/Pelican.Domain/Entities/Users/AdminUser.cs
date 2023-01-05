using Pelican.Domain.Enums;

namespace Pelican.Domain.Entities.Users;
public class AdminUser : User
{
	public new RoleEnum Role { get; set; } = RoleEnum.Admin;
}
