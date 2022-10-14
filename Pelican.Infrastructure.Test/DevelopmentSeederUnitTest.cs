using Xunit;


namespace Pelican.Infrastructure.Persistence.Test;
public class DevelopmentSeederUnitTest
{
	[Fact]
	public void CheckIfSaveIsCalledWhenSeedDbIsCalled()
	{
		////Arrange
		//var fakeUnitOfWork = new Mock<IUnitOfWork>();

		//var fakeSupplierRepository = new Mock<IGenericRepository<Supplier>>();

		//var fakePelicanFaker = new Mock<IPelicanBogusFaker>();

		//var guid = Guid.NewGuid();

		//List<Supplier> suppliers = new List<Supplier>();

		//suppliers.Add(new Supplier(guid)
		//{
		//	Email = "thismail"
		//});

		//fakeUnitOfWork.Setup(x => x.SupplierRepository)
		//	.Returns(fakeSupplierRepository.Object);
		//var supplier = new Supplier(Guid.NewGuid());
		//fakePelicanFaker.Setup(x => x.SupplierFaker(It.IsAny<int>())).Returns(suppliers);

		//Act

		//DevelopmentSeeder.SeedEntireDb(fakeUnitOfWork.Object, fakePelicanFaker.Object);

		//Assert

		//fakeUnitOfWork.Verify(x => x.SupplierRepository.CreateRange(suppliers), Times.Once());

		//fakeUnitOfWork.Verify(x => x.SaveAsync(), Times.Once());
	}
}
