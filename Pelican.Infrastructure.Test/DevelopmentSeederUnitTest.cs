﻿using Moq;
using Pelican.Application.Common.Interfaces;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Xunit;


namespace Pelican.Infrastructure.Persistence.Test;
public class DevelopmentSeederUnitTest
{
	[Fact]
	public async Task CheckIfSaveIsCalledWhenSeedDbIsCalled()
	{
		//Arrange
		var fakeUnitOfWork = new Mock<IUnitOfWork>();

		var fakeSupplierRepository = new Mock<IGenericRepository<Supplier>>();

		var fakePelicanFaker = new Mock<IPelicanBogusFaker>();

		var guid = Guid.NewGuid();

		List<Supplier> suppliers = new List<Supplier>();

		suppliers.Add(new Supplier(guid)
		{
			Email = "thismail"
		});

		fakeUnitOfWork.Setup(x => x.SupplierRepository)
			.Returns(fakeSupplierRepository.Object);

		fakePelicanFaker.Setup(x => x.SupplierFaker(It.IsAny<int>())).Returns(suppliers);

		//Act

		await DevelopmentSeeder.SeedEntireDb(fakeUnitOfWork.Object, fakePelicanFaker.Object);

		//Assert

		fakeUnitOfWork.Verify(x => x.SupplierRepository.CreateRangeAsync(suppliers, CancellationToken.None), Times.Once());

		fakeUnitOfWork.Verify(x => x.Save(), Times.Once());
	}
}