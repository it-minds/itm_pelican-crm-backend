﻿using Moq;
using Pelican.Application.Locations.Queries.GetLocations;
using Pelican.Domain.Repositories;
using Xunit;
namespace Pelican.Application.Test;
public class GetLocationsQueryHandlerUnitTest
{
	private GetLocationsQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledRepositoryIsCalled()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var locationRepositoryMock = new Mock<ILocationRepository>();
		unitOfWorkMock.Setup(x => x.LocationRepository).Returns(locationRepositoryMock.Object);
		uut = new GetLocationsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetLocationsQuery locationsQuery = new GetLocationsQuery();
		//Act
		_ = uut.Handle(locationsQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.LocationRepository.FindAll(), Times.Once());
	}
	[Fact]
	public void TestIfWhenHandleIsCalledMultipleTimesRepositoryIsCalledMultipleTimes()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var locationRepositoryMock = new Mock<ILocationRepository>();
		unitOfWorkMock.Setup(x => x.LocationRepository).Returns(locationRepositoryMock.Object);
		uut = new GetLocationsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetLocationsQuery locationsQuery = new GetLocationsQuery();
		//Act
		for (int i = 0; i < 50; i++)
		{
			_ = uut.Handle(locationsQuery, cancellationToken);
		}

		//Assert
		unitOfWorkMock.Verify(x => x.LocationRepository.FindAll(), Times.Exactly(50));
	}
}
