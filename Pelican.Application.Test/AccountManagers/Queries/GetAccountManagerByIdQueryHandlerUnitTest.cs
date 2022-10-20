using Moq;
using Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.AccountManagers.Queries;
public class GetAccountManagerByIdQueryHandlerUnitTest
{
	private GetAccountManagerByIdQueryHandler uut;
	[Fact]
	public async void TestIfWhenHandleIsCalledDataLoaderIsCalledWithCorrectParameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<AccountManager>>();
		uut = new GetAccountManagerByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetAccountManagerByIdQuery getAccountManagerByIdQuery = new GetAccountManagerByIdQuery(guid);
		List<AccountManager> resultList = new List<AccountManager>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new AccountManager(guid));
		//Act
		resultList.Add(await uut.Handle(getAccountManagerByIdQuery, cancellationToken));
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
	[Fact]
	public async void TestIfWhenHandleIsCalledMultipleTimesDataLoaderIsCalledWithCorrectParametersMultipleTimes()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<AccountManager>>();
		uut = new GetAccountManagerByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetAccountManagerByIdQuery getAccountManagerByIdQuery = new GetAccountManagerByIdQuery(guid);
		List<AccountManager> resultList = new List<AccountManager>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new AccountManager(guid));
		//Act
		for (int i = 0; i < 50; i++)
		{
			resultList.Add(await uut.Handle(getAccountManagerByIdQuery, cancellationToken));
		}
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Exactly(50));
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
}
