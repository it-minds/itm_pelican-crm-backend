using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Authentication.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IPasswordHasher _passwordHasher;
	private readonly ITokenService _tokenService;

	public LoginCommandHandler(
		IUnitOfWork unitOfWork,
		IPasswordHasher passwordHasher,
		ITokenService tokenService)
	{
		_unitOfWork = unitOfWork;
		_passwordHasher = passwordHasher;
		_tokenService = tokenService;
	}

	public async Task<Result> Handle(
		LoginCommand command,
		CancellationToken cancellationToken)
	{
		User? user = await _unitOfWork
			.UserRepository
		  	.FirstOrDefaultAsync(x => x.Email.Equals(command.Email.ToLower()), cancellationToken);

		// var inactiveClient = await _context.Clients
		//   .Include(c => c.Producers)
		//   .AnyAsync(c => c.Email == user.Email && c.Producers.All(p => p.DeactivationTime != null), cancellationToken);

		if (user is null)
		{
			return Result.Failure(new Error("User.NotFound", "The user with the specified email was not found."));
		}

		// if (inactiveClient)
		// {
		// 	throw new ArgumentException("This user is deactivated.");
		// }

		var (verified, needsUpgrade) = _passwordHasher.Check(user.Password, command.Password);

		if (!verified)
		{
			return Result.Failure(new Error("Authentication.InvalidEmailOrPassword", "The specified email or password are incorrect."));
		}

		var token = _tokenService.CreateToken(user);

		return Result.Success();
		// return new UserTokenDto
		// {
		// 	User = _mapper.Map<UserDto>(user),
		// 	Token = token
		// };
	}
}
