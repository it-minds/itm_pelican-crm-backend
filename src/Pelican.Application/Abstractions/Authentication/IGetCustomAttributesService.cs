﻿using Pelican.Application.Security;

namespace Pelican.Application.Abstractions.Authentication;
public interface IGetCustomAttributesService
{
	public IEnumerable<AuthorizeAttribute> GetAuthorizeAttributes<TRequest>(TRequest request);
}
