using Moq;
using Pelican.Application.Clients.Queries.GetClientById;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Xunit;
namespace Pelican.Application.Test;
public class GetClientByIdQueryHandlerUnitTest
{
	private GetClientByIdQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledDataLoaderIsCalledWithCorrectParameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IClientByIdDataLoader>();
		uut = new GetClientByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetClientByIdQuery getClientByIdQuery = new GetClientByIdQuery(guid);
		//Act
		uut.Handle(getClientByIdQuery, cancellationToken);
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
	}
}
