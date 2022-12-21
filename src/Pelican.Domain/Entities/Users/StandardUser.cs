﻿using Pelican.Domain.Enums;

namespace Pelican.Domain.Entities.Users;
public class StandardUser : User
{
	public StandardUser() { }
	public RoleEnum Role { get; set; } = RoleEnum.Standard;
}
