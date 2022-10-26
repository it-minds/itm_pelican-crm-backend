using Moq;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Application.Locations.Queries.GetLocationById;
using Pelican.Domain.Entities;
using Xunit;
namespace Pelican.Application.Test.Locations.Queries;
public class GetLocationByIdQueryHandlerUnitTest
{
	[Fact]
	public async void Test_If_When_Handle_Is_Called_DataLoader_Is_Called_With_Correct_Parameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<Location>>();
		var uut = new GetLocationByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetLocationByIdQuery getLocationByIdQuery = new GetLocationByIdQuery(guid);
		List<Location> resultList = new List<Location>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new Location(guid));
		//Act
		resultList.Add(await uut.Handle(getLocationByIdQuery, cancellationToken));
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
}
