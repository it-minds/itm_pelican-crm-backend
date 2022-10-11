using Moq;
using Pelican.Application.Clients.Queries.GetClientById;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;
using Xunit;
namespace Pelican.Application.Test.Clients.Queries;
public class GetClientByIdQueryHandlerUnitTest
{
	private GetClientByIdQueryHandler uut;
	[Fact]
	public async void TestIfWhenHandleIsCalledDataLoaderIsCalledWithCorrectParameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<Client>>();
		uut = new GetClientByIdQueryHandler(dataLoaderMock.Object);
		var cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		var getClientByIdQuery = new GetClientByIdQuery(guid);
		var resultList = new List<Client>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new Client(guid));
		//Act
		resultList.Add(await uut.Handle(getClientByIdQuery, cancellationToken));
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
	[Fact]
	public async void TestIfWhenHandleIsCalledMultipleTimesDataLoaderIsCalledWithCorrectParametersMultipleTimes()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<Client>>();
		uut = new GetClientByIdQueryHandler(dataLoaderMock.Object);
		var cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		var getClientByIdQuery = new GetClientByIdQuery(guid);
		var resultList = new List<Client>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new Client(guid));
		//Act
		for (var i = 0; i < 50; i++)
		{
			resultList.Add(await uut.Handle(getClientByIdQuery, cancellationToken));
		}
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Exactly(50));
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
}
