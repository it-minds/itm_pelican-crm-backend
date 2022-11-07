using Moq;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.Deals.Commands.DeleteDeal;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Deals.Commands.DeleteDeal;
public class DeleteDealCommandHandlerTests
{
	private readonly DeleteDealCommandHandler _uut;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly CancellationToken _cancellationToken;

	public DeleteDealCommandHandlerTests()
	{
		_unitOfWorkMock = new();
		_uut = new(_unitOfWorkMock.Object);
		_cancellationToken = new();
	}


	[Fact]
	public void DeleteDealCommandHandler_UnitOfWorkNull_ThrowException()
	{
		/// Act
		Exception exceptionResult = Record.Exception(() =>
			new DeleteDealCommandHandler(
				null!));

		/// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			exceptionResult.GetType());

		Assert.Equal(
			"Value cannot be null. (Parameter 'unitOfWork')",
			exceptionResult.Message);
	}

	[Fact]
	public async void Handle_DealNotFound_ReturnsSuccess()
	{
		// Arrange
		DeleteDealCommand command = new(0);

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.DealRepository.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(Enumerable.Empty<Deal>().AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock
			.Verify(
				unitOfWork => unitOfWork.DealRepository.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()),
				Times.Once());

		_unitOfWorkMock
			.Verify(
				unitOfWork => unitOfWork.DealRepository.Delete(It.IsAny<Deal>()),
				Times.Never());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}

	[Fact]
	public async void Handle_DealFound_ReturnsSuccess()
	{
		// Arrange
		DeleteDealCommand command = new(0);
		Deal deal = new(Guid.NewGuid());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.DealRepository.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(new List<Deal>
			{
				deal
			}.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock
			.Verify(unitOfWork => unitOfWork.DealRepository.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock
			.Verify(unitOfWork => unitOfWork.DealRepository.Delete(deal),
			Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}
}
