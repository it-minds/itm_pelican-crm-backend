using Pelican.Domain.Enums;

namespace Pelican.Domain.Entities.Users;
public class AdminUser : User
{
	public AdminUser() { }
	public new RoleEnum Role { get; set; } = RoleEnum.Admin;
}
