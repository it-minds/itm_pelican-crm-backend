using Moq;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Application.Deals.Queries.GetDealById;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.Deals.Queries;
public class GetDealByIdQueryHandlerUnitTest
{
	private GetDealByIdQueryHandler uut;
	[Fact]
	public async void Test_If_When_Handle_Is_Called_DataLoader_Is_Called_With_Correct_Parameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<Deal>>();
		uut = new GetDealByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetDealByIdQuery getDealByIdQuery = new GetDealByIdQuery(guid);
		List<Deal> resultList = new List<Deal>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new Deal(guid));
		//Act
		resultList.Add(await uut.Handle(getDealByIdQuery, cancellationToken));
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
}
