using Moq;
using Pelican.Application.Common.Interfaces;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Infrastructure.Persistence.Repositories;
using Xunit;

namespace Pelican.Infrastructure.Persistence.Test;

public class UnitOfWorkUnitTest
{
	private IUnitOfWork uut;
	private Mock<IPelicanContext> myPelicanContextMock;
	private CancellationToken cancellationToken;
	public UnitOfWorkUnitTest()
	{
		myPelicanContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myPelicanContextMock.Object);
		cancellationToken = new CancellationToken();
	}
	//One Test
	[Fact]
	public void UnitOfWorlSaveIsCalledOnce_DbContextReceivesOneSaveChanges()
	{
		//Arrange

		//Act
		uut.Save();
		//Assert
		myPelicanContextMock.Verify(x => x.SaveChanges(), Times.Once());
	}
	//Many Test
	[Fact]
	public void UnitOfWorkSaveIsCalled50Times_DbContextReceives50SaveChanges()
	{
		//Arrange
		//Act
		for (var i = 0; i < 50; i++)
		{
			uut.Save();
		}
		//Assert
		myPelicanContextMock.Verify(x => x.SaveChanges(), Times.Exactly(50));
	}
	//One Test
	[Fact]
	public async void UnitOfWorlSaveAsyncIsCalledOnce_DbContextReceivesOneSaveChanges()
	{
		//Arrange
		//Act
		await uut.SaveAsync(cancellationToken);
		//Assert
		myPelicanContextMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Once());
	}
	//Many Test
	[Fact]
	public async void UnitOfWorkSaveAsyncIsCalled50Times_DbContextReceives50SaveChanges()
	{
		//Arrange

		//Act
		for (var i = 0; i < 50; i++)
		{
			await uut.SaveAsync(cancellationToken);
		}
		//Assert
		myPelicanContextMock.Verify(x => x.SaveChangesAsync(cancellationToken), Times.Exactly(50));
	}

	[Fact]
	public void UnitOfWorkAccountManagerDealRepositoryNotNull()
	{
		//Arrange
		//Act
		//Assert
		Assert.Throws<InvalidCastException>(() => uut.AccountManagerDealRepository);
	}
	[Fact]
	public void UnitOfWorkAccountManagerRepositoryNotNull()
	{
		//Arrange
		//Act
		//Assert
		Assert.Throws<InvalidCastException>(() => uut.AccountManagerRepository);
	}
	[Fact]
	public void UnitOfWorkClientRepositoryNotNull()
	{
		//Arrange
		//Act
		//Assert
		Assert.Throws<InvalidCastException>(() => uut.ClientRepository);
	}
	[Fact]
	public void UnitOfWorkGetContactRepository()
	{
		//Arrange
		//Act
		//Assert
		Assert.Throws<InvalidCastException>(() => uut.ContactRepository);
	}
	[Fact]
	public void UnitOfWorkGetSupplierRepository()
	{
		//Arrange
		//Act
		//Assert
		Assert.Throws<InvalidCastException>(() => uut.SupplierRepository);
	}
	[Fact]
	public void UnitOfWorkGetLocationRepository()
	{
		//Arrange
		//Act
		//Assert
		Assert.Throws<InvalidCastException>(() => uut.LocationRepository);
	}
	[Fact]
	public void UnitOfWorkGetClientContactRepository()
	{
		//Arrange
		//Act
		//Assert
		Assert.Throws<InvalidCastException>(() => uut.ClientContactRepository);
	}
	[Fact]
	public void UnitOfWorkGetDealContactRepository()
	{
		//Arrange
		//Act
		//Assert
		Assert.Throws<InvalidCastException>(() => uut.DealContactRepository);
	}
	[Fact]
	public void UnitOfWorkGetDealRepository()
	{
		//Arrange
		//Act
		//Assert
		Assert.Throws<InvalidCastException>(() => uut.DealRepository);
	}
}
