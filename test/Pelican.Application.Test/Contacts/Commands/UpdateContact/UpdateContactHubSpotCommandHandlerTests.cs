using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Contacts.HubSpotCommands.Update;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Contacts.HubSpotCommands.Update;
public class UpdateContactHubSpotCommandHandlerTests
{
	private readonly UpdateContactHubSpotCommandHandler _uut;

	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IHubSpotObjectService<Contact>> _hubSpotContactServiceMock = new();
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock = new();

	private readonly Mock<IGenericRepository<Contact>> _contactRepositoryMock = new();
	private readonly Mock<IGenericRepository<Deal>> _dealRepositoryMock = new();
	private readonly Mock<IGenericRepository<Client>> _clientRepositoryMock = new();

	private const long OBJECT_ID = 123;
	private const long SUPPLIER_HUBSPOT_ID = 456;
	private const long UPDATE_TIME = 1;
	private const string NAME = "name";
	private const string VALUE = "value";

	public UpdateContactHubSpotCommandHandlerTests()
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
	public void UpdateContactHubSpotCommandHandler_UnitOfWorkNull_ThrowsArgumentNullException()
	{
		// Act
		var result = Record.Exception(() => new UpdateContactHubSpotCommandHandler(
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
	public void UpdateContactHubSpotCommandHandler_HubSpotContactServiceNull_ThrowsArgumentNullException()
	{
		// Act
		var result = Record.Exception(() => new UpdateContactHubSpotCommandHandler(
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
	public void UpdateContactHubSpotCommandHandler_HubSpotAuthorizationServiceNull_ThrowsArgumentNullException()
	{
		// Act
		var result = Record.Exception(() => new UpdateContactHubSpotCommandHandler(
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
	public async void Handle_ContactNotFoundAccessTokenNotFound_ContactRepositoryCalledReturnsFailure()
	{
		// Arrange
		UpdateContactHubSpotCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, UPDATE_TIME, NAME, VALUE);

		_contactRepositoryMock
			.Setup(repo => repo
				.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(Enumerable.Empty<Contact>().AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(h => h.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<string>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		_contactRepositoryMock.Verify(
			repo => repo.FindByCondition(
				contact => contact.SourceId == command.ObjectId.ToString() && contact.Source == Sources.HubSpot),
			Times.Once);

		_hubSpotAuthorizationServiceMock
			.Verify(h => h
				.RefreshAccessTokenFromSupplierHubSpotIdAsync(SUPPLIER_HUBSPOT_ID, default), Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equal(
			Error.NullValue,
			result.Error);
	}

	[Fact]
	public async void Handle_ContactNotFoundFailedFetchingContactFromHubSpot_HubSpotContactServiceCalledReturnsFailure()
	{
		// Arrange
		UpdateContactHubSpotCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, UPDATE_TIME, NAME, VALUE);

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(Enumerable.Empty<Contact>().AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
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
		UpdateContactHubSpotCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, UPDATE_TIME, NAME, VALUE);

		Mock<Contact> newContactMock = new();

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(Enumerable.Empty<Contact>().AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
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
			repo => repo.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()),
			Times.Never);

		_contactRepositoryMock.Verify(
			repo => repo.Attach(
				newContactMock.Object),
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
		UpdateContactHubSpotCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, UPDATE_TIME, NAME, VALUE);

		DealContact existingDealContact = new();

		Mock<Contact> newContactMock = new();
		newContactMock.Object.DealContacts.Add(existingDealContact);

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(Enumerable.Empty<Contact>().AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
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
		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async void Handle_ContactNotFoundFetchingContactWithClientContacts_ClientLoadedFromRepositoriesReturnsSuccess()
	{
		// Arrange
		UpdateContactHubSpotCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, UPDATE_TIME, NAME, VALUE);

		ClientContact existingClientContact = new();

		Mock<Contact> newContactMock = new();
		newContactMock.Object.ClientContacts.Add(existingClientContact);

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(Enumerable.Empty<Contact>().AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
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
		Assert.True(result.IsSuccess);
	}

	[Fact]
	public async void Handle_ContactNotFoundFetchingContactWithDealContacts_UnitOfWorkCreateAndSaveCalledReturnsSuccess()
	{
		// Arrange
		UpdateContactHubSpotCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, UPDATE_TIME, NAME, VALUE);

		Deal existingDeal = new()
		{
			SourceId = "dealHubSpotId",
		};

		Mock<Contact> newContactMock = new();
		newContactMock.Object.SourceId = "hubSpotId";

		newContactMock.Object.DealContacts.Add(new()
		{
			Contact = newContactMock.Object,
			ContactId = newContactMock.Object.Id,
			SourceContactId = newContactMock.Object.SourceId,
			SourceDealId = existingDeal.SourceId,
		});

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(Enumerable.Empty<Contact>().AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
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
		_contactRepositoryMock.Verify(
			repo => repo.Attach(
				newContactMock.Object),
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
		UpdateContactHubSpotCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, UPDATE_TIME, NAME, VALUE);

		Client existingClient = new()
		{
			SourceId = "clientHubSpotId",
		};

		Mock<Contact> newContactMock = new();
		newContactMock.Object.SourceId = "hubSpotId";

		newContactMock.Object.ClientContacts.Add(new()
		{
			Contact = newContactMock.Object,
			ContactId = newContactMock.Object.Id,
			SourceContactId = newContactMock.Object.SourceId,
			SourceClientId = existingClient.SourceId,
		});

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(Enumerable.Empty<Contact>().AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
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
		_contactRepositoryMock.Verify(
			repo => repo.Attach(newContactMock.Object),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);
	}

	[Fact]
	public async void Handle_ContactFoundFirstnameUpdated_DependenciesCalledReturnSuccess()
	{
		// Arrange
		UpdateContactHubSpotCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, UPDATE_TIME, NAME, VALUE);

		Mock<Contact> contactMock = new();

		_contactRepositoryMock
			.Setup(repo => repo.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(new List<Contact>() { contactMock.Object }.AsQueryable());

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		contactMock.Verify(
			c => c.UpdateProperty(NAME, VALUE),
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
		UpdateContactHubSpotCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, UPDATE_TIME, "num_associated_deals", VALUE);

		string accessToken = "accessToken";

		Mock<Contact> contactMock = new();

		_contactRepositoryMock
			.Setup(repo => repo.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(new List<Contact>() { contactMock.Object }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
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
		UpdateContactHubSpotCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, UPDATE_TIME, "num_associated_deals", VALUE);

		string accessToken = "accessToken";

		Mock<Contact> contactMock = new();

		List<DealContact> dealContactsFromHubSpot = new() { new() };
		Contact contactFromHubSpot = new()
		{
			DealContacts = dealContactsFromHubSpot,
		};

		_contactRepositoryMock
			.Setup(repo => repo.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(new List<Contact>() { contactMock.Object }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
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


	[Theory]
	[InlineData(0, 0, 1, "testProperty", "testValue")]
	public async void Handle_ContactFoundLastCommandUpdateTimeOlderThanLastUpdateAndOlderThanCreateAtAuthServiceReturnsFailure_ReturnsFailure(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateContactHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Mock<Contact> contactMock = new();

		contactMock.Object.LastUpdatedAt = 10;
		contactMock.Object.CreatedAt = 5;

		_contactRepositoryMock
			.Setup(repo => repo.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(new List<Contact>() { contactMock.Object }.AsQueryable());

		_hubSpotAuthorizationServiceMock.Setup(
			h => h.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<string>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, default);

		//Assert
		_hubSpotAuthorizationServiceMock.Verify(
			c => c.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				portalId,
				default),
			Times.Once);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, 1, "testProperty", "testValue")]
	public async void Handle_ContactFoundLastCommandUpdateTimeOlderThanLastUpdateAndOlderThanCreateAtContactServiceReturnsFailure_ReturnsFailure(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateContactHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Mock<Contact> contactMock = new();

		contactMock.Object.LastUpdatedAt = 10;
		contactMock.Object.CreatedAt = 5;

		string accessToken = "accessTokent";

		_contactRepositoryMock
			.Setup(repo => repo.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(new List<Contact>() { contactMock.Object }.AsQueryable());

		_hubSpotAuthorizationServiceMock.Setup(
			h => h.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(accessToken));

		_hubSpotContactServiceMock
			.Setup(h => h.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Contact>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, default);

		//Assert
		_hubSpotContactServiceMock.Verify(
			c => c.GetByIdAsync(
				accessToken,
				command.ObjectId,
				default),
			Times.Once);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, 1, "testProperty", "testValue", "testFirstName", "testLatName", "testEmail", "testJobTitle", "testPhoneNumber", "testSourceOwnerId")]
	public async void Handle_ContactFoundLastCommandUpdateTimeOlderThanLastUpdateAndOlderThanCreateAtContactServiceReturnsSuccessClientUpdated_ReturnsSuccess(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue,
		string testFirstName,
		string testLastName,
		string testEmail,
		string testJobTitle,
		string testPhoneNumber,
		string testSourceOwnerId)
	{
		//Arrange
		UpdateContactHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Mock<Contact> contactMock = new();

		contactMock.Object.LastUpdatedAt = 10;
		contactMock.Object.CreatedAt = 5;

		string accessToken = "accessToken";

		Contact contactResult = new()
		{
			FirstName = testFirstName,
			LastName = testLastName,
			Email = testEmail,
			JobTitle = testJobTitle,
			PhoneNumber = testPhoneNumber,
			SourceOwnerId = testSourceOwnerId,
			DealContacts = new List<DealContact>()
			{
				new DealContact()
			},
		};

		_contactRepositoryMock
			.Setup(repo => repo.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(new List<Contact>() { contactMock.Object }.AsQueryable());

		_hubSpotAuthorizationServiceMock.Setup(
			h => h.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(accessToken));

		_hubSpotContactServiceMock
			.Setup(h => h.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(contactResult));

		// Act
		var result = await _uut.Handle(command, default);

		//Assert
		contactMock.Verify(
			x => x.UpdatePropertiesFromContact(contactResult),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);
	}
}
