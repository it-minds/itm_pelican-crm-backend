using Moq;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.Suppliers.Queries.GetSuppliers;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.Suppliers.Queries;
public class GetSuppliersQueryHandlerUnitTest
{
	private GetSuppliersQueryHandler uut;
	[Fact]
	public async void Test_If_When_Handle_Is_Called_Repository_Is_Called()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var supplierRepositoryMock = new Mock<IGenericRepository<Supplier>>();
		unitOfWorkMock.Setup(x => x.SupplierRepository).Returns(supplierRepositoryMock.Object);
		uut = new GetSuppliersQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetSuppliersQuery accountManagersQuery = new GetSuppliersQuery();
		//Act
		_ = await uut.Handle(accountManagersQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.SupplierRepository.FindAllWithIncludes(), Times.Once());
	}
}
