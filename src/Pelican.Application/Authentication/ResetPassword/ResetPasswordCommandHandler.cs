using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Authentication.ResetPassword;
public class ResetPasswordCommandHandler : ICommandHandler<ResetPasswordCommand, UserTokenDto>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ITokenService _tokenService;
	private readonly IPasswordHasher _passwordHasher;
	public ResetPasswordCommandHandler(
		IUnitOfWork unitOfWork,
		ITokenService tokenService,
		IPasswordHasher passwordHasher)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		_tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
		_passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
	}
	public async Task<Result<UserTokenDto>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
	{
		string userEmail;
		string tokenId;

		try
		{
			(userEmail, tokenId) = _tokenService.ValidateSSOToken(request.SSOToken);
		}
		catch (Exception)
		{
			return Result.Failure<UserTokenDto>(new Error("TokenService.Exception", "The provided token was invalid."));
		}

		User? userEntity = await _unitOfWork.UserRepository.FirstOrDefaultAsync(x => x.Email == userEmail, default);

		if (userEntity is null)
		{
			return Result.Failure<UserTokenDto>(new Error("User.Null", "User is null"));
		}

		if (userEntity.SSOTokenId != tokenId)
		{
			return Result.Failure<UserTokenDto>(
				new Error(
					"User.TokenIdArgument",
					$"The provided token did not match the expected token. Expected '{userEntity.SSOTokenId}' found '{tokenId}'."));
		}

		userEntity.SSOTokenId = null!;
		userEntity.Password = _passwordHasher.Hash(request.NewPassword);

		_unitOfWork.UserRepository.Update(userEntity);
		await _unitOfWork.SaveAsync(cancellationToken);

		var token = _tokenService.CreateToken(userEntity);

		var resultUserTokenDto = new UserTokenDto()
		{
			User = new UserDto()
			{
				Email = userEntity.Email,
				Id = userEntity.Id,
				Name = userEntity.Name,
				Role = userEntity.Role,
			},
			Token = token,
		};

		return Result.Success(resultUserTokenDto);
	}
}
