using AutoMapper;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;

namespace Pelican.Application.Mails.UpdateEmail;

public sealed class UpdateEmailCommandHandler : ICommandHandler<UpdateEmailCommand, EmailDto>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public UpdateEmailCommandHandler(
		IUnitOfWork unitOfWork,
		IMapper mapper)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		_mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
	}

	public async Task<Result<EmailDto>> Handle(
		UpdateEmailCommand request,
		CancellationToken cancellationToken)
	{
		var email = await _unitOfWork
			.EmailRepository
			.FirstOrDefaultAsync(
				e => e.Id == request.Email.Id,
				cancellationToken);

		if (email is null)
		{
			return new Error(
				"Email.NotFound",
				$"Email with id: {request.Email.Id} not found");
		}

		email.Name = request.Email.Name;
		email.Subject = request.Email.Subject;
		email.Heading1 = request.Email.Heading1;
		email.Paragraph1 = request.Email.Paragraph1;
		email.Heading2 = request.Email.Heading2;
		email.Paragraph2 = request.Email.Paragraph2;
		email.Heading3 = request.Email.Heading3;
		email.Paragraph3 = request.Email.Paragraph3;
		email.CtaButtonText = request.Email.CtaButtonText;

		_unitOfWork.EmailRepository.Update(email);

		await _unitOfWork.SaveAsync(cancellationToken);

		return _mapper.Map<EmailDto>(email);
	}
}
