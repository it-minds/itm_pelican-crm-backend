using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Entities.Users;
using Pelican.Domain.Shared;

namespace Pelican.Application.Users.Commands.CreateAdmin;
public class CreateAdminCommandHandler : ICommandHandler<CreateAdminCommand, User>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IPasswordHasher _passwordHasher;

	public CreateAdminCommandHandler(
		IUnitOfWork unitOfWork,
		IPasswordHasher passwordHasher)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		_passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
	}
	public async Task<Result<User>> Handle(CreateAdminCommand request, CancellationToken cancellationToken)
	{
		if (await _unitOfWork.UserRepository.AnyAsync(x => x.Email == request.Email, cancellationToken))
		{
			return Result.Failure<User>(Error.AlreadyExists);
		}

		User userEntity = new AdminUser
		{
			Email = request.Email,
			Password = _passwordHasher.Hash(request.Password),
			Name = request.Name,
		};

		await _unitOfWork.UserRepository.CreateAsync(userEntity);
		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success(userEntity);
	}
}
