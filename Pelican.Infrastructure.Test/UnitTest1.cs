using Pelican.Domain.Repositories;
using Xunit;
namespace Pelican.Infrastructure.Test;

public class RepositoryWrapperTest
{
	private readonly IRepositoryWrapper uut;
	[Fact]
	public void Test1()
	{
		////Arrange
		//var myDbContextMock = new Mock<PelicanContext>();
		//myDbContextMock.Setup(x => x.SaveChanges()).Returns(1);
		//uut = new RepositoryWrapper(myDbContextMock.Object);
		////Act
		//uut.Save();
		////Assert
		//myDbContextMock.Verify(x => x.SaveChanges(), Times.Exactly(1));
	}
}
