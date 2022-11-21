using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.Contacts.Commands.UpdateContact;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Contacts.Commands.UpdateContact;
public class UpdateContactCommandHandlerTests
{
	private readonly UpdateContactCommandHandler _uut;

	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IHubSpotObjectService<Contact>> _hubSpotContactServiceMock = new();
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock = new();

	private readonly Mock<IGenericRepository<Contact>> _contactRepositoryMock = new();
	private readonly Mock<IGenericRepository<Deal>> _dealRepositoryMock = new();
	private readonly Mock<IGenericRepository<Client>> _clientRepositoryMock = new();

	private const long OBJECT_ID = 123;
	private const long SUPPLIER_HUBSPOT_ID = 456;
	private const string NAME = "name";
	private const string VALUE = "value";

	public UpdateContactCommandHandlerTests()
	{
		_uut = new(
			_unitOfWorkMock.Object,
			_hubSpotContactServiceMock.Object,
			_hubSpotAuthorizationServiceMock.Object);

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ContactRepository)
			.Returns(_contactRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.DealRepository)
			.Returns(_dealRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository)
			.Returns(_clientRepositoryMock.Object);
	}

	[Fact]
	public void UpdateContactCommandHandler_UnitOfWorkNull_ThrowsArgumentNullException()
	{
		// Act
		var result = Record.Exception(() => new UpdateContactCommandHandler(
			null!,
			_hubSpotContactServiceMock.Object,
			_hubSpotAuthorizationServiceMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"unitOfWork",
			result.Message);
	}

	[Fact]
	public void UpdateContactCommandHandler_HubSpotContactServiceNull_ThrowsArgumentNullException()
	{
		// Act
		var result = Record.Exception(() => new UpdateContactCommandHandler(
			_unitOfWorkMock.Object,
			null!,
			_hubSpotAuthorizationServiceMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"hubSpotContactService",
			result.Message);
	}

	[Fact]
	public void UpdateContactCommandHandler_HubSpotAuthorizationServiceNull_ThrowsArgumentNullException()
	{
		// Act
		var result = Record.Exception(() => new UpdateContactCommandHandler(
			_unitOfWorkMock.Object,
			_hubSpotContactServiceMock.Object,
			null!));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"hubSpotAuthorizationService",
			result.Message);
	}

	[Fact]
	public async void Handle_ContactNotFoundSupplierNotFound_ContactAndSupplierRepositoriesCalledReturnsFailure()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_hubSpotAuthorizationServiceMock
			.Setup(h => h.RefreshAccessTokenAsync(
				It.IsAny<long>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<string>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		_contactRepositoryMock.Verify(
			repo => repo.FirstOrDefaultAsync(
				contact => contact.HubSpotId == command.ObjectId.ToString(),
				default),
			Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equal(
			Error.NullValue,
			result.Error);
	}

	[Fact]
	public async void Handle_ContactNotFoundFailedRefreshingAccessToken_HubSpotAuthServiceCalledReturnsFailure()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "not_empty"
		};

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<long>(),
				_unitOfWorkMock.Object,
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<string>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		_hubSpotAuthorizationServiceMock.Verify(
			service => service.RefreshAccessTokenAsync(
				SUPPLIER_HUBSPOT_ID,
				_unitOfWorkMock.Object,
				default),
			Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equal(
			Error.NullValue,
			result.Error);
	}

	[Fact]
	public async void Handle_ContactNotFoundFailedFetchingContactFromHubSpot_HubSpotContactServiceCalledReturnsFailure()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "not_empty"
		};

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<long>(),
				_unitOfWorkMock.Object,
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(accessToken));

		_hubSpotContactServiceMock
			.Setup(service => service.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Contact>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		_hubSpotContactServiceMock.Verify(
			service => service.GetByIdAsync(
				accessToken,
				command.ObjectId,
				default),
			Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equal(
			Error.NullValue,
			result.Error);
	}

	[Fact]
	public async void Handle_ContactNotFoundFetchingContactWithoutAssociations_DependencyCallsAssertedReturnsSuccess()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "not_empty"
		};

		Mock<Contact> newContactMock = new();

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<long>(),
				_unitOfWorkMock.Object,
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(accessToken));

		_hubSpotContactServiceMock
			.Setup(service => service.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(newContactMock.Object));

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		newContactMock.Verify(
			contact => contact.FillOutAssociations(
				Enumerable.Empty<Client>(),
				Enumerable.Empty<Deal>()),
			Times.Once);

		_dealRepositoryMock.Verify(
			repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Deal, bool>>>(),
				It.IsAny<CancellationToken>()),
			Times.Never);

		_clientRepositoryMock.Verify(
			repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Client, bool>>>(),
				It.IsAny<CancellationToken>()),
			Times.Never);

		_contactRepositoryMock.Verify(
			repo => repo.CreateAsync(
				newContactMock.Object,
				default),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async void Handle_ContactNotFoundFetchingContactWithDealContacts_DealsLoadedFromRepositoriesReturnsSuccess()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "not_empty"
		};

		DealContact existingDealContact = new(Guid.NewGuid());

		Mock<Contact> newContactMock = new();
		newContactMock.Object.DealContacts.Add(existingDealContact);

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<long>(),
				_unitOfWorkMock.Object,
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(accessToken));

		_hubSpotContactServiceMock
			.Setup(service => service.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(newContactMock.Object));

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		_dealRepositoryMock.Verify(
			repo => repo.FirstOrDefaultAsync(
				d => d.HubSpotId == existingDealContact.Deal.HubSpotId,
				default),
			Times.Once);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async void Handle_ContactNotFoundFetchingContactWithClientContacts_ClientLoadedFromRepositoriesReturnsSuccess()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "not_empty"
		};

		ClientContact existingClientContact = new(Guid.NewGuid());

		Mock<Contact> newContactMock = new();
		newContactMock.Object.ClientContacts.Add(existingClientContact);

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<long>(),
				_unitOfWorkMock.Object,
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(accessToken));

		_hubSpotContactServiceMock
			.Setup(service => service.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(newContactMock.Object));

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		_clientRepositoryMock.Verify(
			repo => repo.FirstOrDefaultAsync(
				c => c.HubSpotId == existingClientContact.Client.HubSpotId,
				default),
			Times.Once);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async void Handle_ContactNotFoundFetchingContactWithDealContacts_UnitOfWorkCreateAndSaveCalledReturnsSuccess()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "not_empty"
		};

		Deal existingDeal = new(Guid.NewGuid())
		{
			HubSpotId = "dealHubSpotId",
		};

		Mock<Contact> newContactMock = new();
		newContactMock.Object.HubSpotId = "hubSpotId";

		newContactMock.Object.DealContacts.Add(new()
		{
			Contact = newContactMock.Object,
			ContactId = newContactMock.Object.Id,
			HubSpotContactId = newContactMock.Object.HubSpotId,
			HubSpotDealId = existingDeal.HubSpotId,
		});

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<long>(),
				_unitOfWorkMock.Object,
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(accessToken));

		_hubSpotContactServiceMock
			.Setup(service => service.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(newContactMock.Object));

		_dealRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Deal, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(existingDeal);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		_contactRepositoryMock.Verify(
			repo => repo.CreateAsync(
				newContactMock.Object,
				default),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async void Handle_ContactNotFoundFetchingContactWithClientContacts_UnitOfWorkCreateAndSaveCalled()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "not_empty"
		};

		Client existingClient = new(Guid.NewGuid())
		{
			HubSpotId = "clientHubSpotId",
		};

		Mock<Contact> newContactMock = new();
		newContactMock.Object.HubSpotId = "hubSpotId";

		newContactMock.Object.ClientContacts.Add(new()
		{
			Contact = newContactMock.Object,
			ContactId = newContactMock.Object.Id,
			HubSpotContactId = newContactMock.Object.HubSpotId,
			HubSpotClientId = existingClient.HubSpotId,
		});

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<long>(),
				_unitOfWorkMock.Object,
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(accessToken));

		_hubSpotContactServiceMock
			.Setup(service => service.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(newContactMock.Object));

		_clientRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Client, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(existingClient);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		_contactRepositoryMock.Verify(
			repo => repo.CreateAsync(
				newContactMock.Object,
				default),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);
	}

	[Fact]
	public async void Handle_ContactFoundFirstnameUpdated_DependenciesCalledReturnSuccess()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		Mock<Contact> contactMock = new();

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(contactMock.Object);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		contactMock.Verify(
			c => c.UpdateProperty(NAME, VALUE),
			Times.Once);

		_contactRepositoryMock.Verify(
			repo => repo.Update(contactMock.Object),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async void Handle_ContactFoundDealAssociationsUpdatedFailedFetchingFromHubSpot_ReturnsFailureUpdateDealContactsNotCalled()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, "num_associated_deals", VALUE);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "not_empty"
		};

		string accessToken = "accessToken";

		Mock<Contact> contactMock = new();

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(contactMock.Object);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<long>(),
				_unitOfWorkMock.Object,
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(accessToken));

		_hubSpotContactServiceMock
			.Setup(service => service.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Contact>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(
			Error.NullValue,
			result.Error);

		contactMock.Verify(
			c => c.UpdateDealContacts(It.IsAny<List<DealContact>>()),
			Times.Never);
	}

	[Fact]
	public async void Handle_ContactFoundDealAssociationsUpdatedSuccessFetchingFromHubSpot_UpdateDealContactsCalledReturnSuccess()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, "num_associated_deals", VALUE);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "not_empty"
		};

		string accessToken = "accessToken";

		Mock<Contact> contactMock = new();

		List<DealContact> dealContactsFromHubSpot = new List<DealContact>() { new() };
		Contact contactFromHubSpot = new()
		{
			DealContacts = dealContactsFromHubSpot,
		};

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(contactMock.Object);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<long>(),
				_unitOfWorkMock.Object,
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(accessToken));

		_hubSpotContactServiceMock
			.Setup(service => service.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(contactFromHubSpot));

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		contactMock.Verify(
			c => c.UpdateDealContacts(dealContactsFromHubSpot),
			Times.Once);

		Assert.True(result.IsSuccess);
	}
}
