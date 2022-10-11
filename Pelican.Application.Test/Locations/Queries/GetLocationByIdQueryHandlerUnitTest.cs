using Moq;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Application.Locations.Queries.GetLocationById;
using Pelican.Domain.Entities;
using Xunit;
namespace Pelican.Application.Test.Locations.Queries;
public class GetLocationByIdQueryHandlerUnitTest
{
	private GetLocationByIdQueryHandler uut;
	[Fact]
	public async void TestIfWhenHandleIsCalledDataLoaderIsCalledWithCorrectParameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<Location>>();
		uut = new GetLocationByIdQueryHandler(dataLoaderMock.Object);
		var cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		var getLocationByIdQuery = new GetLocationByIdQuery(guid);
		var resultList = new List<Location>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new Location(guid));
		//Act
		resultList.Add(await uut.Handle(getLocationByIdQuery, cancellationToken));
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
	[Fact]
	public async void TestIfWhenHandleIsCalledMultipleTimesDataLoaderIsCalledWithCorrectParametersMultipleTimes()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<Location>>();
		uut = new GetLocationByIdQueryHandler(dataLoaderMock.Object);
		var cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		var getLocationByIdQuery = new GetLocationByIdQuery(guid);
		var resultList = new List<Location>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new Location(guid));
		//Act
		for (var i = 0; i < 50; i++)
		{
			resultList.Add(await uut.Handle(getLocationByIdQuery, cancellationToken));
		}
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Exactly(50));
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
}
