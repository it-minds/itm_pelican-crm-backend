using AutoMapper;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserDto>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	public async Task<Result<UserDto>> Handle(
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

		return _mapper.Map<UserDto>(user);
	}
}
