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

	private readonly UpdateDealCommand _command = new(OBJECTID, SUPPLIERHUBSPOTID, EMPTY_PROPERTYNAME, EMPTY_PROPERTYVALUE);

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
		Assert.NotNull(exceptionResult);

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
		Assert.NotNull(exceptionResult);

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
		Assert.NotNull(exceptionResult);

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
		Deal? existingDeal = null;
		Supplier? existingSupplier = null;

		SetupRepositoryMocks(existingDeal, existingSupplier);

		// Act
		Result result = await _uut.Handle(_command, default);

		// Assert
		_dealRepositoryMock
			.Verify(x => x
				.FindByCondition(It.IsAny<Expression<Func<Deal, bool>>>()),
				Times.Once());

		_supplierRepositoryMock
			.Verify(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), default),
				Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Fact]
	public async void Handle_DealNotFoundFailsRefreshingToken_ReturnsFailure()
	{
		// Arrange
		Deal? existingDeal = null;

		Supplier existingSupplier = new(Guid.NewGuid())
		{
			RefreshToken = TOKEN,
		};

		Error error = new("0", "error");

		SetupRepositoryMocks(existingDeal, existingSupplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(It.IsAny<string>(), default))
			.ReturnsAsync(Result.Failure<string>(error));

		// Act
		Result result = await _uut.Handle(_command, default);

		// Assert
		_hubSpotAuthorizationServiceMock
			.Verify(
				service => service.RefreshAccessTokenAsync(It.IsAny<string>(), default),
				Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal(error, result.Error);
	}

	[Fact]
	public async void Handle_DealNotFoundFailedFetchingDealFromHubSpot_ReturnsFailure()
	{
		// Arrange
		Deal? existingDeal = null;

		Supplier existingSupplier = new(Guid.NewGuid())
		{
			RefreshToken = TOKEN,
		};

		SetupRepositoryMocks(existingDeal, existingSupplier);

		Error error = new("0", "error");

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(It.IsAny<string>(), default))
			.ReturnsAsync(Result.Success(TOKEN));

		_hubSpotDealServiceMock
			.Setup(service => service.GetByIdAsync(TOKEN, _command.ObjectId, default))
			.ReturnsAsync(Result.Failure<Deal>(error));

		// Act
		Result result = await _uut.Handle(_command, default);

		// Assert
		_hubSpotDealServiceMock.Verify(
			service => service.GetByIdAsync(TOKEN, _command.ObjectId, default),
			Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal(error, result.Error);
	}

	[Fact]
	public async void Handle_DealNotFoundSuccessFetchingFromHubSpot_ReturnsSuccess()
	{
		// Arrange
		Deal? existingDeal = null;

		Supplier existingSupplier = new(Guid.NewGuid())
		{
			RefreshToken = TOKEN,
		};

		SetupRepositoryMocks(existingDeal, existingSupplier);

		Deal newDeal = new(Guid.NewGuid())
		{
			HubSpotOwnerId = "ownerid",
			HubSpotId = "hubspotid",
		};

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(It.IsAny<string>(), default))
			.ReturnsAsync(Result.Success(TOKEN));

		_hubSpotDealServiceMock
			.Setup(service => service.GetByIdAsync(TOKEN, _command.ObjectId, default))
			.ReturnsAsync(Result.Success(newDeal));

		_accountManagerRepositoryMock
			.Setup(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<AccountManager, bool>>>(), default))
			.ReturnsAsync((AccountManager?)null);

		_clientRepositoryMock
			.Setup(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Client, bool>>>(), default))
			.ReturnsAsync((Client?)null);

		_contactRepositoryMock
			.Setup(x => x
				.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(new List<Contact>().AsQueryable());

		// Act
		Result result = await _uut.Handle(_command, default);

		// Assert
		_dealRepositoryMock
			.Verify(x => x.CreateAsync(newDeal, default),
			Times.Once());

		_unitOfWorkMock
			.Verify(x => x.SaveAsync(default));

		Assert.True(result.IsSuccess);

		Assert.Equal(result.Error, Error.None);
	}

	[Fact]
	public async void Handle_DealFoundPropertyUpdated_ReturnsSuccess()
	{
		// Arrange
		Deal deal = new(Guid.NewGuid());

		SetupRepositoryMocks(deal, null);

		// Act
		Result result = await _uut.Handle(_command, default);

		// Assert
		_dealRepositoryMock
			.Verify(
				x => x.FindByCondition(
					It.IsAny<Expression<Func<Deal, bool>>>()),
				Times.Once());

		_dealRepositoryMock
			.Verify(
				x => x.Update(deal),
				Times.Once());

		_unitOfWorkMock.Verify(
			x => x.SaveAsync(default),
			Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}
}
