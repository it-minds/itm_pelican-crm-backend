using Moq;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.HubSpot.Commands.NewInstallation;
using Pelican.Application.HubSpot.Dtos;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.HubSpot.Commands.NewInstallation;

public class NewInstallationCommandHandlerTests
{
	private readonly NewInstallationCommandHandler _uut;
	private readonly Mock<IHubSpotObjectService<AccountManager>> _hubSpotAccountManagerServiceMock;
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock;
	private readonly Mock<IHubSpotObjectService<Contact>> _hubSpotContactServiceMock;
	private readonly Mock<IHubSpotObjectService<Client>> _hubSpotClientServiceMock;
	private readonly Mock<IHubSpotObjectService<Deal>> _hubSpotDealServiceMock;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly Mock<IGenericRepository<Supplier>> _supplierRepositoryMock;
	private readonly CancellationToken cancellationToken;

	public NewInstallationCommandHandlerTests()
	{
		_hubSpotAccountManagerServiceMock = new();
		_hubSpotAuthorizationServiceMock = new();
		_hubSpotContactServiceMock = new();
		_hubSpotClientServiceMock = new();
		_hubSpotDealServiceMock = new();
		_unitOfWorkMock = new();
		_supplierRepositoryMock = new();
		cancellationToken = new();

		_uut = new NewInstallationCommandHandler(
			_hubSpotAccountManagerServiceMock.Object,
			_hubSpotAuthorizationServiceMock.Object,
			_hubSpotContactServiceMock.Object,
			_hubSpotClientServiceMock.Object,
			_hubSpotDealServiceMock.Object,
			_unitOfWorkMock.Object);
	}


	[Theory]
	[InlineData("code", "0", "fail")]
	public async void Handle_HubSpotServiceAuthorizeUserAsyncReturnsFailure_ReturnsResultIsFailureTrueWithError(
		string code,
		string errorCode,
		string errorMessage)
	{
		// Arrange
		var command = new NewInstallationCommand(code);

		_hubSpotAuthorizationServiceMock
			.Setup(
				h => h.AuthorizeUserAsync(command.Code, cancellationToken))
			.ReturnsAsync(
				Result.Failure<RefreshAccessTokens>(
					new Error(
						errorCode,
						errorMessage)));

		// Act 
		var result = await _uut.Handle(command, cancellationToken);

		// Assert
		_hubSpotAuthorizationServiceMock.Verify(
			h => h.AuthorizeUserAsync(command.Code, cancellationToken),
			Times.Once);

		_hubSpotAuthorizationServiceMock.Verify(
			h => h.DecodeAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
			Times.Never);

		_hubSpotAccountManagerServiceMock.Verify(
			h => h.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
			Times.Never);

		_hubSpotContactServiceMock.Verify(
			h => h.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
			Times.Never);

		_hubSpotClientServiceMock.Verify(
			h => h.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
			Times.Never);

		_hubSpotDealServiceMock.Verify(
			h => h.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
			Times.Never);

		Assert.True(result.IsFailure);

		Assert.Equal(errorCode, result.Error.Code);

		Assert.Equal(errorMessage, result.Error.Message);
	}

	[Theory]
	[InlineData("code", "token1", "token2")]
	public async void Handle_HubSpotServiceReturnsSuccess_ReturnsResultIsSuccessTrueWithErrorNone(
		string code,
		string token1,
		string token2)
	{
		// Arrange
		var command = new NewInstallationCommand(code);

		_hubSpotAuthorizationServiceMock
			.Setup(
				h => h.AuthorizeUserAsync(command.Code, cancellationToken))
			.ReturnsAsync(
				Result.Success(
					new RefreshAccessTokens
					{
						RefreshToken = token1,
						AccessToken = token2
					}));

		_hubSpotAuthorizationServiceMock
			.Setup(
				h => h.DecodeAccessTokenAsync(token2, cancellationToken))
			.ReturnsAsync(
				Result.Success(
					new Supplier()));

		_hubSpotAccountManagerServiceMock
			.Setup(
				h => h.GetAsync(token2, cancellationToken))
			.ReturnsAsync(
				Result.Success(
					new List<AccountManager>().AsEnumerable()));

		_hubSpotContactServiceMock
			.Setup(
				h => h.GetAsync(token2, cancellationToken))
			.ReturnsAsync(
				Result.Success(
					new List<Contact>().AsEnumerable()));

		_hubSpotClientServiceMock
			.Setup(
				h => h.GetAsync(token2, cancellationToken))
			.ReturnsAsync(
				Result.Success(
					new List<Client>().AsEnumerable()));

		_hubSpotDealServiceMock
			.Setup(
				h => h.GetAsync(token2, cancellationToken))
			.ReturnsAsync(
				Result.Success(
					new List<Deal>().AsEnumerable()));

		_unitOfWorkMock
			.Setup(
				u => u.SupplierRepository)
			.Returns(_supplierRepositoryMock.Object);

		// Act 
		var result = await _uut.Handle(command, cancellationToken);

		// Assert
		_hubSpotAuthorizationServiceMock.Verify(
			h => h.AuthorizeUserAsync(command.Code, cancellationToken),
			Times.Once);

		_hubSpotAuthorizationServiceMock.Verify(
			h => h.DecodeAccessTokenAsync(token2, cancellationToken),
			Times.Once);

		_hubSpotAccountManagerServiceMock.Verify(
			h => h.GetAsync(token2, cancellationToken),
			Times.Once);

		_hubSpotContactServiceMock.Verify(
			h => h.GetAsync(token2, cancellationToken),
			Times.Once);

		_hubSpotClientServiceMock.Verify(
			h => h.GetAsync(token2, cancellationToken),
			Times.Once);

		_hubSpotDealServiceMock.Verify(
			h => h.GetAsync(token2, cancellationToken),
			Times.Once);
	}
}
