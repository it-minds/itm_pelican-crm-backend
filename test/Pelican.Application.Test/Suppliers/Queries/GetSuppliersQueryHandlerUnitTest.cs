using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Suppliers.Queries.GetSuppliers;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.Suppliers.Queries;
public class GetSuppliersQueryHandlerUnitTest
{
	[Fact]
	public async void Test_If_When_Handle_Is_Called_Repository_Is_Called()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var supplierRepositoryMock = new Mock<IGenericRepository<Supplier>>();
		unitOfWorkMock.Setup(x => x.SupplierRepository).Returns(supplierRepositoryMock.Object);
		var uut = new GetSuppliersQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetSuppliersQuery accountManagersQuery = new GetSuppliersQuery();
		//Act
		_ = await uut.Handle(accountManagersQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.SupplierRepository.FindAll(), Times.Once());
	}
}
