using AutoMapper;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Authentication;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Pelican.Domain.Shared;

namespace Pelican.Application.Users.Commands.CreateAdmin;
public class CreateAdminCommandHandler : ICommandHandler<CreateAdminCommand, UserDto>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IPasswordHasher _passwordHasher;
	private readonly IMapper _mapper;

	public CreateAdminCommandHandler(
		IUnitOfWork unitOfWork,
		IPasswordHasher passwordHasher,
		IMapper mapper)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		_passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}
	public async Task<Result<UserDto>> Handle(
		CreateAdminCommand request,
		CancellationToken cancellationToken)
	{
		if (await _unitOfWork.UserRepository.AnyAsync(x => x.Email == request.Email, cancellationToken))
		{
			return Result.Failure<UserDto>(Error.AlreadyExists);
		}

		User user = new AdminUser
		{
			Email = request.Email,
			Password = _passwordHasher.Hash(request.Password),
			Name = request.Name,
		};

		await _unitOfWork.UserRepository.CreateAsync(user, cancellationToken);
		await _unitOfWork.SaveAsync(cancellationToken);

		return _mapper.Map<UserDto>(user);
	}
}
