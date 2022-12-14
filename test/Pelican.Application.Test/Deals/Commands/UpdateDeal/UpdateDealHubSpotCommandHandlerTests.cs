using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Deals.HubSpotCommands.Update;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Deals.Commands.UpdateDeal;

public class UpdateDealHubSpotCommandHandlerTests
{
	private const long OBJECTID = 0;
	private const long UPDATETIME = 0;
	private const long SUPPLIERHUBSPOTID = 0;
	private const string EMPTY_PROPERTYNAME = "";
	private const string EMPTY_PROPERTYVALUE = "";

	private const string TOKEN = "token";

	private readonly UpdateDealHubSpotCommandHandler _uut;

	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IHubSpotObjectService<Deal>> _hubSpotDealServiceMock = new();
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock = new();

	private readonly Mock<IGenericRepository<Deal>> _dealRepositoryMock = new();
	private readonly Mock<IGenericRepository<AccountManager>> _accountManagerRepositoryMock = new();
	private readonly Mock<IGenericRepository<Client>> _clientRepositoryMock = new();
	private readonly Mock<IGenericRepository<Contact>> _contactRepositoryMock = new();
	private readonly Mock<IGenericRepository<AccountManagerDeal>> _accountManagerDealRepositoryMock = new();

	public UpdateDealHubSpotCommandHandlerTests()
	{
		_uut = new(
			_unitOfWorkMock.Object,
			_hubSpotDealServiceMock.Object,
			_hubSpotAuthorizationServiceMock.Object);

		_unitOfWorkMock
			.Setup(u => u.DealRepository)
			.Returns(_dealRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(u => u.AccountManagerRepository)
			.Returns(_accountManagerRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(u => u.ClientRepository)
			.Returns(_clientRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(u => u.ContactRepository)
			.Returns(_contactRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(u => u.AccountManagerDealRepository)
			.Returns(_accountManagerDealRepositoryMock.Object);
	}

	private void SetupDealRepositoryMock(Deal? deal)
	{
		_dealRepositoryMock
			.Setup(x => x
				.FindByCondition(It.IsAny<Expression<Func<Deal, bool>>>()))
			.Returns(deal is null ? Enumerable.Empty<Deal>().AsQueryable() : new List<Deal>() { deal }.AsQueryable());
	}

	[Fact]
	public void UpdateDealCommandHandler_UnitOfWorkNull_ThrowException()
	{
		/// Act
		Exception exceptionResult = Record.Exception(() =>
			new UpdateDealHubSpotCommandHandler(
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
			new UpdateDealHubSpotCommandHandler(
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
			new UpdateDealHubSpotCommandHandler(
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
	public async void Handle_DealNotFoundFailsRefreshingToken_ReturnsFailure()
	{
		// Arrange
		UpdateDealHubSpotCommand command = new(OBJECTID, SUPPLIERHUBSPOTID, UPDATETIME, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

		Error error = new("0", "error");

		SetupDealRepositoryMock(null);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenFromSupplierHubSpotIdAsync(It.IsAny<long>(), default))
			.ReturnsAsync(Result.Failure<string>(error));

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_hubSpotAuthorizationServiceMock.Verify(
			service => service.RefreshAccessTokenFromSupplierHubSpotIdAsync(SUPPLIERHUBSPOTID, default),
			Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equal(error, result.Error);
	}

	[Fact]
	public async void Handle_DealNotFoundFailedFetchingDealFromHubSpot_ReturnsFailure()
	{
		// Arrange
		UpdateDealHubSpotCommand command = new(OBJECTID, SUPPLIERHUBSPOTID, UPDATETIME, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

		SetupDealRepositoryMock(null);

		Error error = new("0", "error");

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenFromSupplierHubSpotIdAsync(It.IsAny<long>(), default))
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
		UpdateDealHubSpotCommand command = new(OBJECTID, SUPPLIERHUBSPOTID, UPDATETIME, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

		SetupDealRepositoryMock(null);

		Mock<Deal> newDealMock = new();
		newDealMock.Object.SourceId = "hubspotId";
		newDealMock.Object.SourceOwnerId = "ownerId";

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenFromSupplierHubSpotIdAsync(It.IsAny<long>(), default))
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
		UpdateDealHubSpotCommand command = new(OBJECTID, SUPPLIERHUBSPOTID, UPDATETIME, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

		Contact existingContact = new()
		{
			SourceId = "contactHubSpotId",
		};

		Client existingClient = new()
		{
			SourceId = "clientHubSpotId",
		};

		SetupDealRepositoryMock(null);

		Mock<Deal> newDealMock = new();
		newDealMock.Object.SourceId = "hubspotId";
		newDealMock.Object.SourceOwnerId = "ownerId";
		newDealMock.Object.Client = existingClient;
		newDealMock.Object.DealContacts = new List<DealContact>()
		{
			new DealContact()
			{
				Deal = newDealMock.Object,
				DealId = newDealMock.Object.Id,
				SourceDealId = newDealMock.Object.SourceId,
				SourceContactId = existingContact.SourceId,
			}
		};

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenFromSupplierHubSpotIdAsync(It.IsAny<long>(), default))
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
		UpdateDealHubSpotCommand command = new(OBJECTID, SUPPLIERHUBSPOTID, UPDATETIME, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

		Deal? existingDeal = null;

		Contact existingContact = new()
		{
			SourceId = "contactHubSpotId",
		};

		Client existingClient = new()
		{
			SourceId = "clientHubSpotId",
		};

		SetupDealRepositoryMock(existingDeal);

		Mock<Deal> newDealMock = new();
		newDealMock.Object.SourceId = "hubspotId";
		newDealMock.Object.SourceOwnerId = "ownerId";
		newDealMock.Object.Client = existingClient;
		newDealMock.Object.DealContacts = new List<DealContact>()
		{
			new DealContact()
			{
				Deal = newDealMock.Object,
				DealId = newDealMock.Object.Id,
				SourceDealId = newDealMock.Object.SourceId,
				SourceContactId = existingContact.SourceId,
			}
		};

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenFromSupplierHubSpotIdAsync(It.IsAny<long>(), default))
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
		_unitOfWorkMock.Verify(
			x => x.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);

		Assert.Equal(result.Error, Error.None);
	}

	[Fact]
	public async void Handle_DealFoundAccountManagerAssociationUpdatedIdEqualToActive_ReturnsSuccess()
	{
		// Arrange
		Mock<Deal> dealMock = new();
		dealMock.Object.AccountManagerDeals = new List<AccountManagerDeal>()
		{
			new AccountManagerDeal()
			{
				SourceAccountManagerId="equal",
				IsActive=true,
			}
		};

		SetupDealRepositoryMock(dealMock.Object);

		UpdateDealHubSpotCommand command = new(1, 1, 1, "hs_all_owner_ids", "equal");

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		dealMock.Verify(
		x => x.UpdateProperty(
			command.PropertyName,
			command.PropertyValue));

		_unitOfWorkMock.Verify(
			x => x.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}

	[Fact]
	public async void Handle_DealFoundAccountManagerAssociationUpdated_ReturnsSuccess()
	{
		// Arrange
		Mock<Deal> dealMock = new();

		SetupDealRepositoryMock(dealMock.Object);

		UpdateDealHubSpotCommand command = new(1, 1, 1, "hs_all_owner_ids", EMPTY_PROPERTYVALUE);

		_accountManagerRepositoryMock
			.Setup(a => a
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<AccountManager, bool>>>(),
					default))
			.ReturnsAsync(new AccountManager());

		_accountManagerDealRepositoryMock
			.Setup(a => a.Attach(It.IsAny<AccountManagerDeal>()));

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		dealMock.Verify(
		x => x.UpdateProperty(
			command.PropertyName,
			command.PropertyValue));

		_unitOfWorkMock.Verify(
			x => x.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}

	[Fact]
	public async void Handle_DealFoundAccountManagerAssociationUpdatedAccountManagerNotFound_ReturnsSuccessWithoutChanges()
	{
		// Arrange
		Mock<Deal> dealMock = new();

		SetupDealRepositoryMock(dealMock.Object);

		UpdateDealHubSpotCommand command = new(
			1,
			1,
			1,
			"hs_all_owner_ids",
			EMPTY_PROPERTYVALUE);

		_accountManagerRepositoryMock
			.Setup(a => a
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<AccountManager, bool>>>(),
					default))
			.ReturnsAsync((AccountManager)null!);

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		dealMock.Verify(
			x => x.UpdateProperty(
				command.PropertyName,
				command.PropertyValue));

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
		UpdateDealHubSpotCommand command = new(OBJECTID, SUPPLIERHUBSPOTID, UPDATETIME, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

		Mock<Deal> dealMock = new();

		dealMock
			.Setup(d => d.UpdateProperty(It.IsAny<string>(), It.IsAny<string>()))
			.Returns(dealMock.Object);

		SetupDealRepositoryMock(dealMock.Object);

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		dealMock.Verify(
		x => x.UpdateProperty(
			command.PropertyName,
			command.PropertyValue));

		_unitOfWorkMock.Verify(
			x => x.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}

	[Theory]
	[InlineData(0, 0, 1, "testProperty", "testValue")]
	public async void Handle_DealFoundLastCommandUpdateTimeOlderThanLastUpdateAndOlderThanCreateAtAuthServiceReturnsFailure_ReturnsFailure(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateDealHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Mock<Deal> dealMock = new();

		dealMock.Object.LastUpdatedAt = 10;
		dealMock.Object.CreatedAt = 5;

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.DealRepository
				.FindByCondition(
					It.IsAny<Expression<Func<Deal, bool>>>()))
			.Returns(new List<Deal> { dealMock.Object }.AsQueryable());

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
	public async void Handle_DealFoundLastCommandUpdateTimeOlderThanLastUpdateAndOlderThanCreateAtDealServiceReturnsFailure_ReturnsFailure(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateDealHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Mock<Deal> dealMock = new();

		dealMock.Object.LastUpdatedAt = 10;
		dealMock.Object.CreatedAt = 5;


		string accessToken = "accessToken";

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.DealRepository
				.FindByCondition(
					It.IsAny<Expression<Func<Deal, bool>>>()))
			.Returns(new List<Deal> { dealMock.Object }.AsQueryable());

		_hubSpotAuthorizationServiceMock.Setup(
			h => h.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(accessToken));

		_hubSpotDealServiceMock
			.Setup(h => h.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Deal>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, default);

		//Assert
		_hubSpotDealServiceMock.Verify(
			c => c.GetByIdAsync(
				accessToken,
				command.ObjectId,
				default),
			Times.Once);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, 1, "testProperty", "testValue", 123, 42123, 23123, "testDealStage", "testDealName", "testDealDescription", "testSourceOwnerId")]
	public async void Handle_DealFoundLastCommandUpdateTimeOlderThanLastUpdateAndOlderThanCreateAtDealServiceReturnsSuccessClientUpdated_ReturnsSuccess(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue,
		long testEndDate,
		long testStartDate,
		long testLastContactDate,
		string testDealStatus,
		string testDealName,
		string testDealDescription,
		string testSourceOwnerId)
	{
		//Arrange
		UpdateDealHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Mock<Deal> dealMock = new();

		dealMock.Object.LastUpdatedAt = 10;
		dealMock.Object.CreatedAt = 5;

		string accessToken = "accessToken";

		Deal dealResult = new()
		{
			EndDate = testEndDate,
			StartDate = testStartDate,
			LastContactDate = testLastContactDate,
			Status = testDealStatus,
			Name = testDealName,
			Description = testDealDescription,
			SourceOwnerId = testSourceOwnerId,
		};

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.DealRepository
				.FindByCondition(
					It.IsAny<Expression<Func<Deal, bool>>>()))
			.Returns(new List<Deal> { dealMock.Object }.AsQueryable());

		_hubSpotAuthorizationServiceMock.Setup(
			h => h.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(accessToken));

		_hubSpotDealServiceMock
			.Setup(h => h.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(dealResult));

		// Act
		var result = await _uut.Handle(command, default);

		//Assert
		dealMock.Verify(x => x.UpdatePropertiesFromDeal(dealResult), Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);
	}

	[Theory]
	[InlineData(0, 0, 1, "testProperty", "testValue", 123, 42123, 23123, "testDealStage", "testDealName", "testDealDescription", "testSourceOwnerId")]
	public async void Handle_DealFoundLastCommandUpdateTimeOlderThanLastUpdateAndOlderThanCreateAtSourceOwnerIdNotNullClientUpdated_ReturnsSuccess(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue,
		long testEndDate,
		long testStartDate,
		long testLastContactDate,
		string testDealStatus,
		string testDealName,
		string testDealDescription,
		string testSourceOwnerId)
	{
		//Arrange
		UpdateDealHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Mock<Deal> dealMock = new();

		dealMock.Object.LastUpdatedAt = 10;
		dealMock.Object.CreatedAt = 5;

		string accessToken = "accessToken";

		Deal dealResult = new()
		{
			EndDate = testEndDate,
			StartDate = testStartDate,
			LastContactDate = testLastContactDate,
			Status = testDealStatus,
			Name = testDealName,
			Description = testDealDescription,
			SourceOwnerId = testSourceOwnerId,

			AccountManagerDeals = new List<AccountManagerDeal>()
			{
				new AccountManagerDeal()
			},
		};
		Mock<AccountManager> accountManagerMock = new();

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.DealRepository
				.FindByCondition(
					It.IsAny<Expression<Func<Deal, bool>>>()))
			.Returns(new List<Deal> { dealMock.Object }.AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.AccountManagerRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<AccountManager, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(accountManagerMock.Object);

		_hubSpotAuthorizationServiceMock.Setup(
			h => h.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(accessToken));

		_hubSpotDealServiceMock
			.Setup(h => h.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(dealResult));

		// Act
		var result = await _uut.Handle(command, default);

		//Assert
		dealMock.Verify(x => x.UpdatePropertiesFromDeal(dealResult), Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);
	}
}
