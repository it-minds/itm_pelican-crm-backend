using Moq;
using Pelican.Application.Abstractions.Data.DataLoaders;
using Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.AccountManagers.Queries;
public class GetAccountManagerByIdQueryHandlerUnitTest
{
	[Fact]
	public async void Test_If_When_Handle_Is_Called_DataLoader_Is_Called_With_Correct_Parameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<AccountManager>>();
		var uut = new GetAccountManagerByIdQueryHandler(dataLoaderMock.Object);
		var cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetAccountManagerByIdQuery getAccountManagerByIdQuery = new GetAccountManagerByIdQuery(guid);
		List<AccountManager> resultList = new List<AccountManager>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new AccountManager() { Id = guid });
		//Act
		resultList.Add(await uut.Handle(getAccountManagerByIdQuery, cancellationToken));
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
}
