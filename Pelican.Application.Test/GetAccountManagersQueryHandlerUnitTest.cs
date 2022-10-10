using Moq;
using Pelican.Application.AccountManagers.Queries.GetAccountManagers;
using Pelican.Domain.Repositories;
using Xunit;
namespace Pelican.Application.Test;
public class GetAccountManagersQueryHandlerUnitTest
{
	private GetAccountManagersQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledDataLoaderIsCalledWithCorrectParameters()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var accountMangerRepositoryMock = new Mock<IAccountManagerRepository>();
		unitOfWorkMock.Setup(x => x.AccountManagerRepository).Returns(accountMangerRepositoryMock.Object);
		uut = new GetAccountManagersQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetAccountManagersQuery accountManagersQuery = new GetAccountManagersQuery();
		//Act
		uut.Handle(accountManagersQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.AccountManagerRepository.FindAll(), Times.Once());
	}
	[Fact]
	public void TestIfWhenHandleIsCalledMultipleTimesRepositoryIsCalledMultipleTimes()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var accountManagerRepositoryMock = new Mock<IAccountManagerRepository>();
		unitOfWorkMock.Setup(x => x.AccountManagerRepository).Returns(accountManagerRepositoryMock.Object);
		uut = new GetAccountManagersQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetAccountManagersQuery accountManagersQuery = new GetAccountManagersQuery();
		//Act
		for (int i = 0; i < 50; i++)
		{
			uut.Handle(accountManagersQuery, cancellationToken);
		}

		//Assert
		unitOfWorkMock.Verify(x => x.AccountManagerRepository.FindAll(), Times.Exactly(50));
	}
}
