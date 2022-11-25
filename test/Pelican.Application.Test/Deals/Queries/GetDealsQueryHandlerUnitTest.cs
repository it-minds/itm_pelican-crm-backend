using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Deals.Queries.GetDeals;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.Deals.Queries;
public class GetDealsQueryHandlerUnitTest
{
	[Fact]
	public async void Test_If_When_Handle_Is_Called_Repository_Is_Called()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var dealRepositoryMock = new Mock<IGenericRepository<Deal>>();
		unitOfWorkMock.Setup(x => x.DealRepository).Returns(dealRepositoryMock.Object);
		var uut = new GetDealsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetDealsQuery dealsQuery = new GetDealsQuery();
		//Act
		_ = await uut.Handle(dealsQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.DealRepository.FindAll(), Times.Once());
	}
}
