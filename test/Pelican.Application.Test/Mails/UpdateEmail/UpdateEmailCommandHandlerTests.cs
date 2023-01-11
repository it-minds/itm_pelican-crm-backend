using System.Linq.Expressions;
using AutoMapper;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Mails;
using Pelican.Application.Mails.UpdateEmail;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Mails.UpdateEmail;

public class UpdateEmailCommandHandlerTests
{
	private readonly UpdateEmailCommandHandler _uut;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IMapper> _mapperMock = new();

	public UpdateEmailCommandHandlerTests()
	{
		_uut = new(
			_unitOfWorkMock.Object,
			_mapperMock.Object);
	}

	[Fact]
	public void UpdateEmailCommandHandler_UnitOfWorkNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new UpdateEmailCommandHandler(
			null!,
			_mapperMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"unitOfWork",
			result.Message);
	}

	[Fact]
	public void UpdateEmailCommandHandler_MapperNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new UpdateEmailCommandHandler(
			_unitOfWorkMock.Object,
			null!));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"mapper",
			result.Message);
	}

	[Fact]
	public async void Handle_EmailNotFound_ReturnsFailure()
	{
		// Arrange
		Guid id = Guid.NewGuid();

		EmailDto newEmail = new()
		{
			Id = id,
			Name = "newName",
			Subject = "newSubject",
			Heading1 = "newHeading1",
			Paragraph1 = "newParagraph1",
			Heading2 = "newHeading2",
			Paragraph2 = "newParagraph2",
			Heading3 = "newHeading3",
			Paragraph3 = "newParagraph3",
			CtaButtonText = "newCtaButtonText",
		};

		UpdateEmailCommand command = new(newEmail);

		Email? existingEmail = null;

		_unitOfWorkMock
			.Setup(u => u.EmailRepository.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Email, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(existingEmail);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(
			new Error(
				"Email.NotFound",
				$"Email with id: {command.Email.Id} not found"),
			result.Error);
	}

	[Fact]
	public async void Handle_EmailFoundEmailUpdated_ReturnsSuccess()
	{
		// Arrange
		Guid id = Guid.NewGuid();

		EmailDto newEmail = new()
		{
			Id = id,
			Name = "newName",
			Subject = "newSubject",
			Heading1 = "newHeading1",
			Paragraph1 = "newParagraph1",
			Heading2 = "newHeading2",
			Paragraph2 = "newParagraph2",
			Heading3 = "newHeading3",
			Paragraph3 = "newParagraph3",
			CtaButtonText = "newCtaButtonText",
		};

		UpdateEmailCommand command = new(newEmail);

		Email? existingEmail = new()
		{
			Id = command.Email.Id,
			Name = "oldName",
			Subject = "oldSubject",
			Heading1 = "oldHeading1",
			Paragraph1 = "oldParagraph1",
			Heading2 = "oldHeading2",
			Paragraph2 = "oldParagraph2",
			Heading3 = "oldHeading3",
			Paragraph3 = "oldParagraph3",
			CtaButtonText = "oldCtaButtonText",
		};

		_unitOfWorkMock
			.Setup(u => u.EmailRepository.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Email, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(existingEmail);

		_mapperMock
			.Setup(m => m.Map<EmailDto>(It.IsAny<Email>()))
			.Returns(new EmailDto() { Id = command.Email.Id });

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsSuccess);

		_unitOfWorkMock.Verify(
			u => u.EmailRepository.Update(existingEmail),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);

		_mapperMock.Verify(
			m => m.Map<EmailDto>(existingEmail),
			Times.Once);
	}
}
