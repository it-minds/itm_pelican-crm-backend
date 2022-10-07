using Moq;
using Pelican.Application.Deals.Commands.DeleteDeal;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Deals.Commands.DeleteDeal;
public class DeleteDealCommandHandlerTests
{
	private readonly DeleteDealCommandHandler _uut;
	private readonly Mock<IRepositoryWrapper> _repositoryWrapperMock;
	private readonly CancellationToken _cancellationToken;

	public DeleteDealCommandHandlerTests()
	{
		_repositoryWrapperMock = new();
		_uut = new(_repositoryWrapperMock.Object);
		_cancellationToken = new();
	}

	[Fact]
	public async void Handle_DealNotFound_ReturnsSuccess()
	{
		// Arrange
		DeleteDealCommand command = new(0);

		_repositoryWrapperMock
			.Setup(unitOfWork => unitOfWork.Deal.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(Enumerable.Empty<Deal>().AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_repositoryWrapperMock.Verify(unitOfWork => unitOfWork.Deal.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()), Times.Once());

		_repositoryWrapperMock.Verify(unitOfWork => unitOfWork.Deal.Delete(It.IsAny<Deal>()), Times.Never());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}

	[Fact]
	public async void Handle_DealFound_ReturnsSuccess()
	{
		// Arrange
		DeleteDealCommand command = new(0);
		Deal deal = new(Guid.NewGuid(),
					0,
					String.Empty,
					DateTime.Now,
					Guid.NewGuid());

		_repositoryWrapperMock
			.Setup(unitOfWork => unitOfWork.Deal.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()))
			.Returns(new List<Deal>
			{
				deal
			}.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_repositoryWrapperMock.Verify(unitOfWork => unitOfWork.Deal.FindByCondition(deal => deal.Id.ToString() == command.ObjectId.ToString()), Times.Once());

		_repositoryWrapperMock.Verify(unitOfWork => unitOfWork.Deal.Delete(deal), Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}
}
