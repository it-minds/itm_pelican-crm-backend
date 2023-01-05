using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Authentication.CheckAuth;

public sealed class CheckAuthCommandHandler : ICommandHandler<CheckAuthCommand, UserDto>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentUserService _currentUserService;

	public CheckAuthCommandHandler(
		IUnitOfWork unitOfWork,
		ICurrentUserService currentUserService)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
	}

	public async Task<Result<UserDto>> Handle(
		CheckAuthCommand command,
		CancellationToken cancellationToken)
	{
		User? user = await _unitOfWork
			.UserRepository
		  	.FirstOrDefaultAsync(
				x => x.Email.Equals(_currentUserService.UserId),
				cancellationToken);

		if (user is null)
		{
			return new Error(
				"Auth.InvalidCredentials",
				"Invalid credentials");
		}

		return new UserDto()
		{
			Id = user.Id,
			Name = user.Name,
			Email = user.Email,
			Role = user.Role,
		};
	}
}
