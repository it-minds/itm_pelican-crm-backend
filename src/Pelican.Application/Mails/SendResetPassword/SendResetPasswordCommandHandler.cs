using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Mail;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Mails.SendResetPassword;
public sealed class SendResetPasswordCommandHandler : ICommandHandler<SendResetPasswordCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMailService _mailService;
	private readonly ITokenService _tokenService;
	public SendResetPasswordCommandHandler(
		IUnitOfWork unitOfWork,
		IMailService mailService,
		ITokenService tokenService)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		_mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
		_tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
	}
	public async Task<Result> Handle(SendResetPasswordCommand request, CancellationToken cancellationToken)
	{
		User? userEntity = await _unitOfWork.UserRepository.FirstOrDefaultAsync(e => e.Email == request.Email, cancellationToken);

		if (userEntity is null)
		{
			return Result.Failure(new Error(
				"User.Null",
				$"User with email: {request.Email} was not found"));
		}

		var (tokenId, token) = _tokenService.CreateSSOToken(userEntity);
		userEntity.SSOTokenId = tokenId;

		await _unitOfWork.SaveAsync(cancellationToken);

		await _mailService.SendForgotPasswordEmailAsync(request.Email, token);

		return Result.Success();
	}
}
