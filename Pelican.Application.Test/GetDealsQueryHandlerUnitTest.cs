using Moq;
using Pelican.Application.Deals.Queries.GetDeals;
using Pelican.Domain.Repositories;
using Xunit;
namespace Pelican.Application.Test;
public class GetDealsQueryHandlerUnitTest
{
	private GetDealsQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledRepositoryIsCalled()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var dealRepositoryMock = new Mock<IDealRepository>();
		unitOfWorkMock.Setup(x => x.DealRepository).Returns(dealRepositoryMock.Object);
		uut = new GetDealsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetDealsQuery dealsQuery = new GetDealsQuery();
		//Act
		uut.Handle(dealsQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.DealRepository.FindAll(), Times.Once());
	}
	[Fact]
	public void TestIfWhenHandleIsCalledMultipleTimesRepositoryIsCalledMultipleTimes()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var dealRepositoryMock = new Mock<IDealRepository>();
		unitOfWorkMock.Setup(x => x.DealRepository).Returns(dealRepositoryMock.Object);
		uut = new GetDealsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetDealsQuery dealsQuery = new GetDealsQuery();
		//Act
		for (int i = 0; i < 50; i++)
		{
			uut.Handle(dealsQuery, cancellationToken);
		}

		//Assert
		unitOfWorkMock.Verify(x => x.DealRepository.FindAll(), Times.Exactly(50));
	}
}
