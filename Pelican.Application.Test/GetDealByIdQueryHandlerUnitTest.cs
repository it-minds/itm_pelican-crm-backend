using Moq;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Application.Deals.Queries.GetDealById;
using Xunit;
namespace Pelican.Application.Test;
public class GetDealByIdQueryHandlerUnitTest
{
	private GetDealByIdQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledDataLoaderIsCalledWithCorrectParameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IDealByIdDataLoader>();
		uut = new GetDealByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetDealByIdQuery getDealByIdQuery = new GetDealByIdQuery(guid);
		//Act
		uut.Handle(getDealByIdQuery, cancellationToken);
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
	}
	[Fact]
	public void TestIfWhenHandleIsCalledMultipleTimesDataLoaderIsCalledWithCorrectParametersMultipleTimes()
	{
		//Arrange
		var dataLoaderMock = new Mock<IDealByIdDataLoader>();
		uut = new GetDealByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetDealByIdQuery getDealByIdQuery = new GetDealByIdQuery(guid);
		//Act
		for (int i = 0; i < 50; i++)
		{
			uut.Handle(getDealByIdQuery, cancellationToken);

		}
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Exactly(50));
	}
}
