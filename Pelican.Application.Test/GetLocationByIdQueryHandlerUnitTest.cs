using Moq;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Application.Locations.Queries.GetLocationById;
using Xunit;
namespace Pelican.Application.Test;
public class GetLocationByIdQueryHandlerUnitTest
{
	private GetLocationByIdQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledDataLoaderIsCalledWithCorrectParameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<ILocationByIdDataLoader>();
		uut = new GetLocationByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetLocationByIdQuery getLocationByIdQuery = new GetLocationByIdQuery(guid);
		//Act
		uut.Handle(getLocationByIdQuery, cancellationToken);
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
	}
	[Fact]
	public void TestIfWhenHandleIsCalledMultipleTimesDataLoaderIsCalledWithCorrectParametersMultipleTimes()
	{
		//Arrange
		var dataLoaderMock = new Mock<ILocationByIdDataLoader>();
		uut = new GetLocationByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetLocationByIdQuery getLocationByIdQuery = new GetLocationByIdQuery(guid);
		//Act
		for (int i = 0; i < 50; i++)
		{
			uut.Handle(getLocationByIdQuery, cancellationToken);

		}
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Exactly(50));
	}
}
