using AutoMapper;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Users.Commands.UpdatePassword;

public sealed class UpdatePasswordCommandHandler : ICommandHandler<UpdatePasswordCommand, UserDto>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentUserService _currentUserService;
	private readonly IPasswordHasher _passwordHasher;
	private readonly IMapper _mapper;

	public UpdatePasswordCommandHandler(
		IUnitOfWork unitOfWork,
		ICurrentUserService currentUserService,
		IPasswordHasher passwordHasher,
		IMapper mapper)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
		_passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	public async Task<Result<UserDto>> Handle(
		UpdatePasswordCommand command,
		CancellationToken cancellationToken)
	{
		string? userId = _currentUserService.UserId;

		User? user = await _unitOfWork
			.UserRepository
			.FirstOrDefaultAsync(
			 	x => x.Email == userId,
				cancellationToken);

		if (user is null)
		{
			return Result.Failure<UserDto>(new Error("User.NotFound", $"{userId} was not found"));
		}

		user.Password = _passwordHasher.Hash(command.Password);

		_unitOfWork
			.UserRepository
			.Update(user);

		await _unitOfWork.SaveAsync(cancellationToken);

		return _mapper.Map<UserDto>(user);
	}
}
