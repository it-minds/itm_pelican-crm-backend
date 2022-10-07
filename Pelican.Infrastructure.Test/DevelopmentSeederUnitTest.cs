using Moq;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Xunit;


namespace Pelican.Infrastructure.Persistence.Test;
public class DevelopmentSeederUnitTest
{
	[Fact]
	public void CheckIfSaveIsCalledWhenSeedDbIsCalled()
	{
		//Arrange
		var fakePelicanContext = new Mock<IPelicanContext>();
		var mockDbSet = new Mock<Microsoft.EntityFrameworkCore.DbSet<Supplier>>();
		var fakePelicanFaker = new Mock<IPelicanFaker>();
		var guid = Guid.NewGuid();
		List<Supplier> suppliers = new List<Supplier>();
		suppliers.Add(new Supplier()
		{
			Email = "thismail"
		});
		//Act
		fakePelicanContext.Setup(x => x.Suppliers)
			.Returns((Microsoft.EntityFrameworkCore.DbSet<Supplier>)mockDbSet
			.As<IQueryable<Supplier>>().Object);
		fakePelicanFaker.Setup(x => x.SupplierFaker(It.IsAny<int>())).Returns(suppliers);
		DevelopmentSeeder.SeedEntireDb(fakePelicanContext.Object, fakePelicanFaker.Object);
		//Assert
		fakePelicanContext.Verify(x => x.SaveChanges(), Times.Exactly(1));
		fakePelicanContext.Verify(x => x.Suppliers.AddRange(suppliers), Times.Once());
	}
}
