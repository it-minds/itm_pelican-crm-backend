using System;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Authentication.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, UserTokenDto>
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

	public async Task<Result<UserTokenDto>> Handle(
		LoginCommand command,
		CancellationToken cancellationToken)
	{
		User? user = await _unitOfWork
			.UserRepository
		  	.FirstOrDefaultAsync(x => x.Email.Equals(command.Email.ToLower()), cancellationToken);

		if (user is null)
		{
			return Result.Failure<UserTokenDto>(new Error("User.NotFound", "The user with the specified email was not found."));
		}

		var (verified, needsUpgrade) = _passwordHasher.Check(user.Password, command.Password);

		if (!verified)
		{
			return Result.Failure<UserTokenDto>(new Error("Authentication.InvalidEmailOrPassword", "The specified email or password are incorrect."));
		}

		var token = _tokenService.CreateToken(user);

		return new UserTokenDto
		{
			User = new()
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				Role = user.Role,
			},
			Token = token
		};
	}
}
