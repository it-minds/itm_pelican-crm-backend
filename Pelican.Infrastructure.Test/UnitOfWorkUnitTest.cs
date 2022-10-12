using Moq;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Repositories;
using Pelican.Infrastructure.Persistence.Repositories;
using Xunit;

namespace Pelican.Infrastructure.Test;

public class UnitOfWorkUnitTest
{
	private IUnitOfWork uut;
	//One Test
	[Fact]
	public void UnitOfWorlSaveIsCalledOnce_DbContextReceivesOneSaveChanges()
	{
		//Arrange
		var myPelicantContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicantContextMock.Object);
		//Act
		uut.Save();
		//Assert
		myPelicantContextMock.Verify(x => x.SaveChanges(), Times.Once());
	}
	//Many Test
	[Fact]
	public void UnitOfWorkSaveIsCalled50Times_DbContextReceives50SaveChanges()
	{
		//Arrange
		var myPelicantContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicantContextMock.Object);
		//Act
		for (int i = 0; i < 50; i++)
		{
			uut.Save();
		}
		//Assert
		myPelicantContextMock.Verify(x => x.SaveChanges(), Times.Exactly(50));
	}
	//One Test
	[Fact]
	public void UnitOfWorlSaveAsyncIsCalledOnce_DbContextReceivesOneSaveChanges()
	{
		//Arrange
		var myPelicantContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicantContextMock.Object);
		CancellationToken cancellation = new();
		//Act
		uut.SaveAsync();
		//Assert
		myPelicantContextMock.Verify(x => x.SaveChangesAsync(cancellation), Times.Once());
	}
	//Many Test
	[Fact]
	public void UnitOfWorkSaveAsyncIsCalled50Times_DbContextReceives50SaveChanges()
	{
		//Arrange
		var myPelicantContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicantContextMock.Object);
		CancellationToken cancellation = new();
		//Act
		for (int i = 0; i < 50; i++)
		{
			uut.SaveAsync();
		}
		//Assert
		myPelicantContextMock.Verify(x => x.SaveChangesAsync(cancellation), Times.Exactly(50));
	}

	[Fact]
	public void UnitOfWorkAccountManagerDealRepositoryNotNull()
	{
		//Arrange
		var myPelicantContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicantContextMock.Object);
		//Act
		//Assert
		var ex = Assert.Throws<InvalidCastException>(() => uut.AccountManagerDealRepository);
	}
	[Fact]
	public void UnitOfWorkAccountManagerRepositoryNotNull()
	{
		//Arrange
		var myPelicantContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicantContextMock.Object);
		//Act

		//Assert
		var ex = Assert.Throws<InvalidCastException>(() => uut.AccountManagerRepository);
	}
	[Fact]
	public void UnitOfWorkClientRepositoryNotNull()
	{
		//Arrange
		var myPelicantContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicantContextMock.Object);
		//Act
		//Assert
		var ex = Assert.Throws<InvalidCastException>(() => uut.ClientRepository);
	}
	[Fact]
	public void UnitOfWorkGetContactRepository()
	{
		//Arrange
		var myPelicantContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicantContextMock.Object);
		//Act
		//Assert
		var ex = Assert.Throws<InvalidCastException>(() => uut.ContactRepository);
	}
	[Fact]
	public void UnitOfWorkGetSupplierRepository()
	{
		//Arrange
		var myPelicantContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicantContextMock.Object);
		//Act
		//Assert
		var ex = Assert.Throws<InvalidCastException>(() => uut.SupplierRepository);
	}
	[Fact]
	public void UnitOfWorkGetLocationRepository()
	{
		//Arrange
		var myPelicantContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicantContextMock.Object);
		//Act
		//Assert
		var ex = Assert.Throws<InvalidCastException>(() => uut.LocationRepository);
	}
	[Fact]
	public void UnitOfWorkGetClientContactRepository()
	{
		//Arrange
		var myPelicantContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicantContextMock.Object);
		//Act
		//Assert
		var ex = Assert.Throws<InvalidCastException>(() => uut.ClientContactRepository);
	}
	[Fact]
	public void UnitOfWorkGetDealContactRepository()
	{
		//Arrange
		var myPelicantContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicantContextMock.Object);
		//Act
		//Assert
		var ex = Assert.Throws<InvalidCastException>(() => uut.DealContactRepository);
	}
	[Fact]
	public void UnitOfWorkGetDealRepository()
	{
		//Arrange
		var myPelicantContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicantContextMock.Object);
		//Act
		//Assert
		var ex = Assert.Throws<InvalidCastException>(() => uut.DealRepository);
	}
}
