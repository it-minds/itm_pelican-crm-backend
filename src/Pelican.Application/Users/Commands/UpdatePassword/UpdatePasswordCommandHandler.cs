
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Users.Commands.UpdatePassword;

public sealed class UpdatePasswordCommandHandler : ICommandHandler<UpdatePasswordCommand, User>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentUserService _currentUserService;
	private readonly IPasswordHasher _passwordHasher;

	public UpdatePasswordCommandHandler(
		IUnitOfWork unitOfWork,
		ICurrentUserService currentUserService,
		IPasswordHasher passwordHasher)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
		_passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
	}

	public async Task<Result<User>> Handle(
		UpdatePasswordCommand command,
		CancellationToken cancellationToken)
	{
		User? user = await _unitOfWork
			.UserRepository
			.FirstOrDefaultAsync(
				x => x.Email == _currentUserService.UserId,
				cancellationToken);

		if (user is null)
		{
			return Result.Failure<User>(new Error("User.NotFound", $"{_currentUserService.UserId} was not found"));
		}

		user.Password = _passwordHasher.Hash(command.Password);

		_unitOfWork
			.UserRepository
			.Update(user);

		await _unitOfWork.SaveAsync(cancellationToken);

		return user;
	}
}
