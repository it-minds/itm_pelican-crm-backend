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
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(paramName: nameof(unitOfWork));
	}

	public async Task<Result<User>> Handle(
		UpdateUserCommand command,
		CancellationToken cancellationToken)
	{
		User? user = await _unitOfWork
			.UserRepository
			.FirstOrDefaultAsync(
				expression: u => u.Id == command.User.Id,
				cancellationToken: cancellationToken);

		if (user is null)
		{
			return new Error(
				code: "User.NotFound",
				message: $"{command.User.Id} was not found");
		}

		if (command.User.Email is not null && await _unitOfWork
				.UserRepository
				.AnyAsync(
					expression: x => x.Email == command.User.Email,
					cancellationToken: cancellationToken))
		{
			return new Error(
				code: "Email.InUse",
				message: "Email already in use");
		}

		user.Name = command.User.Name ?? user.Name;
		user.Email = command.User.Email ?? user.Email;

		_unitOfWork
			.UserRepository
			.Update(entity: user);

		await _unitOfWork.SaveAsync(cancellationToken: cancellationToken);

		return user;
	}
}
