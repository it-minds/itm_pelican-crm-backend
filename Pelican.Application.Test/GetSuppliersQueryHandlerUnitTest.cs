﻿using Moq;
using Pelican.Application.Suppliers.Queries.GetSuppliers;
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
		var supplierRepositoryMock = new Mock<ISupplierRepository>();
		unitOfWorkMock.Setup(x => x.SupplierRepository).Returns(supplierRepositoryMock.Object);
		uut = new GetSuppliersQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetSuppliersQuery accountManagersQuery = new GetSuppliersQuery();
		//Act
		uut.Handle(accountManagersQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.SupplierRepository.FindAll(), Times.Once());
	}
}
