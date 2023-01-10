using AutoMapper;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Domain.Shared;

namespace Pelican.Application.Users.Commands.UpdateMe;

public class UpdateMeCommandHandler : ICommandHandler<UpdateMeCommand, UserDto>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public UpdateMeCommandHandler(
		IUnitOfWork unitOfWork,
		ICurrentUserService currentUserService,
		IMapper mapper)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		_currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	public async Task<Result<UserDto>> Handle(
		UpdateMeCommand request,
		CancellationToken cancellationToken)
	{
		var user = await _unitOfWork
			.UserRepository
		  	.FirstOrDefaultAsync(u => u.Id == request.User.Id, cancellationToken);

		if (user is null)
		{
			return new Error(
				"User.NotFound",
				$"User with Id: {request.User.Id} was not found");
		}

		if (user.Email != _currentUserService.UserId)
		{
			return new Error(
				"User.EmailNotFound",
				$"Email: {_currentUserService.UserId} was not found on user with Id: {request.User.Id}");
		}

		if (user.Email.ToLower().Trim() != request.User.Email.ToLower().Trim()
				&& await _unitOfWork
					.UserRepository
					.AnyAsync(
						u => u.Id != user.Id
							&& u.Email.ToLower().Trim() == request.User.Email.ToLower().Trim(),
						cancellationToken))
		{
			return new Error(
				"Email.InUse",
				"Email already in use");
		}

		user.Name = request.User.Name.Trim();
		user.Email = request.User.Email.ToLower().Trim();

		_unitOfWork.UserRepository.Attach(user);

		await _unitOfWork.SaveAsync(cancellationToken);

		return _mapper.Map<UserDto>(user);
	}
}
