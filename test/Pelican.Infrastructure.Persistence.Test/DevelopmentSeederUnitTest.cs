using System.Collections.ObjectModel;
using Moq;
using Pelican.Application.Abstractions.Data;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Infrastructure.Persistence.Test;
public class DevelopmentSeederUnitTest
{
	private readonly IDevelopmentSeeder uut;
	private readonly Mock<IUnitOfWork> fakeUnitOfWork;
	private readonly Mock<IPelicanBogusFaker> fakePelicanFaker;
	private readonly CancellationToken cancellation;

	public DevelopmentSeederUnitTest()
	{
		fakeUnitOfWork = new Mock<IUnitOfWork>();
		fakePelicanFaker = new Mock<IPelicanBogusFaker>();
		uut = new DevelopmentSeeder(fakeUnitOfWork.Object, fakePelicanFaker.Object);
		cancellation = new CancellationToken();
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
		fakePelicanFaker.Setup(x => x.ContactFaker(It.IsAny<int>(), It.IsAny<IQueryable<AccountManager>>()))
			.Returns(fakeContactsCollection.Object);
		fakePelicanFaker.Setup(x => x.DealFaker(It.IsAny<int>(), It.IsAny<IQueryable<Client>>(), It.IsAny<IQueryable<AccountManager>>()))
			.Returns(fakeDealsCollection.Object);
		fakePelicanFaker.Setup(x => x.LocationFaker(It.IsAny<int>(), It.IsAny<IQueryable<Supplier>>()))
			.Returns(fakeLocationsCollection.Object);
		fakePelicanFaker.Setup(x => x.SupplierFaker(It.IsAny<int>()))
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
		fakeUnitOfWork.Verify(x => x.SaveAsync(cancellation), Times.Exactly(1));
	}
	[Theory]
	[InlineData(1)]
	public void SeedAccountManagers_RepositoryContainsNoAccountManagers_CreateRange_And_AccountManagerFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeAccountManagerRepository = new Mock<IGenericRepository<AccountManager>>();
		var fakeSupplier = new Mock<IQueryable<Supplier>>();
		var fakeAccountManagersCollection = new Mock<IEnumerable<AccountManager>>();

		fakePelicanFaker.Setup(x => x.AccountManagerFaker(It.IsAny<int>(), It.IsAny<IQueryable<Supplier>>())).Returns(fakeAccountManagersCollection.Object);
		fakeUnitOfWork.Setup(x => x.AccountManagerRepository)
			.Returns(fakeAccountManagerRepository.Object);
		fakePelicanFaker.Setup(x => x.SupplierFaker(It.IsAny<int>())).Returns(fakeSupplier.Object);
		//Act
		var result = uut.SeedAccountManagers(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeSupplier.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.AccountManagerRepository.CreateRangeAsync(fakeAccountManagersCollection.Object, cancellation), Times.Once());
		fakePelicanFaker.Verify(x => x.AccountManagerFaker(count, fakeSupplier.Object));
	}
	[Theory]
	[InlineData(1)]
	public void SeedClient_RepositoryContainsNoClients_CreateRange_And_ClientFaker_IsCalledWithCorrectParameter(int count)
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
		fakeUnitOfWork.Verify(x => x.ClientRepository.CreateRangeAsync(fakeClientsCollection.Object, cancellation), Times.Once());
		fakePelicanFaker.Verify(x => x.ClientFaker(count, fakeLocation.Object), Times.Once());
	}
	[Theory]
	[InlineData(1)]
	public void SeedContacts_RepositoryContainsNoContacts_CreateRange_And_ContactFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeContactRepository = new Mock<IGenericRepository<Contact>>();
		var fakeContactsCollection = new Mock<IEnumerable<Contact>>();
		var fakeAccountManager = new Mock<IQueryable<AccountManager>>();

		fakePelicanFaker.Setup(x => x.ContactFaker(It.IsAny<int>(), It.IsAny<IQueryable<AccountManager>>())).Returns(fakeContactsCollection.Object);
		fakeUnitOfWork.Setup(x => x.ContactRepository)
			.Returns(fakeContactRepository.Object);
		//Act
		var result = uut.SeedContacts(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeAccountManager.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.ContactRepository.CreateRangeAsync(fakeContactsCollection.Object, cancellation), Times.Once());
		fakePelicanFaker.Verify(x => x.ContactFaker(count, fakeAccountManager.Object), Times.Once());
	}
	[Theory]
	[InlineData(1)]
	public void SeedDeals_RepositoryContainsNoDeals_CreateRange_And_DealFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeDealRepository = new Mock<IGenericRepository<Deal>>();
		var fakeDealsCollection = new Mock<IEnumerable<Deal>>();
		var fakeClient = new Mock<IQueryable<Client>>();
		var fakeAccountManager = new Mock<IQueryable<AccountManager>>();

		fakePelicanFaker.Setup(x => x.DealFaker(It.IsAny<int>(), It.IsAny<IQueryable<Client>>(), It.IsAny<IQueryable<AccountManager>>())).Returns(fakeDealsCollection.Object);
		fakeUnitOfWork.Setup(x => x.DealRepository)
			.Returns(fakeDealRepository.Object);
		//Act
		var result = uut.SeedDeals(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeClient.Object, fakeAccountManager.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.DealRepository.CreateRangeAsync(fakeDealsCollection.Object, cancellation), Times.Once());
		fakePelicanFaker.Verify(x => x.DealFaker(count, fakeClient.Object, fakeAccountManager.Object), Times.Once());
	}
	[Theory]
	[InlineData(1)]
	public void SeedLocations_RepositoryContainsNoLocations_CreateRange_And_LocationFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeLocationRepository = new Mock<IGenericRepository<Location>>();
		var fakeLocationsCollection = new Mock<IEnumerable<Location>>();
		var fakeSupplier = new Mock<IQueryable<Supplier>>();
		fakePelicanFaker.Setup(x => x.LocationFaker(It.IsAny<int>(), It.IsAny<IQueryable<Supplier>>())).Returns(fakeLocationsCollection.Object);
		fakeUnitOfWork.Setup(x => x.LocationRepository)
			.Returns(fakeLocationRepository.Object);
		//Act
		var result = uut.SeedLocations(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeSupplier.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.LocationRepository.CreateRangeAsync(fakeLocationsCollection.Object, cancellation), Times.Once());
		fakePelicanFaker.Verify(x => x.LocationFaker(count, fakeSupplier.Object), Times.Once());
	}
	[Theory]
	[InlineData(1)]
	public void SeedSupplier_RepositoryContainsNoSuppliers_CreateRange_And_SupplierFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeSupplierRepository = new Mock<IGenericRepository<Supplier>>();
		var fakeSuppliersCollection = new Mock<IEnumerable<Supplier>>();
		fakePelicanFaker.Setup(x => x.SupplierFaker(It.IsAny<int>())).Returns(fakeSuppliersCollection.Object);
		fakeUnitOfWork.Setup(x => x.SupplierRepository)
			.Returns(fakeSupplierRepository.Object);
		//Act
		var result = uut.SeedSuppliers(fakeUnitOfWork.Object, fakePelicanFaker.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.SupplierRepository.CreateRangeAsync(fakeSuppliersCollection.Object, cancellation), Times.Once());
		fakePelicanFaker.Verify(x => x.SupplierFaker(count), Times.Once());
	}
	[Fact]
	public void SeedAccountManagerDeals_RepositoryContainsNoAccountManagerDeals_CreateRange_And_AccountManagerDealFaker_IsCalledWithCorrectParameter()
	{
		//Arrange
		var fakeAccountManagerDealRepository = new Mock<IGenericRepository<AccountManagerDeal>>();
		var fakeAccountManagersCollection = new Mock<IEnumerable<AccountManagerDeal>>();
		var fakeAccountManager = new Mock<IQueryable<AccountManager>>();
		var fakeDeal = new Mock<IQueryable<Deal>>();
		fakePelicanFaker.Setup(x => x.AccountManagerDealFaker(It.IsAny<IQueryable<AccountManager>>(), It.IsAny<IQueryable<Deal>>())).Returns(fakeAccountManagersCollection.Object);
		fakeUnitOfWork.Setup(x => x.AccountManagerDealRepository)
			.Returns(fakeAccountManagerDealRepository.Object);
		//Act
		var result = uut.SeedAccountManagerDeals(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeAccountManager.Object, fakeDeal.Object);
		//Assert
		fakeUnitOfWork.Verify(x => x.AccountManagerDealRepository.CreateRangeAsync(fakeAccountManagersCollection.Object, cancellation), Times.Once());
		fakePelicanFaker.Verify(x => x.AccountManagerDealFaker(fakeAccountManager.Object, fakeDeal.Object), Times.Once());
	}
	[Fact]
	public void SeedClientContacts_RepositoryContainsNoClientContacts_CreateRange_And_ClientContactFaker_IsCalledWithCorrectParameter()
	{
		//Arrange
		var fakeClientContactRepository = new Mock<IGenericRepository<ClientContact>>();
		var fakeClientContactsCollection = new Mock<IEnumerable<ClientContact>>();
		var fakeClient = new Mock<IQueryable<Client>>();
		var fakeContact = new Mock<IQueryable<Contact>>();
		fakePelicanFaker.Setup(x => x.ClientContactFaker(It.IsAny<IQueryable<Client>>(), It.IsAny<IQueryable<Contact>>())).Returns(fakeClientContactsCollection.Object);
		fakeUnitOfWork.Setup(x => x.ClientContactRepository)
			.Returns(fakeClientContactRepository.Object);
		//Act
		var result = uut.SeedClientContacts(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeClient.Object, fakeContact.Object);
		//Assert
		fakeUnitOfWork.Verify(x => x.ClientContactRepository.CreateRangeAsync(fakeClientContactsCollection.Object, cancellation), Times.Once());
		fakePelicanFaker.Verify(x => x.ClientContactFaker(fakeClient.Object, fakeContact.Object), Times.Once());
	}
	[Fact]
	public void SeedDealContacts_RepositoryContainsNoDealContacts_CreateRange_And_DealContactFaker_IsCalledWithCorrectParameter()
	{
		//Arrange
		var fakeDealContactRepository = new Mock<IGenericRepository<DealContact>>();
		var fakeDealContactsCollection = new Mock<IEnumerable<DealContact>>();
		var fakeDeal = new Mock<IQueryable<Deal>>();
		var fakeContact = new Mock<IQueryable<Contact>>();
		fakePelicanFaker.Setup(x => x.DealContactFaker(It.IsAny<IQueryable<Deal>>(), It.IsAny<IQueryable<Contact>>())).Returns(fakeDealContactsCollection.Object);
		fakeUnitOfWork.Setup(x => x.DealContactRepository)
			.Returns(fakeDealContactRepository.Object);
		//Act
		var result = uut.SeedDealContacts(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeDeal.Object, fakeContact.Object);
		//Assert
		fakeUnitOfWork.Verify(x => x.DealContactRepository.CreateRangeAsync(fakeDealContactsCollection.Object, cancellation), Times.Once());
		fakePelicanFaker.Verify(x => x.DealContactFaker(fakeDeal.Object, fakeContact.Object), Times.Once());
	}
	[Theory]
	[InlineData(1)]
	public void SeedAccountManagers_RepositoryContainsAccountManagers_CreateRange_And_AccountManagerFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeAccountManagerRepository = new Mock<IGenericRepository<AccountManager>>();
		var fakeSupplier = new Mock<IQueryable<Supplier>>();
		ICollection<AccountManager> fakeAccountManager = new Collection<AccountManager>();
		fakeAccountManager.Add(new AccountManager());
		var fakeAccountManagersCollection = new Mock<IEnumerable<AccountManager>>();
		fakeAccountManagerRepository.Setup(x => x.FindAll()).Returns(fakeAccountManager.AsQueryable());
		fakePelicanFaker.Setup(x => x.AccountManagerFaker(It.IsAny<int>(), It.IsAny<IQueryable<Supplier>>()))
			.Returns(fakeAccountManagersCollection.Object);
		fakeUnitOfWork.Setup(x => x.AccountManagerRepository)
			.Returns(fakeAccountManagerRepository.Object);
		fakePelicanFaker.Setup(x => x.SupplierFaker(It.IsAny<int>())).Returns(fakeSupplier.Object);
		//Act
		var result = uut.SeedAccountManagers(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeSupplier.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.AccountManagerRepository.CreateRangeAsync(fakeAccountManagersCollection.Object, cancellation), Times.Never());
		fakePelicanFaker.Verify(x => x.AccountManagerFaker(count, fakeSupplier.Object), Times.Never());
		Assert.Equal(fakeAccountManager, result);
	}
	[Theory]
	[InlineData(1)]
	public void SeedAccountManagerDeals_RepositoryContainsAccountManagerDeals_CreateRange_And_AccountManagerDealFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeAccountManagerDealRepository = new Mock<IGenericRepository<AccountManagerDeal>>();
		var fakeAccountManager = new Mock<IQueryable<AccountManager>>();
		var fakeDeal = new Mock<IQueryable<Deal>>();
		ICollection<AccountManagerDeal> fakeAccountManagerDeal = new Collection<AccountManagerDeal>();
		fakeAccountManagerDeal.Add(new AccountManagerDeal());
		var fakeAccountManagerDealsCollection = new Mock<IEnumerable<AccountManagerDeal>>();
		fakeAccountManagerDealRepository.Setup(x => x.FindAll()).Returns(fakeAccountManagerDeal.AsQueryable());
		fakePelicanFaker.Setup(x => x.AccountManagerDealFaker(It.IsAny<IQueryable<AccountManager>>(), It.IsAny<IQueryable<Deal>>()))
			.Returns(fakeAccountManagerDealsCollection.Object);
		fakeUnitOfWork.Setup(x => x.AccountManagerDealRepository)
			.Returns(fakeAccountManagerDealRepository.Object);
		//Act
		var result = uut.SeedAccountManagerDeals(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeAccountManager.Object, fakeDeal.Object);
		//Assert
		fakeUnitOfWork.Verify(x => x.AccountManagerDealRepository.CreateRangeAsync(fakeAccountManagerDealsCollection.Object, cancellation), Times.Never());
		fakePelicanFaker.Verify(x => x.AccountManagerDealFaker(fakeAccountManager.Object, fakeDeal.Object), Times.Never());
		Assert.Equal(fakeAccountManagerDeal, result);
	}
	[Theory]
	[InlineData(1)]
	public void SeedClients_RepositoryContainsClients_CreateRange_And_ClientFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeClientRepository = new Mock<IGenericRepository<Client>>();
		var fakeLocation = new Mock<IQueryable<Location>>();
		ICollection<Client> fakeClient = new Collection<Client>();
		fakeClient.Add(new Client());
		var fakeClientsCollection = new Mock<IEnumerable<Client>>();
		fakeClientRepository.Setup(x => x.FindAll()).Returns(fakeClient.AsQueryable());
		fakePelicanFaker.Setup(x => x.ClientFaker(count, It.IsAny<IQueryable<Location>>()))
			.Returns(fakeClientsCollection.Object);
		fakeUnitOfWork.Setup(x => x.ClientRepository)
			.Returns(fakeClientRepository.Object);
		//Act
		var result = uut.SeedClients(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeLocation.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.ClientRepository.CreateRangeAsync(fakeClientsCollection.Object, cancellation), Times.Never());
		fakePelicanFaker.Verify(x => x.ClientFaker(count, fakeLocation.Object), Times.Never());
		Assert.Equal(fakeClient, result);
	}
	[Theory]
	[InlineData(1)]
	public void SeedClientContacts_RepositoryContainsClientContacts_CreateRange_And_ClientContactFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeClientContactRepository = new Mock<IGenericRepository<ClientContact>>();
		var fakeClient = new Mock<IQueryable<Client>>();
		var fakeContact = new Mock<IQueryable<Contact>>();
		ICollection<ClientContact> fakeClientContact = new Collection<ClientContact>();
		fakeClientContact.Add(new ClientContact());
		var fakeClientContactsCollection = new Mock<IEnumerable<ClientContact>>();
		fakeClientContactRepository.Setup(x => x.FindAll()).Returns(fakeClientContact.AsQueryable());
		fakePelicanFaker.Setup(x => x.ClientContactFaker(It.IsAny<IQueryable<Client>>(), It.IsAny<IQueryable<Contact>>()))
			.Returns(fakeClientContactsCollection.Object);
		fakeUnitOfWork.Setup(x => x.ClientContactRepository)
			.Returns(fakeClientContactRepository.Object);
		//Act
		var result = uut.SeedClientContacts(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeClient.Object, fakeContact.Object);
		//Assert
		fakeUnitOfWork.Verify(x => x.ClientContactRepository.CreateRangeAsync(fakeClientContactsCollection.Object, cancellation), Times.Never());
		fakePelicanFaker.Verify(x => x.ClientContactFaker(fakeClient.Object, fakeContact.Object), Times.Never());
		Assert.Equal(fakeClientContact, result);
	}
	[Theory]
	[InlineData(1)]
	public void SeedContacts_RepositoryContainsContacts_CreateRange_And_ContactFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeContactRepository = new Mock<IGenericRepository<Contact>>();
		ICollection<Contact> fakeContact = new Collection<Contact>();
		fakeContact.Add(new Contact());
		var fakeContactsCollection = new Mock<IEnumerable<Contact>>();
		var fakeAccountManager = new Mock<IQueryable<AccountManager>>();
		fakeContactRepository.Setup(x => x.FindAll()).Returns(fakeContact.AsQueryable());
		fakePelicanFaker.Setup(x => x.ContactFaker(count, fakeAccountManager.Object))
			.Returns(fakeContactsCollection.Object);
		fakeUnitOfWork.Setup(x => x.ContactRepository)
			.Returns(fakeContactRepository.Object);
		//Act
		var result = uut.SeedContacts(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeAccountManager.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.ContactRepository.CreateRangeAsync(fakeContactsCollection.Object, cancellation), Times.Never());
		fakePelicanFaker.Verify(x => x.ContactFaker(count, fakeAccountManager.Object), Times.Never());
		Assert.Equal(fakeContact, result);
	}
	[Theory]
	[InlineData(1)]
	public void SeedDeals_RepositoryContainsDeals_CreateRange_And_DealFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeDealRepository = new Mock<IGenericRepository<Deal>>();
		ICollection<Deal> fakeDeal = new Collection<Deal>();
		fakeDeal.Add(new Deal());
		var fakeAccountManager = new Mock<IQueryable<AccountManager>>();
		var fakeClient = new Mock<IQueryable<Client>>();
		var fakeDealsCollection = new Mock<IEnumerable<Deal>>();
		fakeDealRepository.Setup(x => x.FindAll()).Returns(fakeDeal.AsQueryable());
		fakePelicanFaker.Setup(x => x.DealFaker(count, fakeClient.Object, fakeAccountManager.Object))
			.Returns(fakeDealsCollection.Object);
		fakeUnitOfWork.Setup(x => x.DealRepository)
			.Returns(fakeDealRepository.Object);
		//Act
		var result = uut.SeedDeals(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeClient.Object, fakeAccountManager.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.DealRepository.CreateRangeAsync(fakeDealsCollection.Object, cancellation), Times.Never());
		fakePelicanFaker.Verify(x => x.DealFaker(count, fakeClient.Object, fakeAccountManager.Object), Times.Never());
		Assert.Equal(fakeDeal, result);
	}
	[Theory]
	[InlineData(1)]
	public void SeedDealContacts_RepositoryContainsDealContacts_CreateRange_And_DealContactFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeDealContactRepository = new Mock<IGenericRepository<DealContact>>();
		ICollection<DealContact> fakeDealContact = new Collection<DealContact>();
		fakeDealContact.Add(new DealContact());
		var fakeContact = new Mock<IQueryable<Contact>>();
		var fakeDeal = new Mock<IQueryable<Deal>>();
		var fakeDealContactsCollection = new Mock<IEnumerable<DealContact>>();
		fakeDealContactRepository.Setup(x => x.FindAll()).Returns(fakeDealContact.AsQueryable());
		fakePelicanFaker.Setup(x => x.DealContactFaker(fakeDeal.Object, fakeContact.Object))
			.Returns(fakeDealContactsCollection.Object);
		fakeUnitOfWork.Setup(x => x.DealContactRepository)
			.Returns(fakeDealContactRepository.Object);
		//Act
		var result = uut.SeedDealContacts(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeDeal.Object, fakeContact.Object);
		//Assert
		fakeUnitOfWork.Verify(x => x.DealContactRepository.CreateRangeAsync(fakeDealContactsCollection.Object, cancellation), Times.Never());
		fakePelicanFaker.Verify(x => x.DealContactFaker(fakeDeal.Object, fakeContact.Object), Times.Never());
		Assert.Equal(fakeDealContact, result);
	}
	[Theory]
	[InlineData(1)]
	public void SeedLocations_RepositoryContainsLocations_CreateRange_And_LocationFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeLocationRepository = new Mock<IGenericRepository<Location>>();
		ICollection<Location> fakeLocation = new Collection<Location>();
		fakeLocation.Add(new Location());
		var fakeSupplier = new Mock<IQueryable<Supplier>>();
		var fakeLocationsCollection = new Mock<IEnumerable<Location>>();
		fakeLocationRepository.Setup(x => x.FindAll()).Returns(fakeLocation.AsQueryable());
		fakePelicanFaker.Setup(x => x.LocationFaker(count, fakeSupplier.Object))
			.Returns(fakeLocationsCollection.Object);
		fakeUnitOfWork.Setup(x => x.LocationRepository)
			.Returns(fakeLocationRepository.Object);
		//Act
		var result = uut.SeedLocations(fakeUnitOfWork.Object, fakePelicanFaker.Object, fakeSupplier.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.LocationRepository.CreateRangeAsync(fakeLocationsCollection.Object, cancellation), Times.Never());
		fakePelicanFaker.Verify(x => x.LocationFaker(count, fakeSupplier.Object), Times.Never());
		Assert.Equal(fakeLocation, result);
	}

	[Theory]
	[InlineData(1)]
	public void Seedsuppliers_RepositoryContainsSuppliers_CreateRange_And_SupplierFaker_IsCalledWithCorrectParameter(int count)
	{
		//Arrange
		var fakeSupplierRepository = new Mock<IGenericRepository<Supplier>>();
		ICollection<Supplier> fakeSupplier = new Collection<Supplier>();
		fakeSupplier.Add(new Supplier());
		var fakeSuppliersCollection = new Mock<IEnumerable<Supplier>>();
		fakeSupplierRepository.Setup(x => x.FindAll()).Returns(fakeSupplier.AsQueryable());
		fakePelicanFaker.Setup(x => x.SupplierFaker(count))
			.Returns(fakeSuppliersCollection.Object);
		fakeUnitOfWork.Setup(x => x.SupplierRepository)
			.Returns(fakeSupplierRepository.Object);
		//Act
		var result = uut.SeedSuppliers(fakeUnitOfWork.Object, fakePelicanFaker.Object, count);
		//Assert
		fakeUnitOfWork.Verify(x => x.SupplierRepository.CreateRangeAsync(fakeSuppliersCollection.Object, cancellation), Times.Never());
		fakePelicanFaker.Verify(x => x.SupplierFaker(count), Times.Never());
		Assert.Equal(fakeSupplier, result);
	}
}
