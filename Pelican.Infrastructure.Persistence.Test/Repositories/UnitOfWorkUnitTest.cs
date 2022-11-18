using Moq;
using Pelican.Application.Abstractions.Data;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Infrastructure.Persistence.Repositories;
using Xunit;

namespace Pelican.Infrastructure.Persistence.Test.Repositories;

public class UnitOfWorkUnitTest
{
	private readonly IUnitOfWork _uut;
	private readonly Mock<IPelicanContext> _myPelicanContextMock;

	public UnitOfWorkUnitTest()
	{
		_myPelicanContextMock = new Mock<IPelicanContext>();
		_uut = new UnitOfWork(_myPelicanContextMock.Object);
	}

	[Fact]
	public async void UnitOfWorlSaveAsyncIsCalledOnce_DbContextReceivesOneSaveChanges()
	{
		//Arrange
		//Act
		await _uut.SaveAsync(default);
		//Assert
		_myPelicanContextMock.Verify(x => x.SaveChangesAsync(default), Times.Once());
	}

	[Fact]
	public async void UnitOfWorkSaveAsyncIsCalled50Times_DbContextReceives50SaveChanges()
	{
		//Arrange

		//Act
		for (var i = 0; i < 50; i++)
		{
			await _uut.SaveAsync(default);
		}
		//Assert
		_myPelicanContextMock.Verify(x => x.SaveChangesAsync(default), Times.Exactly(50));
	}

	[Fact]
	public void UnitOfWorkAccountManagerDealRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => _uut.AccountManagerDealRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}

	[Fact]
	public void UnitOfWorkAccountManagerRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => _uut.AccountManagerRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}

	[Fact]
	public void UnitOfWorkClientRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => _uut.ClientRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}

	[Fact]
	public void UnitOfWorkGetContactRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => _uut.ContactRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}

	[Fact]
	public void UnitOfWorkGetSupplierRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => _uut.SupplierRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}

	[Fact]
	public void UnitOfWorkGetLocationRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => _uut.LocationRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}

	[Fact]
	public void UnitOfWorkGetClientContactRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{   //Arrange
		//Act
		var exception = Record.Exception(() => _uut.ClientContactRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}

	[Fact]
	public void UnitOfWorkGetDealContactRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => _uut.DealContactRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}

	[Fact]
	public void UnitOfWorkGetDealRepository_ThrowsExceptionDueToContextNotBeingofType_PelicanContext()
	{
		//Arrange
		//Act
		var exception = Record.Exception(() => _uut.DealRepository);
		//Assert
		Assert.NotNull(exception);
		Assert.IsType<InvalidCastException>(exception);
	}
}
