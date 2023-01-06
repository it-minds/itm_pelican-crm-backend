using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, User>
{
	private readonly IUnitOfWork _unitOfWork;

	public UpdateUserCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result<User>> Handle(
		UpdateUserCommand command,
		CancellationToken cancellationToken)
	{
		User? user = await _unitOfWork
			.UserRepository
			.FirstOrDefaultAsync(
				u => u.Id == command.User.Id,
				cancellationToken);

		if (user is null)
		{
			return new Error(
				"User.NotFound",
				$"User with Id: {command.User.Id} was not found");
		}

		if (await _unitOfWork
				.UserRepository
				.AnyAsync(
					x => x.Email == command.User.Email,
					cancellationToken))
		{
			return new Error(
				"Email.InUse",
				"Email already in use");
		}

		user.Name = command.User.Name;
		user.Email = command.User.Email;

		_unitOfWork
			.UserRepository
			.Update(user);

		await _unitOfWork.SaveAsync(cancellationToken);

		return user;
	}
}
