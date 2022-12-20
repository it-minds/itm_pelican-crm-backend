namespace Pelican.Application.Test.HubSpot.Commands.NewInstallation;

public class NewInstallationCommandHandlerTests
{
	private readonly NewInstallationHubSpotCommandHandler _uut;
	private readonly Mock<IHubSpotOwnersService> _accountManagerServiceMock = new();
	private readonly Mock<IHubSpotAuthorizationService> _authorizationServiceMock = new();
	private readonly Mock<IHubSpotObjectService<Contact>> _contactServiceMock = new();
	private readonly Mock<IHubSpotObjectService<Client>> _clientServiceMock = new();
	private readonly Mock<IHubSpotObjectService<Deal>> _dealServiceMock = new();
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

	public NewInstallationCommandHandlerTests() => _uut = new NewInstallationHubSpotCommandHandler(
			_accountManagerServiceMock.Object,
			_authorizationServiceMock.Object,
			_contactServiceMock.Object,
			_clientServiceMock.Object,
			_dealServiceMock.Object,
			_unitOfWorkMock.Object);

	[Fact]
	public void NewInstallationHubSpotCommandHandler_HubSpotAccountManagerServiceNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new NewInstallationHubSpotCommandHandler(
			null!,
			_authorizationServiceMock.Object,
			_contactServiceMock.Object,
			_clientServiceMock.Object,
			_dealServiceMock.Object,
			_unitOfWorkMock.Object));

		Assert.Contains(
			"hubSpotAccountManagerService",
			result.Message);

		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void NewInstallationHubSpotCommandHandler_HubSpotAuthorizationServiceNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new NewInstallationHubSpotCommandHandler(
			_accountManagerServiceMock.Object,
			null!,
			_contactServiceMock.Object,
			_clientServiceMock.Object,
			_dealServiceMock.Object,
			_unitOfWorkMock.Object));

		Assert.Contains(
			"hubSpotAuthorizationService",
			result.Message);

		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void NewInstallationHubSpotCommandHandler_HubSpotContactServiceNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new NewInstallationHubSpotCommandHandler(
			_accountManagerServiceMock.Object,
			_authorizationServiceMock.Object,
			null!,
			_clientServiceMock.Object,
			_dealServiceMock.Object,
			_unitOfWorkMock.Object));

		Assert.Contains(
			"hubSpotContactService",
			result.Message);

		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void NewInstallationHubSpotCommandHandler_HubSpotClientServiceNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new NewInstallationHubSpotCommandHandler(
			_accountManagerServiceMock.Object,
			_authorizationServiceMock.Object,
			_contactServiceMock.Object,
			null!,
			_dealServiceMock.Object,
			_unitOfWorkMock.Object));

		Assert.Contains(
			"hubSpotClientService",
			result.Message);

		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void NewInstallationHubSpotCommandHandler_HubSpotDealServiceNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new NewInstallationHubSpotCommandHandler(
			_accountManagerServiceMock.Object,
			_authorizationServiceMock.Object,
			_contactServiceMock.Object,
			_clientServiceMock.Object,
			null!,
			_unitOfWorkMock.Object));

		Assert.Contains(
			"hubSpotDealService",
			result.Message);

		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void NewInstallationHubSpotCommandHandler_unitOfWorkNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new NewInstallationHubSpotCommandHandler(
			_accountManagerServiceMock.Object,
			_authorizationServiceMock.Object,
			_contactServiceMock.Object,
			_clientServiceMock.Object,
			_dealServiceMock.Object,
			null!));

		Assert.Contains(
			"unitOfWork",
			result.Message);

		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public async void Handle_AuthorizeUserAsyncReturnsFailure_ReturnsResult()
	{
		// Arrange
		var command = new NewInstallationHubSpotCommand("code");

		var returnResult = Result.Failure<RefreshAccessTokens>(Error.NullValue);

		_authorizationServiceMock
			.Setup(h => h.AuthorizeUserAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(returnResult);

		// Act 
		var result = await _uut.Handle(command, default);

		// Assert
		_authorizationServiceMock.Verify(
			h => h.AuthorizeUserAsync(command.Code, default),
			Times.Once);

		Assert.Equal(returnResult, result);
	}

	[Fact]
	public async void Handle_DecodeAccessTokenAsyncReturnsFailure_ReturnsResult()
	{
		// Arrange
		var command = new NewInstallationHubSpotCommand("code");

		var returnResult = Result.Failure<Supplier>(Error.NullValue);

		_authorizationServiceMock
			.Setup(h => h.AuthorizeUserAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new RefreshAccessTokens() { AccessToken = "Token" });

		_authorizationServiceMock
			.Setup(h => h.DecodeAccessTokenAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(returnResult);

		// Act 
		var result = await _uut.Handle(command, default);

		// Assert
		_authorizationServiceMock.Verify(
			h => h.DecodeAccessTokenAsync("Token", default),
			Times.Once);

		Assert.Equal(returnResult, result);
	}

	[Fact]
	public async void Handle_SupplierAlreadyExists_ReturnsResult()
	{
		// Arrange
		var command = new NewInstallationHubSpotCommand("code");

		_authorizationServiceMock
			.Setup(h => h.AuthorizeUserAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new RefreshAccessTokens() { AccessToken = "Token" });

		_authorizationServiceMock
			.Setup(h => h.DecodeAccessTokenAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Supplier() { Source = Sources.HubSpot, SourceId = 12 });

		_unitOfWorkMock
			.Setup(u => u.SupplierRepository.FindAll())
			.Returns(new List<Supplier>()
			{
				new() { Source = Sources.HubSpot, SourceId = 12 }
			}.AsQueryable());

		// Act 
		var result = await _uut.Handle(command, default);

		// Assert
		_unitOfWorkMock.Verify(
			u => u.SupplierRepository.FindAll(),
			Times.Once);

		Assert.True(result.IsFailure);
		Assert.Equal(Error.AlreadyExists, result.Error);
	}

	[Fact]
	public async void Handle_AccountManagerGetAsyncReturnsFailure_ReturnsResult()
	{
		// Arrange
		var command = new NewInstallationHubSpotCommand("code");

		var returnResult = Result.Failure<List<AccountManager>>(Error.NullValue);

		_authorizationServiceMock
			.Setup(h => h.AuthorizeUserAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new RefreshAccessTokens() { AccessToken = "Token" });

		_authorizationServiceMock
			.Setup(h => h.DecodeAccessTokenAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Supplier() { Source = Sources.HubSpot, SourceId = 12 });

		_unitOfWorkMock
			.Setup(u => u.SupplierRepository.FindAll())
			.Returns(new List<Supplier>().AsQueryable());

		_accountManagerServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(returnResult);

		// Act 
		var result = await _uut.Handle(command, default);

		// Assert
		_accountManagerServiceMock.Verify(
			h => h.GetAsync("Token", default),
			Times.Once);

		Assert.Equal(returnResult, result);
	}

	[Fact]
	public async void Handle_DealGetAsyncReturnsFailure_ReturnsResult()
	{
		// Arrange
		var command = new NewInstallationHubSpotCommand("code");

		var returnResult = Result.Failure<List<Deal>>(Error.NullValue);

		_authorizationServiceMock
			.Setup(h => h.AuthorizeUserAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new RefreshAccessTokens() { AccessToken = "Token" });

		_authorizationServiceMock
			.Setup(h => h.DecodeAccessTokenAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Supplier() { Source = Sources.HubSpot, SourceId = 12 });

		_unitOfWorkMock
			.Setup(u => u.SupplierRepository.FindAll())
			.Returns(new List<Supplier>().AsQueryable());

		_accountManagerServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<AccountManager>());

		_unitOfWorkMock
			.Setup(u => u.AccountManagerRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<AccountManager>>(),
				It.IsAny<CancellationToken>()));

		_dealServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(returnResult);

		// Act 
		var result = await _uut.Handle(command, default);

		// Assert
		_dealServiceMock.Verify(
			h => h.GetAsync("Token", default),
			Times.Once);

		Assert.Equal(returnResult, result);
	}

	[Fact]
	public async void Handle_ContactGetAsyncReturnsFailure_ReturnsResult()
	{
		// Arrange
		var command = new NewInstallationHubSpotCommand("code");

		var returnResult = Result.Failure<List<Contact>>(Error.NullValue);

		_authorizationServiceMock
			.Setup(h => h.AuthorizeUserAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new RefreshAccessTokens() { AccessToken = "Token" });

		_authorizationServiceMock
			.Setup(h => h.DecodeAccessTokenAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Supplier() { Source = Sources.HubSpot, SourceId = 12 });

		_unitOfWorkMock
			.Setup(u => u.SupplierRepository.FindAll())
			.Returns(new List<Supplier>().AsQueryable());

		_accountManagerServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<AccountManager>());

		_unitOfWorkMock
			.Setup(u => u.AccountManagerRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<AccountManager>>(),
				It.IsAny<CancellationToken>()));

		_dealServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<Deal>());

		_unitOfWorkMock
			.Setup(u => u.DealRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<Deal>>(),
				It.IsAny<CancellationToken>()));

		_contactServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(returnResult);

		// Act 
		var result = await _uut.Handle(command, default);

		// Assert
		_contactServiceMock.Verify(
			h => h.GetAsync("Token", default),
			Times.Once);

		Assert.Equal(returnResult, result);
	}

	[Fact]
	public async void Handle_ClientGetAsyncReturnsFailure_ReturnsResult()
	{
		// Arrange
		var command = new NewInstallationHubSpotCommand("code");

		var returnResult = Result.Failure<List<Client>>(Error.NullValue);

		_authorizationServiceMock
			.Setup(h => h.AuthorizeUserAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new RefreshAccessTokens() { AccessToken = "Token" });

		_authorizationServiceMock
			.Setup(h => h.DecodeAccessTokenAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Supplier() { Source = Sources.HubSpot, SourceId = 12 });

		_unitOfWorkMock
			.Setup(u => u.SupplierRepository.FindAll())
			.Returns(new List<Supplier>().AsQueryable());

		_accountManagerServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<AccountManager>());

		_unitOfWorkMock
			.Setup(u => u.AccountManagerRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<AccountManager>>(),
				It.IsAny<CancellationToken>()));

		_dealServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<Deal>());

		_unitOfWorkMock
			.Setup(u => u.DealRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<Deal>>(),
				It.IsAny<CancellationToken>()));

		_contactServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<Contact>());

		_unitOfWorkMock
			.Setup(u => u.ContactRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<Contact>>(),
				It.IsAny<CancellationToken>()));

		_clientServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(returnResult);

		// Act 
		var result = await _uut.Handle(command, default);

		// Assert
		_clientServiceMock.Verify(
			h => h.GetAsync("Token", default),
			Times.Once);

		Assert.Equal(returnResult, result);
	}

	[Fact]
	public async void Handle_AllServicesReturnsSuccess_ReturnsResult()
	{
		// Arrange
		var command = new NewInstallationHubSpotCommand("code");

		_authorizationServiceMock
			.Setup(h => h.AuthorizeUserAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new RefreshAccessTokens() { AccessToken = "Token" });

		_authorizationServiceMock
			.Setup(h => h.DecodeAccessTokenAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Supplier() { Source = Sources.HubSpot, SourceId = 12 });

		_unitOfWorkMock
			.Setup(u => u.SupplierRepository.FindAll())
			.Returns(new List<Supplier>().AsQueryable());

		_accountManagerServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<AccountManager>());

		_unitOfWorkMock
			.Setup(u => u.AccountManagerRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<AccountManager>>(),
				It.IsAny<CancellationToken>()));

		_dealServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<Deal>());

		_unitOfWorkMock
			.Setup(u => u.DealRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<Deal>>(),
				It.IsAny<CancellationToken>()));

		_contactServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<Contact>());

		_unitOfWorkMock
			.Setup(u => u.ContactRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<Contact>>(),
				It.IsAny<CancellationToken>()));
		_clientServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<Client>());

		_unitOfWorkMock
			.Setup(u => u.ClientRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<Client>>(),
				It.IsAny<CancellationToken>()));

		// Act 
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async void Handle_AllServicesReturnsSuccessWithObjects_ReturnsResult()
	{
		// Arrange
		var command = new NewInstallationHubSpotCommand("code");

		_authorizationServiceMock
			.Setup(h => h.AuthorizeUserAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new RefreshAccessTokens() { AccessToken = "Token" });

		_authorizationServiceMock
			.Setup(h => h.DecodeAccessTokenAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Supplier() { Source = Sources.HubSpot, SourceId = 12 });

		_unitOfWorkMock
			.Setup(u => u.SupplierRepository.FindAll())
			.Returns(new List<Supplier>().AsQueryable());

		_accountManagerServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<AccountManager>());

		_unitOfWorkMock
			.Setup(u => u.AccountManagerRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<AccountManager>>(),
				It.IsAny<CancellationToken>()));

		_dealServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<Deal>() { new() });

		_unitOfWorkMock
			.Setup(u => u.DealRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<Deal>>(),
				It.IsAny<CancellationToken>()));

		_contactServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<Contact>() { new() });

		_unitOfWorkMock
			.Setup(u => u.ContactRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<Contact>>(),
				It.IsAny<CancellationToken>()));
		_clientServiceMock
			.Setup(h => h.GetAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new List<Client>() { new() });

		_unitOfWorkMock
			.Setup(u => u.ClientRepository.CreateRangeAsync(
				It.IsAny<IEnumerable<Client>>(),
				It.IsAny<CancellationToken>()));

		// Act 
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsSuccess);
	}
}
