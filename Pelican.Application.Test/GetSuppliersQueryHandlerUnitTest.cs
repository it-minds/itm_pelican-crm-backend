using Moq;
using Pelican.Application.Suppliers.Queries.GetSuppliers;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Xunit;
namespace Pelican.Application.Test;
public class GetSuppliersQueryHandlerUnitTest
{
	private GetSuppliersQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledRepositoryIsCalled()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var supplierRepositoryMock = new Mock<IRepositoryBase<Supplier>>();
		unitOfWorkMock.Setup(x => x.SupplierRepository).Returns(supplierRepositoryMock.Object);
		uut = new GetSuppliersQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetSuppliersQuery accountManagersQuery = new GetSuppliersQuery();
		//Act
		_ = uut.Handle(accountManagersQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.SupplierRepository.FindAll(), Times.Once());
	}
	[Fact]
	public void TestIfWhenHandleIsCalledMultipleTimesRepositoryIsCalledMultipleTimes()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var supplierRepositoryMock = new Mock<IRepositoryBase<Supplier>>();
		unitOfWorkMock.Setup(x => x.SupplierRepository).Returns(supplierRepositoryMock.Object);
		uut = new GetSuppliersQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetSuppliersQuery suppliersQuery = new GetSuppliersQuery();
		//Act
		for (int i = 0; i < 50; i++)
		{
			_ = uut.Handle(suppliersQuery, cancellationToken);
		}

		//Assert
		unitOfWorkMock.Verify(x => x.SupplierRepository.FindAll(), Times.Exactly(50));
	}
}
