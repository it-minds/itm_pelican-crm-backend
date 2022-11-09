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
	private readonly CancellationToken _cancellationToken;

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

		_cancellationToken = new();
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_DealNotFoundSupplierNotFound_ReturnsFailureErrorNullValue(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateDealCommand command = new(objectId, portalId, propertyName, propertyValue);

		_unitOfWorkMock
			.Setup(
				u => u
					.DealRepository
					.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(Enumerable.Empty<Deal>().AsQueryable());

		_unitOfWorkMock
			.Setup(
				u => u
					.SupplierRepository
					.FindByCondition(supplier => supplier.HubSpotId == portalId))
			.Returns(Enumerable.Empty<Supplier>().AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			u => u
				.DealRepository
				.FindByCondition(
					deal => deal.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			u => u
				.SupplierRepository
				.FindByCondition(
					supplier => supplier.HubSpotId == portalId),
			Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_DealNotFoundFailsRefreshingToken_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateDealCommand command = new(objectId, portalId, propertyName, propertyValue);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.DealRepository.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(Enumerable.Empty<Deal>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.SupplierRepository
				.FindByCondition(supplier => supplier.HubSpotId == portalId))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(It.IsAny<string>(), _cancellationToken))
			.ReturnsAsync(Result.Failure<string>(new Error("0", "error")));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.DealRepository.FindByCondition(
				deal => deal.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.SupplierRepository.FindByCondition(
				supplier => supplier.HubSpotId == portalId),
			Times.Once());

		_hubSpotAuthorizationServiceMock.Verify(
			service => service.RefreshAccessTokenAsync(It.IsAny<string>(), _cancellationToken),
			Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal("0", result.Error.Code);
		Assert.Equal("error", result.Error.Message);
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_DealNotFoundSuccessFetchingFromHubSpot_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateDealCommand command = new(objectId, portalId, propertyName, propertyValue);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};

		Deal deal = new(Guid.NewGuid());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.DealRepository.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(Enumerable.Empty<Deal>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.SupplierRepository
				.FindByCondition(supplier => supplier.HubSpotId == portalId))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(It.IsAny<string>(), _cancellationToken))
			.ReturnsAsync(Result.Success("token"));

		_hubSpotDealServiceMock
			.Setup(service => service.GetByIdAsync("token", command.ObjectId, _cancellationToken))
			.ReturnsAsync(Result.Success(deal));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.DealRepository.FindByCondition(
				deal => deal.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.SupplierRepository.FindByCondition(
				supplier => supplier.HubSpotId == portalId),
			Times.Once());

		_hubSpotDealServiceMock.Verify(
			service => service.GetByIdAsync("token", command.ObjectId, _cancellationToken),
			Times.Once());

		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_DealFoundNoUpdates_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateDealCommand command = new(objectId, portalId, propertyName, propertyValue);

		Deal deal = new(Guid.NewGuid());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.DealRepository.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(new List<Deal> { deal }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.DealRepository.FindByCondition(
				deal => deal.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.DealRepository.Update(deal),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.SaveAsync(_cancellationToken),
			Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "closedate", "1664958141535")]
	public async void Handle_DealFoundCloseDateUpdated_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateDealCommand command = new(objectId, portalId, propertyName, propertyValue);

		Deal deal = new(Guid.NewGuid());

		Deal updatedDeal = deal;
		updatedDeal.EndDate = new DateTime(Convert.ToInt64(command.PropertyValue), DateTimeKind.Utc);

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.DealRepository.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(new List<Deal> { deal }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.DealRepository.FindByCondition(
				deal => deal.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.DealRepository.Update(updatedDeal),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.SaveAsync(_cancellationToken),
			Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}

}
