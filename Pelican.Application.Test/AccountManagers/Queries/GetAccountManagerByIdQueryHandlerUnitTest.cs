using Moq;
using Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.AccountManagers.Queries;
public class GetAccountManagerByIdQueryHandlerUnitTest
{
	private GetAccountManagerByIdQueryHandler uut;
	private Mock<IGenericDataLoader<AccountManager>> dataLoaderMock;

	public GetAccountManagerByIdQueryHandlerUnitTest()
	{
		dataLoaderMock = new Mock<IGenericDataLoader<AccountManager>>();
		uut = new GetAccountManagerByIdQueryHandler(dataLoaderMock.Object);
	}

	[Fact]
	public async void Test_If_When_Handle_Is_Called_DataLoader_Is_Called_With_Correct_Parameters()
	{
		//Arrange
		var cancellationToken = new CancellationToken();
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

}
