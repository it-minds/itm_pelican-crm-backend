using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.Deals.Commands.UpdateDeal;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Deals.Commands.UpdateDeal;

public class UpdateDealCommandHandlerTests
{
	private const long OBJECTID = 0;
	private const long SUPPLIERHUBSPOTID = 0;
	private const string EMPTY_PROPERTYNAME = "";
	private const string EMPTY_PROPERTYVALUE = "";

	private const string TOKEN = "token";

	private readonly UpdateDealCommandHandler _uut;

	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly Mock<IHubSpotObjectService<Deal>> _hubSpotDealServiceMock;
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock;

	private readonly Mock<IGenericRepository<Deal>> _dealRepositoryMock;
	private readonly Mock<IGenericRepository<Supplier>> _supplierRepositoryMock;
	private readonly Mock<IGenericRepository<AccountManager>> _accountManagerRepositoryMock;
	private readonly Mock<IGenericRepository<Client>> _clientRepositoryMock;
	private readonly Mock<IGenericRepository<Contact>> _contactRepositoryMock;

	public UpdateDealCommandHandlerTests()
	{
		_unitOfWorkMock = new();
		_hubSpotDealServiceMock = new();
		_hubSpotAuthorizationServiceMock = new();

		_dealRepositoryMock = new();
		_supplierRepositoryMock = new();
		_accountManagerRepositoryMock = new();
		_clientRepositoryMock = new();
		_contactRepositoryMock = new();

		_uut = new(
			_unitOfWorkMock.Object,
			_hubSpotDealServiceMock.Object,
			_hubSpotAuthorizationServiceMock.Object);

		_unitOfWorkMock
			.Setup(
				u => u.DealRepository)
			.Returns(_dealRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(
				u => u.SupplierRepository)
			.Returns(_supplierRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(
				u => u.AccountManagerRepository)
			.Returns(_accountManagerRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(
				u => u.ClientRepository)
			.Returns(_clientRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(
				u => u.ContactRepository)
			.Returns(_contactRepositoryMock.Object);
	}

	private void SetupRepositoryMocks(Deal? deal, Supplier? supplier)
	{
		_dealRepositoryMock
			.Setup(x => x
				.FindByCondition(It.IsAny<Expression<Func<Deal, bool>>>()))
			.Returns(deal is null ? Enumerable.Empty<Deal>().AsQueryable() : new List<Deal>() { deal }.AsQueryable());

		_supplierRepositoryMock
			.Setup(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), default))
			.ReturnsAsync(supplier);
	}

	[Fact]
	public void UpdateDealCommandHandler_UnitOfWorkNull_ThrowException()
	{
		/// Act
		Exception exceptionResult = Record.Exception(() =>
			new UpdateDealCommandHandler(
				null!,
				_hubSpotDealServiceMock.Object,
				_hubSpotAuthorizationServiceMock.Object));

		/// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			exceptionResult.GetType());

		Assert.Equal(
			"Value cannot be null. (Parameter 'unitOfWork')",
			exceptionResult.Message);
	}

	[Fact]
	public void UpdateDealCommandHandler_HubSpotDealServiceNull_ThrowException()
	{
		/// Act
		Exception exceptionResult = Record.Exception(() =>
			new UpdateDealCommandHandler(
				_unitOfWorkMock.Object,
				null!,
				_hubSpotAuthorizationServiceMock.Object));

		/// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			exceptionResult.GetType());

		Assert.Equal(
			"Value cannot be null. (Parameter 'hubSpotDealService')",
			exceptionResult.Message);
	}

	[Fact]
	public void UpdateDealCommandHandler_HubSpotAuthorizationServiceNull_ThrowException()
	{
		/// Act
		Exception exceptionResult = Record.Exception(() =>
			new UpdateDealCommandHandler(
				_unitOfWorkMock.Object,
				_hubSpotDealServiceMock.Object,
				null!));

		/// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			exceptionResult.GetType());

		Assert.Equal(
			"Value cannot be null. (Parameter 'hubSpotAuthorizationService')",
			exceptionResult.Message);
	}

	[Fact]
	public async Task Handle_DealNotFoundSupplierNotFound_ReturnsFailureErrorNullValue()
	{
		// Arrange
		UpdateDealCommand command = new(OBJECTID, SUPPLIERHUBSPOTID, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

		SetupRepositoryMocks(null, null);

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_dealRepositoryMock.Verify(
			x => x.FindByCondition(d => d.HubSpotId == command.ObjectId.ToString()),
			Times.Once);

		_supplierRepositoryMock.Verify(
			x => x.FirstOrDefaultAsync(supplier => supplier.HubSpotId == SUPPLIERHUBSPOTID, default),
			Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Fact]
	public async void Handle_DealNotFoundFailsRefreshingToken_ReturnsFailure()
	{
		// Arrange
		UpdateDealCommand command = new(OBJECTID, SUPPLIERHUBSPOTID, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

		Supplier existingSupplier = new(Guid.NewGuid())
		{
			RefreshToken = TOKEN,
		};

		Error error = new("0", "error");

		SetupRepositoryMocks(null, existingSupplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), default))
			.ReturnsAsync(Result.Failure<string>(error));

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_hubSpotAuthorizationServiceMock.Verify(
			service => service.RefreshAccessTokenAsync(TOKEN, default),
			Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equal(error, result.Error);
	}

	[Fact]
	public async void Handle_DealNotFoundFailedFetchingDealFromHubSpot_ReturnsFailure()
	{
		// Arrange
		UpdateDealCommand command = new(OBJECTID, SUPPLIERHUBSPOTID, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

		Supplier existingSupplier = new(Guid.NewGuid())
		{
			RefreshToken = TOKEN,
		};

		SetupRepositoryMocks(null, existingSupplier);

		Error error = new("0", "error");

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), default))
			.ReturnsAsync(Result.Success(TOKEN));

		_hubSpotDealServiceMock
			.Setup(service => service
				.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), default))
			.ReturnsAsync(Result.Failure<Deal>(error));

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_hubSpotDealServiceMock.Verify(
			service => service.GetByIdAsync(TOKEN, command.ObjectId, default),
			Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equal(error, result.Error);
	}

	[Fact]
	public async void Handle_DealNotFoundSuccessFetchingFromHubSpot_NewDealSavedReturnsSuccess()
	{
		// Arrange
		UpdateDealCommand command = new(OBJECTID, SUPPLIERHUBSPOTID, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

		Supplier existingSupplier = new(Guid.NewGuid())
		{
			RefreshToken = TOKEN,
		};

		SetupRepositoryMocks(null, existingSupplier);

		Mock<Deal> newDealMock = new();
		newDealMock.Object.HubSpotId = "hubspotId";
		newDealMock.Object.HubSpotOwnerId = "ownerId";

		newDealMock
			.Setup(d => d.FillOutAssociations(It.IsAny<AccountManager>(), It.IsAny<Client>(), It.IsAny<List<Contact>>()))
			.Returns(newDealMock.Object);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), default))
			.ReturnsAsync(Result.Success(TOKEN));

		_hubSpotDealServiceMock
			.Setup(service => service
				.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), default))
			.ReturnsAsync(Result.Success(newDealMock.Object));

		_accountManagerRepositoryMock
			.Setup(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<AccountManager, bool>>>(), default))
			.ReturnsAsync((AccountManager?)null);

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_dealRepositoryMock.Verify(
			x => x.CreateAsync(newDealMock.Object, default),
			Times.Once);

		_unitOfWorkMock.Verify(
			x => x.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);

		Assert.Equal(result.Error, Error.None);
	}

	[Fact]
	public async void Handle_DealNotFoundSuccessFetchingFromHubSpotWithAssociationsNotExistingInDb_ReturnsSuccess()
	{
		// Arrange
		UpdateDealCommand command = new(OBJECTID, SUPPLIERHUBSPOTID, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

		Supplier existingSupplier = new(Guid.NewGuid())
		{
			RefreshToken = TOKEN,
		};
		Contact existingContact = new(Guid.NewGuid())
		{
			HubSpotId = "contactHubSpotId",
		};

		Client existingClient = new(Guid.NewGuid())
		{
			HubSpotId = "clientHubSpotId",
		};

		SetupRepositoryMocks(null, existingSupplier);

		Mock<Deal> newDealMock = new();
		newDealMock.Object.HubSpotId = "hubspotId";
		newDealMock.Object.HubSpotOwnerId = "ownerId";
		newDealMock.Object.Client = existingClient;
		newDealMock.Object.DealContacts = new List<DealContact>()
		{
			new DealContact(Guid.NewGuid())
			{
				Deal = newDealMock.Object,
				DealId = newDealMock.Object.Id,
				HubSpotDealId = newDealMock.Object.HubSpotId,
				HubSpotContactId = existingContact.HubSpotId,
			}
		};

		newDealMock
			.Setup(d => d.FillOutAssociations(It.IsAny<AccountManager>(), It.IsAny<Client>(), It.IsAny<List<Contact>>()))
			.Returns(newDealMock.Object);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), default))
			.ReturnsAsync(Result.Success(TOKEN));

		_hubSpotDealServiceMock
			.Setup(service => service
				.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), default))
			.ReturnsAsync(Result.Success(newDealMock.Object));

		_accountManagerRepositoryMock
			.Setup(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<AccountManager, bool>>>(), default))
			.ReturnsAsync((AccountManager?)null);

		_clientRepositoryMock
			.Setup(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Client, bool>>>(), default))
			.ReturnsAsync((Client)null!);

		_contactRepositoryMock
			.Setup(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Contact, bool>>>(), default))
			.ReturnsAsync((Contact)null!);

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_dealRepositoryMock.Verify(
			x => x.CreateAsync(newDealMock.Object, default),
			Times.Once);

		_unitOfWorkMock.Verify(
			x => x.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);

		Assert.Equal(result.Error, Error.None);
	}

	[Fact]
	public async void Handle_DealNotFoundSuccessFetchingFromHubSpotWithAssociations_ReturnsSuccess()
	{
		// Arrange
		UpdateDealCommand command = new(OBJECTID, SUPPLIERHUBSPOTID, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

		Deal? existingDeal = null;

		Supplier existingSupplier = new(Guid.NewGuid())
		{
			RefreshToken = TOKEN,
		};
		Contact existingContact = new(Guid.NewGuid())
		{
			HubSpotId = "contactHubSpotId",
		};

		Client existingClient = new(Guid.NewGuid())
		{
			HubSpotId = "clientHubSpotId",
		};

		SetupRepositoryMocks(existingDeal, existingSupplier);

		Mock<Deal> newDealMock = new();
		newDealMock.Object.HubSpotId = "hubspotId";
		newDealMock.Object.HubSpotOwnerId = "ownerId";
		newDealMock.Object.Client = existingClient;
		newDealMock.Object.DealContacts = new List<DealContact>()
		{
			new DealContact(Guid.NewGuid())
			{
				Deal = newDealMock.Object,
				DealId = newDealMock.Object.Id,
				HubSpotDealId = newDealMock.Object.HubSpotId,
				HubSpotContactId = existingContact.HubSpotId,
			}
		};

		newDealMock
			.Setup(d => d.FillOutAssociations(It.IsAny<AccountManager>(), It.IsAny<Client>(), It.IsAny<List<Contact>>()))
			.Returns(newDealMock.Object);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), default))
			.ReturnsAsync(Result.Success(TOKEN));

		_hubSpotDealServiceMock
			.Setup(service => service
				.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), default))
			.ReturnsAsync(Result.Success(newDealMock.Object));

		_accountManagerRepositoryMock
			.Setup(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<AccountManager, bool>>>(), default))
			.ReturnsAsync((AccountManager?)null);

		_clientRepositoryMock
			.Setup(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Client, bool>>>(), default))
			.ReturnsAsync(existingClient);

		_contactRepositoryMock
			.Setup(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Contact, bool>>>(), default))
			.ReturnsAsync(existingContact);

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_dealRepositoryMock.Verify(
			x => x.CreateAsync(newDealMock.Object, default),
			Times.Once);

		_unitOfWorkMock.Verify(
			x => x.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);

		Assert.Equal(result.Error, Error.None);
	}

	[Fact]
	public async void Handle_DealFoundAccountManagerAssociationUpdated_ReturnsSuccess()
	{
		// Arrange
		Mock<Deal> dealMock = new(Guid.NewGuid());

		SetupRepositoryMocks(dealMock.Object, null);

		UpdateDealCommand command = new(1, 1, "hs_all_owner_ids", EMPTY_PROPERTYVALUE);

		_accountManagerRepositoryMock
			.Setup(a => a
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<AccountManager, bool>>>(), default))
			.ReturnsAsync(new AccountManager(Guid.NewGuid()));

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_dealRepositoryMock.Verify(
			x => x.FindByCondition(
				d => d.HubSpotId == command.ObjectId.ToString()),
			Times.Once);

		_dealRepositoryMock.Verify(
			x => x.Update(dealMock.Object),
			Times.Once);

		_unitOfWorkMock.Verify(
			x => x.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}

	[Fact]
	public async void Handle_DealFoundPropertyUpdated_ReturnsSuccess()
	{
		// Arrange
		UpdateDealCommand command = new(OBJECTID, SUPPLIERHUBSPOTID, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

		Mock<Deal> dealMock = new();

		dealMock
			.Setup(d => d.UpdateProperty(It.IsAny<string>(), It.IsAny<string>()))
			.Returns(dealMock.Object);

		SetupRepositoryMocks(dealMock.Object, null);

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_dealRepositoryMock.Verify(
			x => x.FindByCondition(
				d => d.HubSpotId == command.ObjectId.ToString()),
			Times.Once);

		_dealRepositoryMock.Verify(
			x => x.Update(dealMock.Object),
			Times.Once);

		_unitOfWorkMock.Verify(
			x => x.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}
}
