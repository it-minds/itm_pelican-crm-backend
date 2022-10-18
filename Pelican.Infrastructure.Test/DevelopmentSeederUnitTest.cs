using Moq;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Xunit;


namespace Pelican.Infrastructure.Persistence.Test;
public class DevelopmentSeederUnitTest
{
	[Theory]
	[InlineData(1)]
	[InlineData(50)]
	public void CheckIfSaveIsCalledWhenSeedEntireDbIsCalled(int count)
	{
		//Arrange;
		var fakeUnitOfWork = new Mock<IUnitOfWork>();
		var fakeAccountManagerRepository = new Mock<IGenericRepository<AccountManager>>();
		var fakeClientRepository = new Mock<IGenericRepository<Client>>();
		var fakeContactRepository = new Mock<IGenericRepository<Contact>>();
		var fakeDealRepository = new Mock<IGenericRepository<Deal>>();
		var fakeLocationRepository = new Mock<IGenericRepository<Location>>();
		var fakeSupplierRepository = new Mock<IGenericRepository<Supplier>>();
		var fakePelicanFaker = new Mock<IPelicanBogusFaker>();
		var fakeAccountManagersCollection = new Mock<IEnumerable<AccountManager>>();
		var fakeClientsCollection = new Mock<IEnumerable<Client>>();
		var fakeContactsCollection = new Mock<IEnumerable<Contact>>();
		var fakeDealsCollection = new Mock<IEnumerable<Deal>>();
		var fakeLocationsCollection = new Mock<IEnumerable<Location>>();
		var fakeSuppliersCollection = new Mock<IEnumerable<Supplier>>();
		var uut = new DevelopmentSeeder(fakeUnitOfWork.Object, fakePelicanFaker.Object);

		fakeUnitOfWork.Setup(x => x.AccountManagerRepository)
			.Returns(fakeAccountManagerRepository.Object);
		fakeUnitOfWork.Setup(x => x.ClientRepository)
			.Returns(fakeClientRepository.Object);
		fakeUnitOfWork.Setup(x => x.ContactRepository)
			.Returns(fakeContactRepository.Object);
		fakeUnitOfWork.Setup(x => x.DealRepository)
			.Returns(fakeDealRepository.Object);
		fakeUnitOfWork.Setup(x => x.LocationRepository)
			.Returns(fakeLocationRepository.Object);
		fakeUnitOfWork.Setup(x => x.SupplierRepository)
		.Returns(fakeSupplierRepository.Object);

		fakePelicanFaker.Setup(x => x.AccountManagerFaker(It.IsAny<int>(), It.IsAny<IQueryable<Supplier>>())).Returns(fakeAccountManagersCollection.Object);
		fakePelicanFaker.Setup(x => x.ClientFaker(It.IsAny<int>(), It.IsAny<IQueryable<Location>>())).Returns(fakeClientsCollection.Object);
		fakePelicanFaker.Setup(x => x.ContactFaker(It.IsAny<int>())).Returns(fakeContactsCollection.Object);
		fakePelicanFaker.Setup(x => x.DealFaker(It.IsAny<int>(), It.IsAny<IQueryable<Client>>())).Returns(fakeDealsCollection.Object);
		fakePelicanFaker.Setup(x => x.LocationFaker(It.IsAny<int>())).Returns(fakeLocationsCollection.Object);
		fakePelicanFaker.Setup(x => x.SupplierFaker(It.IsAny<int>(), It.IsAny<IQueryable<Location>>())).Returns(fakeSuppliersCollection.Object);

		//Act

		uut.SeedEntireDb(count);

		//Assert
		fakeUnitOfWork.Verify(x => x.LocationRepository.CreateRange(fakeLocationsCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.AccountManagerRepository.CreateRange(fakeAccountManagersCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.ClientRepository.CreateRange(fakeClientsCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.ContactRepository.CreateRange(fakeContactsCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.DealRepository.CreateRange(fakeDealsCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.SupplierRepository.CreateRange(fakeSuppliersCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.SaveAsync(), Times.Exactly(1));
	}
	[Theory]
	[InlineData(1)]
	[InlineData(50)]
	public void CheckIfAccountManagerIsCalledWhenSeedAccountManagerIsCalled(int count)
	{
		//Arrange
		var fakeUnitOfWork = new Mock<IUnitOfWork>();
		var fakeAccountManagerRepository = new Mock<IGenericRepository<AccountManager>>();
		var fakePelicanFaker = new Mock<IPelicanBogusFaker>();
		var fakeSupplier = new Mock<IQueryable<Supplier>>();
		var fakeAccountManagersCollection = new Mock<IEnumerable<AccountManager>>();
		var uut = new DevelopmentSeeder(fakeUnitOfWork.Object, fakePelicanFaker.Object);

		fakePelicanFaker.Setup(x => x.AccountManagerFaker(It.IsAny<int>(), It.IsAny<IQueryable<Supplier>>())).Returns(fakeAccountManagersCollection.Object);
		fakeUnitOfWork.Setup(x => x.AccountManagerRepository)
			.Returns(fakeAccountManagerRepository.Object);
		fakePelicanFaker.Setup(x => x.SupplierFaker(It.IsAny<int>(), It.IsAny<IQueryable<Location>>())).Returns(fakeSupplier.Object);
		//Act
		var result = uut.SeedAccountManagers(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeSupplier.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.AccountManagerRepository.CreateRange(fakeAccountManagersCollection.Object), Times.Once());
		fakePelicanFaker.Verify(x => x.AccountManagerFaker(count, fakeSupplier.Object));
	}
	[Theory]
	[InlineData(1)]
	[InlineData(50)]
	public void CheckIfClientFakerIsCalledWhenSeedClientIsCalled(int count)
	{
		//Arrange
		var fakeUnitOfWork = new Mock<IUnitOfWork>();
		var fakeClientRepository = new Mock<IGenericRepository<Client>>();
		var fakePelicanFaker = new Mock<IPelicanBogusFaker>();
		var fakeLocation = new Mock<IQueryable<Location>>();
		var fakeClientsCollection = new Mock<IEnumerable<Client>>();
		var uut = new DevelopmentSeeder(fakeUnitOfWork.Object, fakePelicanFaker.Object);

		fakePelicanFaker.Setup(x => x.ClientFaker(It.IsAny<int>(), It.IsAny<IQueryable<Location>>())).Returns(fakeClientsCollection.Object);
		fakeUnitOfWork.Setup(x => x.ClientRepository)
			.Returns(fakeClientRepository.Object);
		//Act
		var result = uut.SeedClients(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeLocation.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.ClientRepository.CreateRange(fakeClientsCollection.Object), Times.Once());
		fakePelicanFaker.Verify(x => x.ClientFaker(count, fakeLocation.Object), Times.Once());
	}
	[Theory]
	[InlineData(1)]
	[InlineData(50)]
	public void CheckIfContactFakerIsCalledWhenSeedContactIsCalled(int count)
	{
		//Arrange
		var fakeUnitOfWork = new Mock<IUnitOfWork>();
		var fakeContactRepository = new Mock<IGenericRepository<Contact>>();
		var fakePelicanFaker = new Mock<IPelicanBogusFaker>();
		var fakeContactsCollection = new Mock<IEnumerable<Contact>>();
		var uut = new DevelopmentSeeder(fakeUnitOfWork.Object, fakePelicanFaker.Object);

		fakePelicanFaker.Setup(x => x.ContactFaker(It.IsAny<int>())).Returns(fakeContactsCollection.Object);
		fakeUnitOfWork.Setup(x => x.ContactRepository)
			.Returns(fakeContactRepository.Object);
		//Act
		var result = uut.SeedContacts(fakeUnitOfWork.Object, fakePelicanFaker.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.ContactRepository.CreateRange(fakeContactsCollection.Object), Times.Once());
		fakePelicanFaker.Verify(x => x.ContactFaker(count), Times.Once());
	}
	[Theory]
	[InlineData(1)]
	[InlineData(50)]
	public void CheckIfSaveIsCalledWhenSeedDealIsCalled(int count)
	{
		//Arrange
		var fakeUnitOfWork = new Mock<IUnitOfWork>();
		var fakeDealRepository = new Mock<IGenericRepository<Deal>>();
		var fakePelicanFaker = new Mock<IPelicanBogusFaker>();
		var fakeDealsCollection = new Mock<IEnumerable<Deal>>();
		var fakeClient = new Mock<IQueryable<Client>>();
		var uut = new DevelopmentSeeder(fakeUnitOfWork.Object, fakePelicanFaker.Object);

		fakePelicanFaker.Setup(x => x.DealFaker(It.IsAny<int>(), fakeClient.Object)).Returns(fakeDealsCollection.Object);
		fakeUnitOfWork.Setup(x => x.DealRepository)
			.Returns(fakeDealRepository.Object);
		//Act
		var result = uut.SeedDeals(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeClient.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.DealRepository.CreateRange(fakeDealsCollection.Object), Times.Once());
		fakePelicanFaker.Verify(x => x.DealFaker(count, fakeClient.Object), Times.Once());
	}
	[Theory]
	[InlineData(1)]
	[InlineData(50)]
	public void CheckIfSaveIsCalledWhenSeedLocationIsCalled(int count)
	{
		//Arrange
		var fakeUnitOfWork = new Mock<IUnitOfWork>();
		var fakeLocationRepository = new Mock<IGenericRepository<Location>>();
		var fakePelicanFaker = new Mock<IPelicanBogusFaker>();
		var fakeLocationsCollection = new Mock<IEnumerable<Location>>();
		var uut = new DevelopmentSeeder(fakeUnitOfWork.Object, fakePelicanFaker.Object);

		fakePelicanFaker.Setup(x => x.LocationFaker(It.IsAny<int>())).Returns(fakeLocationsCollection.Object);
		fakeUnitOfWork.Setup(x => x.LocationRepository)
			.Returns(fakeLocationRepository.Object);
		//Act
		var result = uut.SeedLocations(fakeUnitOfWork.Object, fakePelicanFaker.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.LocationRepository.CreateRange(fakeLocationsCollection.Object), Times.Once());
		fakePelicanFaker.Verify(x => x.LocationFaker(count), Times.Once());
	}
	[Theory]
	[InlineData(1)]
	[InlineData(50)]
	public void CheckIfSaveIsCalledWhenSeedsupplierIsCalled(int count)
	{
		//Arrange
		var fakeUnitOfWork = new Mock<IUnitOfWork>();
		var fakeSupplierRepository = new Mock<IGenericRepository<Supplier>>();
		var fakePelicanFaker = new Mock<IPelicanBogusFaker>();
		var fakeSuppliersCollection = new Mock<IEnumerable<Supplier>>();
		var fakeLocation = new Mock<IQueryable<Location>>();
		var uut = new DevelopmentSeeder(fakeUnitOfWork.Object, fakePelicanFaker.Object);


		fakePelicanFaker.Setup(x => x.SupplierFaker(It.IsAny<int>(), fakeLocation.Object)).Returns(fakeSuppliersCollection.Object);
		fakeUnitOfWork.Setup(x => x.SupplierRepository)
			.Returns(fakeSupplierRepository.Object);
		//Act
		var result = uut.SeedSuppliers(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeLocation.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.SupplierRepository.CreateRange(fakeSuppliersCollection.Object), Times.Once());
		fakePelicanFaker.Verify(x => x.SupplierFaker(count, fakeLocation.Object), Times.Once());
	}

}
