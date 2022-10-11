using Moq;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Application.Suppliers.Queries.GetSupplierById;
using Pelican.Domain.Entities;
using Xunit;
namespace Pelican.Application.Test;
public class GetSupplierByIdQueryHandlerUnitTest
{
	private GetSupplierByIdQueryHandler uut;
	[Fact]
	public async void TestIfWhenHandleIsCalledDataLoaderIsCalledWithCorrectParameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<Supplier>>();
		uut = new GetSupplierByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetSupplierByIdQuery getSupplierByIdQuery = new GetSupplierByIdQuery(guid);
		List<Supplier> resultList = new List<Supplier>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new Supplier
		{
			Id = guid
		});
		//Act
		resultList.Add(await uut.Handle(getSupplierByIdQuery, cancellationToken));
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
	[Fact]
	public async void TestIfWhenHandleIsCalledMultipleTimesDataLoaderIsCalledWithCorrectParametersMultipleTimes()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<Supplier>>();
		uut = new GetSupplierByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetSupplierByIdQuery getSupplierByIdQuery = new GetSupplierByIdQuery(guid);
		List<Supplier> resultList = new List<Supplier>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new Supplier
		{
			Id = guid
		});
		//Act
		for (int i = 0; i < 50; i++)
		{
			resultList.Add(await uut.Handle(getSupplierByIdQuery, cancellationToken));
		}
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Exactly(50));
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
}
