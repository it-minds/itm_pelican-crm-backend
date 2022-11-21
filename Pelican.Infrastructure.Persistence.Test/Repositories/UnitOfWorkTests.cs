using System.Reflection;
using Moq;
using Pelican.Application.Abstractions.Data;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Primitives;
using Pelican.Infrastructure.Persistence.Repositories;
using Xunit;

namespace Pelican.Infrastructure.Persistence.Test.Repositories;

public class UnitOfWorkTests
{
	private readonly UnitOfWork _uut;

	private readonly Mock<IPelicanContext> _pelicanContextMock = new();

	public UnitOfWorkTests()
	{
		_uut = new UnitOfWork(_pelicanContextMock.Object);
	}

	[Fact]
	public void UnitOfWork_NullArgument_ThrowsException()
	{
		// Act
		var result = Record.Exception(() => new UnitOfWork(null!));

		// Assert
		Assert.Contains(
			"pelicanContext",
			result.Message);
	}

	[Fact]
	public void GetRepository_TypeAccountManager_ReturnsAccountManagerRepository()
	{
		// Act
		var result = _uut.GetRepository<AccountManager>();

		// Assert
		Assert.IsType<GenericRepository<AccountManager>>(result);
	}

	[Fact]
	public void GetRepository_TypeAccountManagerDeal_ReturnsAccountManagerDealRepository()
	{
		// Act
		var result = _uut.GetRepository<AccountManagerDeal>();

		// Assert
		Assert.IsType<GenericRepository<AccountManagerDeal>>(result);
	}

	[Fact]
	public void GetRepository_TypeClientContact_ReturnsClientContactRepository()
	{
		// Act
		var result = _uut.GetRepository<ClientContact>();

		// Assert
		Assert.IsType<GenericRepository<ClientContact>>(result);
	}

	[Fact]
	public void GetRepository_TypeClient_ReturnsClientRepository()
	{
		// Act
		var result = _uut.GetRepository<Client>();

		// Assert
		Assert.IsType<GenericRepository<Client>>(result);
	}

	[Fact]
	public void GetRepository_TypeDealContact_ReturnsDealContactRepository()
	{
		// Act
		var result = _uut.GetRepository<DealContact>();

		// Assert
		Assert.IsType<GenericRepository<DealContact>>(result);
	}

	[Fact]
	public void GetRepository_TypeDeal_ReturnsDealRepository()
	{
		// Act
		var result = _uut.GetRepository<Deal>();

		// Assert
		Assert.IsType<GenericRepository<Deal>>(result);
	}

	[Fact]
	public void GetRepository_TypeLocation_ReturnsLocationRepository()
	{
		// Act
		var result = _uut.GetRepository<Location>();

		// Assert
		Assert.IsType<GenericRepository<Location>>(result);
	}

	[Fact]
	public void GetRepository_TypeSupplier_ReturnsSupplierRepository()
	{
		// Act
		var result = _uut.GetRepository<Supplier>();

		// Assert
		Assert.IsType<GenericRepository<Supplier>>(result);
	}

	[Fact]
	public void GetRepository_TypeContact_ReturnsContactRepository()
	{
		// Act
		var result = _uut.GetRepository<Contact>();

		// Assert
		Assert.IsType<GenericRepository<Contact>>(result);
	}

	[Fact]
	public void GetRepository_TypeInvalid_ThrowsExceptiom()
	{
		// Act
		var result = Record.Exception(() => _uut.GetRepository<Entity>());

		// Assert
		Assert.Contains(
			"Generic Repository is not of correct Entity type",
			result.Message);
	}

	[Fact]
	public void AccountManagerDealRepository_ReturnsAccountManagerDealRepository()
	{
		// Act
		var result = _uut.AccountManagerDealRepository;

		// Assert
		Assert.IsType<GenericRepository<AccountManagerDeal>>(result);
	}

	[Fact]
	public void AccountManagerRepository_ReturnsAccountManagerRepository()
	{
		// Act
		var result = _uut.AccountManagerRepository;

		// Assert
		Assert.IsType<GenericRepository<AccountManager>>(result);
	}

	[Fact]
	public void ClientRepository_ReturnsClientRepository()
	{
		// Act
		var result = _uut.ClientRepository;

		// Assert
		Assert.IsType<GenericRepository<Client>>(result);
	}

	[Fact]
	public void DealRepository_ReturnsDealRepository()
	{
		// Act
		var result = _uut.DealRepository;

		// Assert
		Assert.IsType<GenericRepository<Deal>>(result);
	}

	[Fact]
	public void LocationRepository_ReturnsLocationRepository()
	{
		// Act
		var result = _uut.LocationRepository;

		// Assert
		Assert.IsType<GenericRepository<Location>>(result);
	}

	[Fact]
	public void SupplierRepository_ReturnsSupplierRepository()
	{
		// Act
		var result = _uut.SupplierRepository;

		// Assert
		Assert.IsType<GenericRepository<Supplier>>(result);
	}

	[Fact]
	public void ContactRepository_ReturnsContactRepository()
	{
		// Act
		var result = _uut.ContactRepository;

		// Assert
		Assert.IsType<GenericRepository<Contact>>(result);
	}

	[Fact]
	public void ClientContactRepository_ReturnsClientContactRepository()
	{
		// Act
		var result = _uut.ClientContactRepository;

		// Assert
		Assert.IsType<GenericRepository<ClientContact>>(result);
	}

	[Fact]
	public void DealContactRepository_ReturnsDealContactRepository()
	{
		// Act
		var result = _uut.DealContactRepository;

		// Assert
		Assert.IsType<GenericRepository<DealContact>>(result);
	}

	[Fact]
	public async void SaveAsync_IsCalledOnce_DbContextReceivesOneSaveChanges()
	{
		//Act
		await _uut.SaveAsync(default);

		//Assert
		_pelicanContextMock.Verify(
			x => x.SaveChangesAsync(default),
			Times.Once);
	}
}
