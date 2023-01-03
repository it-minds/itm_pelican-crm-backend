using MediatR;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Authentication;
using Pelican.Domain.Enums;

namespace Pelican.Application.Behaviours;
public class AuthorizationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IAuthorizationService _authorizationService;
	private readonly IGetCustomAttributesService _getCustomAttributesService;

	public AuthorizationBehaviour(
		ICurrentUserService currentUserService, IAuthorizationService authorizationService, IGetCustomAttributesService getCustomAttributesService)
	{
		_currentUserService = currentUserService;
		_authorizationService = authorizationService;
		_getCustomAttributesService = getCustomAttributesService;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var authorizeAttributes = _getCustomAttributesService.GetAttributes(request);

		if (authorizeAttributes.Any())
		{
			//Must be authenticated user
			if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
			{
				throw new UnauthorizedAccessException();
			}

			//Role-based authorization
			var authorizeAttributesWithRoles = authorizeAttributes.Where(a => Enum.IsDefined(typeof(RoleEnum), a.Role));

			if (authorizeAttributesWithRoles.Any())
			{
				foreach (var item in authorizeAttributesWithRoles.Select(a => a.Role))
				{
					var authorized = _authorizationService.IsInRole(item);

					if (!authorized)
					{
						throw new ForbiddenAccessException();
					}
				}
			}
		}

		//User is authorized or authorization not required.
		return await next();
	}
}
