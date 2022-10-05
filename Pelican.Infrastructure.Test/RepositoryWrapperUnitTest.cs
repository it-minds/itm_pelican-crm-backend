using Moq;
using Pelican.Application.Common.Interfaces;
using Pelican.Domain.Repositories;
using Pelican.Infrastructure.Persistence.Repositories;
using Xunit;
namespace Pelican.Infrastructure.Test;

public class RepositoryWrapperUnitTest
{
	private IRepositoryWrapper? uut;
	[Fact]
	public void WrapperSaveIsCalledOnce_DbContextReceivesOneSaveChanges()
	{
		//Arrange
		var myDbContextMock = new Mock<IPelicanContext>();
		myDbContextMock.Setup(x => x.SaveChanges());
		uut = new RepositoryWrapper(myDbContextMock.Object);
		//Act
		uut.Save();
		//Assert
		myDbContextMock.Verify(x => x.SaveChanges(), Times.Exactly(1));
	}
	[Fact]
	public void WrapperSaveIsCalled50Times_DbContextReceives50SaveChanges()
	{
		//Arrange
		var myDbContextMock = new Mock<IPelicanContext>();
		myDbContextMock.Setup(x => x.SaveChanges());
		uut = new RepositoryWrapper(myDbContextMock.Object);
		//Act
		for (int i = 0; i < 50; i++)
		{
			uut.Save();
		}
		//Assert
		myDbContextMock.Verify(x => x.SaveChanges(), Times.Exactly(50));
	}
}
