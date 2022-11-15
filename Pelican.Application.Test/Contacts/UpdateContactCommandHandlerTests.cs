using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.Contacts.Commands.UpdateContact;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Contacts;
public class UpdateContactCommandHandlerTests
{
	private readonly UpdateContactCommandHandler _uut;

	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IHubSpotObjectService<Contact>> _hubSpotContactServiceMock = new();
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock = new();

	private readonly Mock<IGenericRepository<Contact>> _contactRepositoryMock = new();
	private readonly Mock<IGenericRepository<Supplier>> _supplierRepositoryMock = new();
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
			.Setup(unitOfWork => unitOfWork.SupplierRepository)
			.Returns(_supplierRepositoryMock.Object);

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
	}

	[Fact]
	public async void Handle_ClientNotFoundSupplierNotFound_ContactAndSupplierRepositoriesCalled()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Supplier)null!);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		_contactRepositoryMock.Verify(
			repo => repo.FirstOrDefaultAsync(
				contact => contact.HubSpotId == command.ObjectId.ToString(),
				default),
			Times.Once);

		_supplierRepositoryMock.Verify(
			repo => repo.FirstOrDefaultAsync(
				supplier => supplier.HubSpotId == SUPPLIER_HUBSPOT_ID,
				default),
			Times.Once);
	}

	[Fact]
	public async void Handle_ClientNotFoundSupplierNotFound_ReturnsFailure()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Supplier)null!);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(
			Error.NullValue,
			result.Error);
	}

	[Fact]
	public async void Handle_ClientNotFoundRefreshTokenEmpty_ReturnsFailure()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		Supplier supplier = new(Guid.NewGuid());

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(
			Error.NullValue,
			result.Error);
	}

	[Fact]
	public async void Handle_ClientNotFoundFailedRefreshingAccessToken_HubSpotAuthServiceCalled()
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

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<string>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		_hubSpotAuthorizationServiceMock.Verify(
			service => service.RefreshAccessTokenAsync(
				supplier.RefreshToken,
				default),
			Times.Once);
	}

	[Fact]
	public async void Handle_ClientNotFoundFailedRefreshingAccessToken_ReturnsFailure()
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

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<string>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<string>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsFailure);

		Assert.Equal(
			Error.NullValue,
			result.Error);
	}
	[Fact]
	public async void Handle_ClientNotFoundFailedFetchingContactFromHubSpot_HubSpotContactServiceCalled()
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

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<string>(),
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
	}

	[Fact]
	public async void Handle_ClientNotFoundFailedFetchingContactFromHubSpot_ReturnsFailure()
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

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<string>(),
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
	}

	[Fact]
	public async void Handle_ClientNotFoundFetchingContactWithoutAssociations_FillOutAssociationCalled()
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

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<string>(),
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
	}

	[Fact]
	public async void Handle_ClientNotFoundFetchingContactWithoutAssociations_DealAndClientRepositoriesNotCalled()
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

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<string>(),
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
				It.IsAny<Expression<Func<Deal, bool>>>(),
				It.IsAny<CancellationToken>()),
			Times.Never);

		_clientRepositoryMock.Verify(
			repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Client, bool>>>(),
				It.IsAny<CancellationToken>()),
			Times.Never);
	}

	[Fact]
	public async void Handle_ClientNotFoundFetchingContactWithoutAssociations_UnitOfWorkCreateAndSaveCalled()
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

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<string>(),
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
			repo => repo.CreateAsync(
				newContactMock.Object,
				default),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);
	}

	[Fact]
	public async void Handle_ClientNotFoundFetchingContactWithoutAssociations_ReturnsSuccess()
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

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<string>(),
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
	public async void Handle_ClientNotFoundFetchingContactWithAssociations_DealsAndClientLoadedFromRepositories()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "not_empty"
		};

		DealContact existingDealContact = new(Guid.NewGuid());
		ClientContact existingClientContact = new(Guid.NewGuid());

		Mock<Contact> newContactMock = new();
		newContactMock.Object.DealContacts.Add(existingDealContact);
		newContactMock.Object.ClientContacts.Add(existingClientContact);

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<string>(),
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

		_clientRepositoryMock.Verify(
			repo => repo.FirstOrDefaultAsync(
				c => c.HubSpotId == existingClientContact.Client.HubSpotId,
				default),
			Times.Once);
	}

	[Fact]
	public async void Handle_ClientNotFoundFetchingContactWithAssociations_UnitOfWorkCreateAndSaveCalled()
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
		Client existingClient = new(Guid.NewGuid())
		{
			HubSpotId = "clientHubSpotId",
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

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<string>(),
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
	public async void Handle_ClientNotFoundFetchingContactWithAssociations_ReturnsSuccess()
	{
		// Arrange
		UpdateContactCommand command = new(OBJECT_ID, SUPPLIER_HUBSPOT_ID, NAME, VALUE);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "not_empty"
		};


		DealContact existingDealContact = new(Guid.NewGuid());
		ClientContact existingClientContact = new(Guid.NewGuid());

		Mock<Contact> newContactMock = new();
		newContactMock.Object.DealContacts.Add(existingDealContact);
		newContactMock.Object.ClientContacts.Add(existingClientContact);

		string accessToken = "accessToken";

		_contactRepositoryMock
			.Setup(repo => repo.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Contact, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_supplierRepositoryMock
			.Setup(x => x.FirstOrDefaultAsync(
				It.IsAny<Expression<Func<Supplier, bool>>>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(supplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(
				It.IsAny<string>(),
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






}
