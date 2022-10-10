using Moq;
using Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Xunit;
namespace Pelican.Application.Test;
public class GetAccountManagerByIdQueryHandlerUnitTest
{
	private GetAccountManagerByIdQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledDataLoaderIsCalledWithCorrectParameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IAccountManagerByIdDataLoader>();
		uut = new GetAccountManagerByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetAccountManagerByIdQuery getAccountManagerByIdQuery = new GetAccountManagerByIdQuery(guid);
		//Act
		uut.Handle(getAccountManagerByIdQuery, cancellationToken);
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
	}
	[Fact]
	public void TestIfWhenHandleIsCalledMultipleTimesDataLoaderIsCalledWithCorrectParametersMultipleTimes()
	{
		//Arrange
		var dataLoaderMock = new Mock<IAccountManagerByIdDataLoader>();
		uut = new GetAccountManagerByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetAccountManagerByIdQuery getAccountManagerByIdQuery = new GetAccountManagerByIdQuery(guid);
		//Act
		for (int i = 0; i < 50; i++)
		{
			uut.Handle(getAccountManagerByIdQuery, cancellationToken);

		}
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Exactly(50));
	}
}
