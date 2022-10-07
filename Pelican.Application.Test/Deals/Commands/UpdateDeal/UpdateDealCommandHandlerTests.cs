using Moq;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Deals.Commands.UpdateDeal;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Deals.Commands.UpdateDeal;

public class UpdateDealCommandHandlerTests
{
	private readonly UpdateDealCommandHandler _uut;
	private readonly Mock<IRepositoryWrapper> _unitOfWorkMock;
	private readonly Mock<IHubSpotDealService> _hubSpotDealServiceMock;
	private readonly CancellationToken _cancellationToken;

	public UpdateDealCommandHandlerTests()
	{
		_unitOfWorkMock = new();
		_hubSpotDealServiceMock = new();

		_uut = new(
			_unitOfWorkMock.Object,
			_hubSpotDealServiceMock.Object);

		_cancellationToken = new();
	}

	[Fact]
	public async void Handle_DealNotFoundAccountManagerNotFound_ReturnsFailureErrorNullValue()
	{
		// Arrange
		UpdateDealCommand command = new(
			0,
			"0",
			"0",
			"0");

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.Deal.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(Enumerable.Empty<Deal>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.AccountManager.FindByCondition(accountManager => accountManager.Id.ToString() == command.UserId.ToString()))
			.Returns(Enumerable.Empty<AccountManager>().AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.Deal.FindByCondition(
				deal => deal.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.AccountManager.FindByCondition(
				accountManager => accountManager.Id.ToString() == command.UserId.ToString()),
			Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Fact]
	public async void Handle_DealNotFoundFailsFetchingFromHubSpot_ReturnsFailure()
	{
		// Arrange
		UpdateDealCommand command = new(
			0,
			"0",
			"0",
			"0");

		AccountManager accountManager = new(
			Guid.NewGuid(),
			"name",
			string.Empty,
			"email",
			string.Empty,
			string.Empty,
			Guid.NewGuid());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.Deal.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(Enumerable.Empty<Deal>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.AccountManager.FindByCondition(accountManager => accountManager.Id.ToString() == command.UserId.ToString()))
			.Returns(new List<AccountManager> { accountManager }.AsQueryable());

		_hubSpotDealServiceMock
			.Setup(service => service.GetDealByIdAsync("", command.ObjectId, _cancellationToken))
			.ReturnsAsync(Result.Failure<Deal>(new Error("0", "error")));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.Deal.FindByCondition(
				deal => deal.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.AccountManager.FindByCondition(
				accountManager => accountManager.Id.ToString() == command.UserId.ToString()),
			Times.Once());

		_hubSpotDealServiceMock.Verify(
			service => service.GetDealByIdAsync("", command.ObjectId, _cancellationToken),
			Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal("0", result.Error.Code);
		Assert.Equal("error", result.Error.Message);
	}

	[Fact]
	public async void Handle_DealNotFoundSuccessFetchingFromHubSpot_ReturnsFailure()
	{
		// Arrange
		UpdateDealCommand command = new(
			0,
			"0",
			"0",
			"0");

		AccountManager accountManager = new(
			Guid.NewGuid(),
			"name",
			string.Empty,
			"email",
			string.Empty,
			string.Empty,
			Guid.NewGuid());

		Deal deal = new(
			Guid.NewGuid(),
			null,
			string.Empty,
			DateTime.Now,
			Guid.NewGuid());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.Deal.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(Enumerable.Empty<Deal>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.AccountManager.FindByCondition(accountManager => accountManager.Id.ToString() == command.UserId.ToString()))
			.Returns(new List<AccountManager> { accountManager }.AsQueryable());

		_hubSpotDealServiceMock
			.Setup(service => service.GetDealByIdAsync("", command.ObjectId, _cancellationToken))
			.ReturnsAsync(Result.Success(deal));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.Deal.FindByCondition(
				deal => deal.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.AccountManager.FindByCondition(
				accountManager => accountManager.Id.ToString() == command.UserId.ToString()),
			Times.Once());

		_hubSpotDealServiceMock.Verify(
			service => service.GetDealByIdAsync("", command.ObjectId, _cancellationToken),
			Times.Once());

		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}

	[Fact]
	public async void Handle_DealFoundNoUpdates_ReturnsFailure()
	{
		// Arrange
		UpdateDealCommand command = new(
			0,
			"0",
			"0",
			"0");

		Deal deal = new(
			Guid.NewGuid(),
			null,
			string.Empty,
			DateTime.Now,
			Guid.NewGuid());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.Deal.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(new List<Deal> { deal }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.Deal.FindByCondition(
				deal => deal.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.Deal.Update(deal),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.Save(),
			Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}

	[Fact]
	public async void Handle_DealFoundCloseDateUpdated_ReturnsFailure()
	{
		// Arrange
		UpdateDealCommand command = new(
			0,
			"0",
			"closedate",
			"1664958141535");

		Deal deal = new(
			Guid.NewGuid(),
			null,
			string.Empty,
			DateTime.Now,
			Guid.NewGuid());

		Deal updatedDeal = deal;
		updatedDeal.EndDate = new DateTime(Convert.ToInt64(command.PropertyValue), DateTimeKind.Utc);

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.Deal.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(new List<Deal> { deal }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.Deal.FindByCondition(
				deal => deal.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.Deal.Update(updatedDeal),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.Save(),
			Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}

}
