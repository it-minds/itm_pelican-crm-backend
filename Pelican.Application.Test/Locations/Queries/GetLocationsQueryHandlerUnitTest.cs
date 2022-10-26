using Moq;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.Locations.Queries.GetLocations;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.Locations.Queries;
public class GetLocationsQueryHandlerUnitTest
{
	[Fact]
	public async void Test_If_When_Handle_Is_Called_Repository_Is_Called()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var locationRepositoryMock = new Mock<IGenericRepository<Location>>();
		unitOfWorkMock.Setup(x => x.LocationRepository).Returns(locationRepositoryMock.Object);
		var uut = new GetLocationsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetLocationsQuery locationsQuery = new GetLocationsQuery();
		//Act
		_ = await uut.Handle(locationsQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.LocationRepository.FindAllWithIncludes(), Times.Once());
	}
}
