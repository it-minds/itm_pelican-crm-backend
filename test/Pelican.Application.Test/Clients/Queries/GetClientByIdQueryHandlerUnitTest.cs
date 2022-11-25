using Moq;
using Pelican.Application.Abstractions.Data.DataLoaders;
using Pelican.Application.Clients.Queries.GetClientById;
using Pelican.Domain.Entities;
using Xunit;
namespace Pelican.Application.Test.Clients.Queries;
public class GetClientByIdQueryHandlerUnitTest
{
	[Fact]
	public async void Test_If_When_Handle_Is_Called_DataLoader_Is_Called_With_Correct_Parameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<Client>>();
		var uut = new GetClientByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetClientByIdQuery getClientByIdQuery = new GetClientByIdQuery(guid);
		List<Client> resultList = new List<Client>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new Client(guid));
		//Act
		resultList.Add(await uut.Handle(getClientByIdQuery, cancellationToken));
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
}
