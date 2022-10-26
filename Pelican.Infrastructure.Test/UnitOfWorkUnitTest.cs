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
	public void UnitOfWorkAccountManagerDealRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => uut.AccountManagerDealRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}
	[Fact]
	public void UnitOfWorkAccountManagerRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => uut.AccountManagerRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}
	[Fact]
	public void UnitOfWorkClientRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => uut.ClientRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}
	[Fact]
	public void UnitOfWorkGetContactRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => uut.ContactRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}
	[Fact]
	public void UnitOfWorkGetSupplierRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => uut.SupplierRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}
	[Fact]
	public void UnitOfWorkGetLocationRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => uut.LocationRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}
	[Fact]
	public void UnitOfWorkGetClientContactRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{   //Arrange
		//Act
		var exception = Record.Exception(() => uut.ClientContactRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}
	[Fact]
	public void UnitOfWorkGetDealContactRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => uut.DealContactRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}
	[Fact]
	public void UnitOfWorkGetDealRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => uut.DealRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}
}
