using Moq;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Application.Suppliers.Queries.GetSupplierById;
using Xunit;
namespace Pelican.Application.Test;
public class GetSupplierByIdQueryHandlerUnitTest
{
	private GetSupplierByIdQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledDataLoaderIsCalledWithCorrectParameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<ISupplierByIdDataLoader>();
		uut = new GetSupplierByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetSupplierByIdQuery getSupplierByIdQuery = new GetSupplierByIdQuery(guid);
		//Act
		uut.Handle(getSupplierByIdQuery, cancellationToken);
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
	}
}
