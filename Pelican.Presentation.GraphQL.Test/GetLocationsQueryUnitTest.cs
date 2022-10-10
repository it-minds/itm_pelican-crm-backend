using MediatR;
using Moq;
using Pelican.Application.Locations.Queries.GetLocationById;
using Pelican.Application.Locations.Queries.GetLocations;
using Pelican.Domain.Entities;
using Pelican.Presentation.GraphQL.Locations;
using Xunit;

namespace Pelican.Presentation.GraphQL.Test;
public class GetLocationsQueryUnitTest
{
	private LocationsQuery uut;
	[Fact]
	public void IfGetLocationsIsCalledMediatorCallsSendWithCorrectCancellationToken()
	{
		//Arrange
		uut = new LocationsQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		//Act
		_ = uut.GetLocations(mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(It.IsAny<GetLocationsQuery>(), cancellationToken), Times.Once());
	}
	[Fact]
	public async void IfGetLocationAsyncIsCalledMediatorCallsSendWithCorrectCancellationTokenAndInput()
	{
		//Arrange
		uut = new LocationsQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		Guid id = Guid.NewGuid();
		GetLocationByIdQuery input = new GetLocationByIdQuery(id);
		mediatorMock.Setup(x => x.Send(input, cancellationToken)).ReturnsAsync(new Location { Id = id });
		//Act
		var result = await uut.GetLocationAsync(input, mediatorMock.Object, cancellationToken);
		//Assert
		Assert.Equal(id, result.Id);
		mediatorMock.Verify(x => x.Send(input, cancellationToken), Times.Once());
	}
}
