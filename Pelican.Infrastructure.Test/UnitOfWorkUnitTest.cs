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
		var myDbContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myDbContextMock.Object);
		//Act
		uut.Save();
		//Assert
		myDbContextMock.Verify(x => x.SaveChanges(), Times.Once());
	}
	//Many Test
	[Fact]
	public void UnitOfWorkSaveIsCalled50Times_DbContextReceives50SaveChanges()
	{
		//Arrange
		var myDbContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myDbContextMock.Object);
		//Act
		for (int i = 0; i < 50; i++)
		{
			uut.Save();
		}
		//Assert
		myDbContextMock.Verify(x => x.SaveChanges(), Times.Exactly(50));
	}
	//One Test
	[Fact]
	public void UnitOfWorlSaveAsyncIsCalledOnce_DbContextReceivesOneSaveChanges()
	{
		//Arrange
		var myDbContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myDbContextMock.Object);
		CancellationToken cancellation = new();
		//Act
		uut.SaveAsync();
		//Assert
		myDbContextMock.Verify(x => x.SaveChangesAsync(cancellation), Times.Once());
	}
	//Many Test
	[Fact]
	public void UnitOfWorkSaveAsyncIsCalled50Times_DbContextReceives50SaveChanges()
	{
		//Arrange
		var myDbContextMock = new Mock<IPelicanContext>();
		uut = new UnitOfWork(myDbContextMock.Object);
		CancellationToken cancellation = new();
		//Act
		for (int i = 0; i < 50; i++)
		{
			uut.SaveAsync();
		}
		//Assert
		myDbContextMock.Verify(x => x.SaveChangesAsync(cancellation), Times.Exactly(50));
	}
}
