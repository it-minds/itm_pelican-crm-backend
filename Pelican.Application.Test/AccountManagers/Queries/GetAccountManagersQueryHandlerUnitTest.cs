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
		CancellationToken cancellationToken = new CancellationToken();
		GetAccountManagersQuery accountManagersQuery = new GetAccountManagersQuery();
		//Act
		_ = uut.Handle(accountManagersQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.AccountManagerRepository.FindAllWithIncludes(), Times.Once());
	}
}
