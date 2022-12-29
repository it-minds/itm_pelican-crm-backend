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
		_unitOfWork = unitOfWork;
		_currentUserService = currentUserService;
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
			// throw 401??
			throw new ArgumentException("Invalid credentials.");
		}

		// var inactiveClient = await _context.Clients
		//   .Include(c => c.Producers)
		//   .AnyAsync(c => c.Email == user.Email && c.Producers.All(p => p.DeactivationTime != null), cancellationToken);

		// if (inactiveClient)
		// {
		// 	throw new ArgumentException("This user is deactivated.");
		// }

		return new UserDto()
		{
			Name = user.Name,
		};
	}
}
