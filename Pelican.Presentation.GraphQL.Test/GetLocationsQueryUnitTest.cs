using MediatR;
using Moq;
using Pelican.Application.Locations.Queries.GetLocationById;
using Pelican.Application.Locations.Queries.GetLocations;
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
		uut.GetLocations(mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(It.IsAny<GetLocationsQuery>(), cancellationToken), Times.Exactly(1));
	}
	[Fact]
	public void IfGetLocationAsyncIsCalledMediatorCallsSendWithCorrectCancellationTokenAndInput()
	{
		//Arrange
		uut = new LocationsQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		Guid id = Guid.NewGuid();
		GetLocationByIdQuery input = new GetLocationByIdQuery(id);
		//Act
		uut.GetLocationAsync(input, mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(input, cancellationToken), Times.Exactly(1));
	}
}
