using Moq;
using Pelican.Application.Abstractions.Data.DataLoaders;
using Pelican.Application.Suppliers.Queries.GetSupplierById;
using Pelican.Domain.Entities;
using Xunit;
namespace Pelican.Application.Test.Suppliers.Queries;
public class GetSupplierByIdQueryHandlerUnitTest
{
	[Fact]
	public async void Test_If_When_Handle_Is_Called_DataLoader_Is_Called_With_Correct_Parameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<Supplier>>();
		var uut = new GetSupplierByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetSupplierByIdQuery getSupplierByIdQuery = new GetSupplierByIdQuery(guid);
		List<Supplier> resultList = new List<Supplier>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new Supplier(guid));
		//Act
		resultList.Add(await uut.Handle(getSupplierByIdQuery, cancellationToken));
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
}
