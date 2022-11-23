//using System.Linq.Expressions;
//using Moq;
//using Pelican.Application.Abstractions.HubSpot;
//using Pelican.Application.AccountManagers.Commands.ValidateWebhookUserId;
//using Pelican.Application.Common.Interfaces.Repositories;
//using Pelican.Domain.Entities;
//using Pelican.Domain.Shared;
//using Xunit;


//namespace Pelican.Application.Test.AccountManagers.Commands.ValidateWebhookUserId;

//public class ValidateWebhookUserIdCommandHandlerTests
//{
//	private readonly ValidateWebhookUserIdCommandHandler _uut;

//	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
//	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock = new();
//	private readonly Mock<IHubSpotObjectService<AccountManager>> _hubSpotAccountManagerServiceMock = new();

//	public ValidateWebhookUserIdCommandHandlerTests()
//	{
//		_uut = new(
//			_unitOfWorkMock.Object,
//			_hubSpotAuthorizationServiceMock.Object,
//			_hubSpotAccountManagerServiceMock.Object);
//	}

//	[Fact]
//	public void ValidateWebhookUserIdCommandHandler_UnitOfWorkNull_ThrowException()
//	{
//		// Act 
//		var result = Record.Exception(() => new ValidateWebhookUserIdCommandHandler(
//			null!,
//			_hubSpotAuthorizationServiceMock.Object,
//			_hubSpotAccountManagerServiceMock.Object));

//		// Assert
//		Assert.IsType<ArgumentNullException>(typeof(ArgumentNullException));

//		Assert.Contains("unitOfWork", result.Message);
//	}

//	[Fact]
//	public void ValidateWebhookUserIdCommandHandler_HubSpotAuthorizationServiceNull_ThrowException()
//	{
//		// Act 
//		var result = Record.Exception(() => new ValidateWebhookUserIdCommandHandler(
//			_unitOfWorkMock.Object,
//			null!,
//			_hubSpotAccountManagerServiceMock.Object));

//		// Assert
//		Assert.IsType<ArgumentNullException>(typeof(ArgumentNullException));

//		Assert.Contains("hubSpotAuthorizationService", result.Message);
//	}

//	[Fact]
//	public void ValidateWebhookUserIdCommandHandler_HubSpotAccountManagerServiceNull_ThrowException()
//	{
//		// Act 
//		var result = Record.Exception(() => new ValidateWebhookUserIdCommandHandler(
//			_unitOfWorkMock.Object,
//			_hubSpotAuthorizationServiceMock.Object,
//			null!));

//		// Assert
//		Assert.IsType<ArgumentNullException>(typeof(ArgumentNullException));

//		Assert.Contains("hubSpotAccountManagerService", result.Message);
//	}

//	[Fact]
//	public async Task Handle_AccountManagerExists_UnitOfWorkCalledReturnsSuccess()
//	{
//		// Arrange
//		ValidateWebhookUserIdCommand command = new(1, 1);

//		_unitOfWorkMock
//			.Setup(u => u.AccountManagerRepository.FirstOrDefaultAsync(
//				It.IsAny<Expression<Func<AccountManager, bool>>>(),
//				default))
//			.ReturnsAsync(new AccountManager(Guid.NewGuid()));

//		// Act 
//		var result = await _uut.Handle(command, default);

//		// Assert
//		_unitOfWorkMock.Verify(
//			u => u.AccountManagerRepository.FirstOrDefaultAsync(
//				a => a.HubSpotUserId == command.UserId,
//				default),
//			Times.Once);

//		Assert.True(result.IsSuccess);
//	}

//	[Fact]
//	public async Task Handle_NewAccountManagerRefreshingFailed_DependenciesCalledReturnsFailure()
//	{
//		// Arrange
//		ValidateWebhookUserIdCommand command = new(1, 1);

//		_unitOfWorkMock
//			.Setup(u => u.AccountManagerRepository.FirstOrDefaultAsync(
//				It.IsAny<Expression<Func<AccountManager, bool>>>(),
//				default))
//			.ReturnsAsync((AccountManager)null!);

//		_hubSpotAuthorizationServiceMock
//			.Setup(h => h.RefreshAccessTokenAsync(
//				It.IsAny<long>(),
//				It.IsAny<IUnitOfWork>(),
//				default))
//			.ReturnsAsync(Result.Failure<string>(Error.NullValue));

//		// Act 
//		var result = await _uut.Handle(command, default);

//		// Assert
//		_unitOfWorkMock.Verify(
//			u => u.AccountManagerRepository.FirstOrDefaultAsync(
//				a => a.HubSpotUserId == command.UserId,
//				default),
//			Times.Once);

//		_hubSpotAuthorizationServiceMock.Verify(
//			h => h.RefreshAccessTokenAsync(
//				command.SupplierHubSpotId,
//				_unitOfWorkMock.Object,
//				default),
//			Times.Once);

//		Assert.True(result.IsFailure);
//	}

//	[Fact]
//	public async Task Handle_NewAccountManagerFetchingAccountManagerFailed_DependenciesCalledReturnsFailure()
//	{
//		// Arrange
//		ValidateWebhookUserIdCommand command = new(1, 1);

//		string accessToken = "token";

//		_unitOfWorkMock
//			.Setup(u => u.AccountManagerRepository.FirstOrDefaultAsync(
//				It.IsAny<Expression<Func<AccountManager, bool>>>(),
//				default))
//			.ReturnsAsync((AccountManager)null!);

//		_hubSpotAuthorizationServiceMock
//			.Setup(h => h.RefreshAccessTokenAsync(
//				It.IsAny<long>(),
//				It.IsAny<IUnitOfWork>(),
//				default))
//			.ReturnsAsync(accessToken);

//		_hubSpotAccountManagerServiceMock
//			.Setup(h => h.GetByIdAsync(
//				It.IsAny<string>(),
//				It.IsAny<long>(),
//				default))
//			.ReturnsAsync(Result.Failure<AccountManager>(Error.NullValue));

//		// Act 
//		var result = await _uut.Handle(command, default);

//		// Assert
//		_unitOfWorkMock.Verify(
//			u => u.AccountManagerRepository.FirstOrDefaultAsync(
//				a => a.HubSpotUserId == command.UserId,
//				default),
//			Times.Once);

//		_hubSpotAuthorizationServiceMock.Verify(
//			h => h.RefreshAccessTokenAsync(
//				command.SupplierHubSpotId,
//				_unitOfWorkMock.Object,
//				default),
//			Times.Once);

//		_hubSpotAccountManagerServiceMock.Verify(
//			h => h.GetByIdAsync(
//				accessToken,
//				command.UserId,
//				default),
//			Times.Once);

//		Assert.True(result.IsFailure);
//	}

//	[Fact]
//	public async Task Handle_NewAccountManagerFetchingAccountManagerSucceeded_DependenciesCalledReturnsSuccess()
//	{
//		// Arrange
//		ValidateWebhookUserIdCommand command = new(1, 1);

//		string accessToken = "token";

//		AccountManager accountManager = new(Guid.NewGuid());

//		_unitOfWorkMock
//			.Setup(u => u.AccountManagerRepository.FirstOrDefaultAsync(
//				It.IsAny<Expression<Func<AccountManager, bool>>>(),
//				default))
//			.ReturnsAsync((AccountManager)null!);

//		_hubSpotAuthorizationServiceMock
//			.Setup(h => h.RefreshAccessTokenAsync(
//				It.IsAny<long>(),
//				It.IsAny<IUnitOfWork>(),
//				default))
//			.ReturnsAsync(accessToken);

//		_hubSpotAccountManagerServiceMock
//			.Setup(h => h.GetByIdAsync(
//				It.IsAny<string>(),
//				It.IsAny<long>(),
//				default))
//			.ReturnsAsync(accountManager);

//		// Act 
//		var result = await _uut.Handle(command, default);

//		// Assert
//		_unitOfWorkMock.Verify(
//			u => u.AccountManagerRepository.FirstOrDefaultAsync(
//				a => a.HubSpotUserId == command.UserId,
//				default),
//			Times.Once);

//		_hubSpotAuthorizationServiceMock.Verify(
//			h => h.RefreshAccessTokenAsync(
//				command.SupplierHubSpotId,
//				_unitOfWorkMock.Object,
//				default),
//			Times.Once);

//		_hubSpotAccountManagerServiceMock.Verify(
//			h => h.GetByIdAsync(
//				accessToken,
//				command.UserId,
//				default),
//			Times.Once);

//		_unitOfWorkMock.Verify(
//			u => u.AccountManagerRepository.CreateAsync(
//				accountManager,
//				default),
//			Times.Once);

//		_unitOfWorkMock.Verify(
//			u => u.SaveAsync(default),
//			Times.Once);

//		Assert.True(result.IsSuccess);
//	}
//}
