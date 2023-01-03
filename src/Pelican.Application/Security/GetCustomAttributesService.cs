using System.Reflection;
using Pelican.Application.Abstractions.Authentication;

namespace Pelican.Application.Security;
public class GetCustomAttributesService : IGetCustomAttributesService
{
	public IEnumerable<AuthorizeAttribute> GetAttributes<TRequest>(TRequest request)
	{
		return request?.GetType().GetCustomAttributes<AuthorizeAttribute>() ?? new List<AuthorizeAttribute>();
	}
}
