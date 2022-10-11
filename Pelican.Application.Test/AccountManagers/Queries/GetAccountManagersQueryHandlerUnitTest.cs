using Moq;
using Pelican.Application.AccountManagers.Queries.GetAccountManagers;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Xunit;
namespace Pelican.Application.Test.AccountManagers.Queries;
public class GetAccountManagersQueryHandlerUnitTest
{
	private GetAccountManagersQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledDataLoaderIsCalledWithCorrectParameters()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var accountMangerRepositoryMock = new Mock<IGenericRepository<AccountManager>>();
		unitOfWorkMock.Setup(x => x.AccountManagerRepository).Returns(accountMangerRepositoryMock.Object);
		uut = new GetAccountManagersQueryHandler(unitOfWorkMock.Object);
		var cancellationToken = new CancellationToken();
		var accountManagersQuery = new GetAccountManagersQuery();
		//Act
		_ = uut.Handle(accountManagersQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.AccountManagerRepository.FindAll(), Times.Once());
	}
	[Fact]
	public void TestIfWhenHandleIsCalledMultipleTimesRepositoryIsCalledMultipleTimes()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var accountManagerRepositoryMock = new Mock<IGenericRepository<AccountManager>>();
		unitOfWorkMock.Setup(x => x.AccountManagerRepository).Returns(accountManagerRepositoryMock.Object);
		uut = new GetAccountManagersQueryHandler(unitOfWorkMock.Object);
		var cancellationToken = new CancellationToken();
		var accountManagersQuery = new GetAccountManagersQuery();
		//Act
		for (var i = 0; i < 50; i++)
		{
			_ = uut.Handle(accountManagersQuery, cancellationToken);
		}

		//Assert
		unitOfWorkMock.Verify(x => x.AccountManagerRepository.FindAll(), Times.Exactly(50));
	}
}
