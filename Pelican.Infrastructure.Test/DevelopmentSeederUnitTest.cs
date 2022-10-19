using Moq;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Xunit;


namespace Pelican.Infrastructure.Persistence.Test;
public class DevelopmentSeederUnitTest
{
	private IDevelopmentSeeder uut;
	private Mock<IUnitOfWork> fakeUnitOfWork;
	private Mock<IPelicanBogusFaker> fakePelicanFaker;

	public DevelopmentSeederUnitTest()
	{
		fakeUnitOfWork = new Mock<IUnitOfWork>();
		fakePelicanFaker = new Mock<IPelicanBogusFaker>();
		uut = new DevelopmentSeeder(fakeUnitOfWork.Object, fakePelicanFaker.Object);
	}
	[Theory]
	[InlineData(1)]
	[InlineData(50)]
	public void SeedEntireDb_AllRepositoriesCreateRangedCalledWithCorrectParameterAndSaveAsyncIsCalled(int count)
	{
		//Arrange;
		var fakeAccountManagerRepository = new Mock<IGenericRepository<AccountManager>>();
		var fakeClientRepository = new Mock<IGenericRepository<Client>>();
		var fakeContactRepository = new Mock<IGenericRepository<Contact>>();
		var fakeDealRepository = new Mock<IGenericRepository<Deal>>();
		var fakeLocationRepository = new Mock<IGenericRepository<Location>>();
		var fakeSupplierRepository = new Mock<IGenericRepository<Supplier>>();
		var fakeAccountManagerDealRepository = new Mock<IGenericRepository<AccountManagerDeal>>();
		var fakeClientContactRepository = new Mock<IGenericRepository<ClientContact>>();
		var fakeDealContactRepository = new Mock<IGenericRepository<DealContact>>();
		var fakeAccountManagersCollection = new Mock<IEnumerable<AccountManager>>();
		var fakeClientsCollection = new Mock<IEnumerable<Client>>();
		var fakeContactsCollection = new Mock<IEnumerable<Contact>>();
		var fakeDealsCollection = new Mock<IEnumerable<Deal>>();
		var fakeLocationsCollection = new Mock<IEnumerable<Location>>();
		var fakeSuppliersCollection = new Mock<IEnumerable<Supplier>>();
		var fakeAccountManagerDealsCollection = new Mock<IEnumerable<AccountManagerDeal>>();
		var fakeDealContactsCollection = new Mock<IEnumerable<DealContact>>();
		var fakeClientContactsCollection = new Mock<IEnumerable<ClientContact>>();

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
		fakeUnitOfWork.Setup(x => x.AccountManagerDealRepository)
			.Returns(fakeAccountManagerDealRepository.Object);
		fakeUnitOfWork.Setup(x => x.ClientContactRepository)
			.Returns(fakeClientContactRepository.Object);
		fakeUnitOfWork.Setup(x => x.DealContactRepository)
			.Returns(fakeDealContactRepository.Object);

		fakePelicanFaker.Setup(x => x.AccountManagerFaker(It.IsAny<int>(), It.IsAny<IQueryable<Supplier>>()))
			.Returns(fakeAccountManagersCollection.Object);
		fakePelicanFaker.Setup(x => x.ClientFaker(It.IsAny<int>(), It.IsAny<IQueryable<Location>>()))
			.Returns(fakeClientsCollection.Object);
		fakePelicanFaker.Setup(x => x.ContactFaker(It.IsAny<int>()))
			.Returns(fakeContactsCollection.Object);
		fakePelicanFaker.Setup(x => x.DealFaker(It.IsAny<int>(), It.IsAny<IQueryable<Client>>()))
			.Returns(fakeDealsCollection.Object);
		fakePelicanFaker.Setup(x => x.LocationFaker(It.IsAny<int>()))
			.Returns(fakeLocationsCollection.Object);
		fakePelicanFaker.Setup(x => x.SupplierFaker(It.IsAny<int>(), It.IsAny<IQueryable<Location>>()))
			.Returns(fakeSuppliersCollection.Object);
		fakePelicanFaker.Setup(x => x.AccountManagerDealFaker(It.IsAny<IQueryable<AccountManager>>(), It.IsAny<IQueryable<Deal>>()))
			.Returns(fakeAccountManagerDealsCollection.Object);
		fakePelicanFaker.Setup(x => x.DealContactFaker(It.IsAny<IQueryable<Deal>>(), It.IsAny<IQueryable<Contact>>()))
		.Returns(fakeDealContactsCollection.Object);
		fakePelicanFaker.Setup(x => x.ClientContactFaker(It.IsAny<IQueryable<Client>>(), It.IsAny<IQueryable<Contact>>()))
		.Returns(fakeClientContactsCollection.Object);
		//Act

		uut.SeedEntireDb(count);

		//Assert
		fakeUnitOfWork.Verify(x => x.LocationRepository.CreateRange(fakeLocationsCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.AccountManagerRepository.CreateRange(fakeAccountManagersCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.ClientRepository.CreateRange(fakeClientsCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.ContactRepository.CreateRange(fakeContactsCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.DealRepository.CreateRange(fakeDealsCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.SupplierRepository.CreateRange(fakeSuppliersCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.ClientContactRepository.CreateRange(fakeClientContactsCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.DealContactRepository.CreateRange(fakeDealContactsCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.AccountManagerDealRepository.CreateRange(fakeAccountManagerDealsCollection.Object), Times.Exactly(1));
		fakeUnitOfWork.Verify(x => x.SaveAsync(), Times.Exactly(1));
	}
	[Theory]
	[InlineData(1)]
	public void SeedClients_RepositoryIsContainsNoAccountManagers_CreateRangeIsCalled(int count)
	{
		//Arrange
		var fakeAccountManagerRepository = new Mock<IGenericRepository<AccountManager>>();
		var fakeSupplier = new Mock<IQueryable<Supplier>>();
		var fakeAccountManagersCollection = new Mock<IEnumerable<AccountManager>>();

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
	public void CheckIfClientFakerIsCalledWhenSeedClientIsCalled(int count)
	{
		//Arrange
		var fakeClientRepository = new Mock<IGenericRepository<Client>>();
		var fakeLocation = new Mock<IQueryable<Location>>();
		var fakeClientsCollection = new Mock<IEnumerable<Client>>();

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
		var fakeContactRepository = new Mock<IGenericRepository<Contact>>();
		var fakeContactsCollection = new Mock<IEnumerable<Contact>>();

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
		var fakeDealRepository = new Mock<IGenericRepository<Deal>>();
		var fakeDealsCollection = new Mock<IEnumerable<Deal>>();
		var fakeClient = new Mock<IQueryable<Client>>();

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
		var fakeLocationRepository = new Mock<IGenericRepository<Location>>();
		var fakeLocationsCollection = new Mock<IEnumerable<Location>>();

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
		var fakeSupplierRepository = new Mock<IGenericRepository<Supplier>>();
		var fakeSuppliersCollection = new Mock<IEnumerable<Supplier>>();
		var fakeLocation = new Mock<IQueryable<Location>>();

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
