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
	private readonly UpdateDealCommandHandler _uut;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly Mock<IGenericRepository<Deal>> _dealRepositoryMock;
	private readonly Mock<IGenericRepository<Supplier>> _supplierRepositoryMock;
	private readonly Mock<IHubSpotObjectService<Deal>> _hubSpotDealServiceMock;
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock;

	public UpdateDealCommandHandlerTests()
	{
		_unitOfWorkMock = new();
		_dealRepositoryMock = new();
		_supplierRepositoryMock = new();
		_hubSpotDealServiceMock = new();
		_hubSpotAuthorizationServiceMock = new();

		_uut = new(
			_unitOfWorkMock.Object,
			_hubSpotDealServiceMock.Object,
			_hubSpotAuthorizationServiceMock.Object);

		SetupUnitOfWork();
	}

	private void SetupUnitOfWork()
	{
		_unitOfWorkMock
			.Setup(
				u => u.DealRepository)
			.Returns(_dealRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(
				u => u.SupplierRepository)
			.Returns(_supplierRepositoryMock.Object);
	}

	private void SetupRepositoryMocks(Deal? deal, Supplier? supplier)
	{
		_dealRepositoryMock
			.Setup(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Deal, bool>>>(), default))
			.ReturnsAsync(deal);

		_supplierRepositoryMock
			.Setup(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), default))
			.ReturnsAsync(supplier);
	}

	[Fact]
	public void UpdateDealCommandHandler_unitOfWorkNull_ThrowException()
	{
		/// Act
		Exception exceptionResult = Record.Exception(() =>
			new UpdateDealCommandHandler(
				null,
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
				null,
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
				null));

		/// Assert
		Assert.NotNull(exceptionResult);

		Assert.Equal(
			typeof(ArgumentNullException),
			exceptionResult.GetType());

		Assert.Equal(
			"Value cannot be null. (Parameter 'hubSpotAuthorizationService')",
			exceptionResult.Message);
	}

	[Theory]
	[InlineData(1, "0", "0", "0")]
	public async Task Handle_DealNotFoundSupplierNotFound_ReturnsFailureErrorNullValue(
		long objectId,
		string userId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateDealCommand command = new(objectId, userId, propertyName, propertyValue);

		Deal? existingDeal = null;
		Supplier? existingSupplier = null;

		SetupRepositoryMocks(existingDeal, existingSupplier);

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_dealRepositoryMock
			.Verify(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Deal, bool>>>(), default),
				Times.Once());

		_supplierRepositoryMock
			.Verify(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), default),
				Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, "0", "0", "0")]
	public async void Handle_DealNotFoundFailsRefreshingToken_ReturnsFailure(
		long objectId,
		string userId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateDealCommand command = new(objectId, userId, propertyName, propertyValue);

		Deal? existingDeal = null;

		Supplier existingSupplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};

		Error error = new("0", "error");

		SetupRepositoryMocks(existingDeal, existingSupplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(It.IsAny<string>(), default))
			.ReturnsAsync(Result.Failure<string>(error));

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_dealRepositoryMock
			.Verify(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Deal, bool>>>(), default),
				Times.Once());

		_supplierRepositoryMock
			.Verify(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), default),
				Times.Once());

		_hubSpotAuthorizationServiceMock
			.Verify(
				service => service.RefreshAccessTokenAsync(It.IsAny<string>(), default),
				Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal(error, result.Error);
	}

	[Theory]
	[InlineData(0, "0", "0", "0")]
	public async void Handle_DealNotFoundFailedFetchingFromHubSpot_ReturnsFailure(
		long objectId,
		string userId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateDealCommand command = new(objectId, userId, propertyName, propertyValue);

		string token = "token";

		Deal existingDeal = null;

		Supplier existingSupplier = new(Guid.NewGuid())
		{
			RefreshToken = token,
		};

		Error error = new("0", "error");

		SetupRepositoryMocks(existingDeal, existingSupplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(It.IsAny<string>(), default))
			.ReturnsAsync(Result.Success(token));

		_hubSpotDealServiceMock
			.Setup(service => service.GetByIdAsync(token, command.ObjectId, default))
			.ReturnsAsync(Result.Failure<Deal>(error));

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_dealRepositoryMock
			.Verify(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Deal, bool>>>(), default),
				Times.Once());

		_supplierRepositoryMock
			.Verify(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), default),
				Times.Once());

		_hubSpotAuthorizationServiceMock
			.Verify(
				service => service.RefreshAccessTokenAsync(It.IsAny<string>(), default),
				Times.Once());

		_hubSpotDealServiceMock.Verify(
			service => service.GetByIdAsync("token", command.ObjectId, default),
			Times.Once());

		Assert.True(result.IsFailure);
		Assert.Equal(error, result.Error);
	}

	[Theory]
	[InlineData(0, "0", "0", "0")]
	public async void Handle_DealNotFoundSuccessFetchingFromHubSpot_ReturnsSuccess(
		long objectId,
		string userId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateDealCommand command = new(objectId, userId, propertyName, propertyValue);

		string token = "token";

		Deal existingDeal = null;

		Supplier existingSupplier = new(Guid.NewGuid())
		{
			RefreshToken = token,
		};

		Deal newDeal = new(Guid.NewGuid());

		SetupRepositoryMocks(existingDeal, existingSupplier);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(It.IsAny<string>(), default))
			.ReturnsAsync(Result.Success(token));

		_hubSpotDealServiceMock
			.Setup(service => service.GetByIdAsync(token, command.ObjectId, default))
			.ReturnsAsync(Result.Success(newDeal));

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_dealRepositoryMock
			.Verify(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Deal, bool>>>(), default),
				Times.Once());

		_supplierRepositoryMock
			.Verify(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Supplier, bool>>>(), default),
				Times.Once());

		_hubSpotAuthorizationServiceMock
			.Verify(
				service => service.RefreshAccessTokenAsync(It.IsAny<string>(), default),
				Times.Once());

		_hubSpotDealServiceMock.Verify(
			service => service.GetByIdAsync("token", command.ObjectId, default),
			Times.Once());

		_dealRepositoryMock
			.Verify(x => x.CreateAsync(newDeal, default),
			Times.Once());

		_unitOfWorkMock
			.Verify(x => x.SaveAsync(default));

		Assert.True(result.IsSuccess);
		Assert.Equal(result.Error, Error.None);
	}

	[Theory]
	[InlineData(0, "0", "0", "0")]
	public async void Handle_DealFoundPropertyUpdated_ReturnsSuccess(
		long objectId,
		string userId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateDealCommand command = new(objectId, userId, propertyName, propertyValue);

		Deal deal = new(Guid.NewGuid());

		_dealRepositoryMock
			.Setup(x => x
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Deal, bool>>>(), default))
			.ReturnsAsync(deal);

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_dealRepositoryMock
			.Verify(
				x => x.FirstOrDefaultAsync(deal => deal.HubSpotId == command.ObjectId.ToString(), default),
				Times.Once());

		_dealRepositoryMock
			.Verify(
				x => x.Update(deal)
				, Times.Once());

		_unitOfWorkMock.Verify(
			x => x.SaveAsync(default),
			Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}
}
