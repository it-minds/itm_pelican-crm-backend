using System.Linq.Expressions;
using FluentValidation.TestHelper;
using Moq;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.AccountManagers.Commands.ValidateWebhookUserId;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.Deals.Commands.UpdateDeal;
using Pelican.Domain.Entities;
using Xunit;


namespace Pelican.Application.Test.AccountManagers.Commands.ValidateWebhookUserId;

public class ValidateWebhookUserIdCommandValidatorTests
{
	private readonly ValidateWebhookUserIdCommandValidator _uut;

	public ValidateWebhookUserIdCommandValidatorTests()
	{
		_uut = new();
	}

	[Fact]
	public void UpdateDealCommandValidator_EmptyStringOrDefaultValue_ReturnsError()
	{
		// Arrange
		ValidateWebhookUserIdCommand command = new(0, 0);

		// Act
		TestValidationResult<ValidateWebhookUserIdCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldHaveValidationErrorFor(command => command.UserId);
		result.ShouldHaveValidationErrorFor(command => command.SupplierHubSpotId);
	}

	[Fact]
	public void UpdateDealCommandValidator_NoEmptyStringsOrDefaultValues_ReturnsNoError()
	{
		// Arrange
		ValidateWebhookUserIdCommand command = new(1, 1);

		// Act
		TestValidationResult<ValidateWebhookUserIdCommand> result = _uut.TestValidate(command);

		// Assert
		result.ShouldNotHaveValidationErrorFor(command => command.UserId);
		result.ShouldNotHaveValidationErrorFor(command => command.SupplierHubSpotId);
	}

}

public class ValidateWebhookUserIdCommandHandlerTests
{
	private readonly ValidateWebhookUserIdCommandHandler _uut;

	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock = new();
	private readonly Mock<IHubSpotObjectService<AccountManager>> _hubSpotAccountManagerServiceMock = new();

	public ValidateWebhookUserIdCommandHandlerTests()
	{
		_uut = new(
			_unitOfWorkMock.Object,
			_hubSpotAuthorizationServiceMock.Object,
			_hubSpotAccountManagerServiceMock.Object);
	}

	[Fact]
	public void ValidateWebhookUserIdCommandHandler_UnitOfWorkNull_ThrowException()
	{
		// Act 
		var result = Record.Exception(() => new ValidateWebhookUserIdCommandHandler(
			null!,
			_hubSpotAuthorizationServiceMock.Object,
			_hubSpotAccountManagerServiceMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(typeof(ArgumentNullException));

		Assert.Contains("unitOfWork", result.Message);
	}

	[Fact]
	public void ValidateWebhookUserIdCommandHandler_HubSpotAuthorizationServiceNull_ThrowException()
	{
		// Act 
		var result = Record.Exception(() => new ValidateWebhookUserIdCommandHandler(
			_unitOfWorkMock.Object,
			null!,
			_hubSpotAccountManagerServiceMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(typeof(ArgumentNullException));

		Assert.Contains("hubSpotAuthorizationService", result.Message);
	}

	[Fact]
	public void ValidateWebhookUserIdCommandHandler_HubSpotAccountManagerServiceNull_ThrowException()
	{
		// Act 
		var result = Record.Exception(() => new ValidateWebhookUserIdCommandHandler(
			_unitOfWorkMock.Object,
			_hubSpotAuthorizationServiceMock.Object,
			null!));

		// Assert
		Assert.IsType<ArgumentNullException>(typeof(ArgumentNullException));

		Assert.Contains("hubSpotAccountManagerService", result.Message);
	}

	[Fact]
	public async Task Handle_AccountManagerExists_UnitOfWorkCalledReturnsSuccess()
	{
		// Arrange
		ValidateWebhookUserIdCommand command = new(1, 1);

		_unitOfWorkMock
			.Setup(u => u.AccountManagerRepository.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<AccountManager, bool>>>(),
				default))
			.ReturnsAsync(new AccountManager(Guid.NewGuid()));

		// Act 
		var result = await _uut.Handle(command, default);

		// Assert
		_unitOfWorkMock.Verify(
			u => u.AccountManagerRepository.FirstOrDefaultAsync(
				a => a.HubSpotUserId == command.UserId,
				default),
			Times.Once);

		Assert.True(result.IsSuccess);
	}
}
