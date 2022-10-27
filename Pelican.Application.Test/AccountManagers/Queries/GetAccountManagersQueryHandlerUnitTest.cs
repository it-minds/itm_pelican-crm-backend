using Moq;
using Pelican.Application.AccountManagers.Queries.GetAccountManagers;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Xunit;
namespace Pelican.Application.Test.AccountManagers.Queries;
public class GetAccountManagersQueryHandlerUnitTest
{
	[Fact]
	public async void Test_If_When_Handle_Is_Called_Repository_Is_Called()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var accountMangerRepositoryMock = new Mock<IGenericRepository<AccountManager>>();
		unitOfWorkMock.Setup(x => x.AccountManagerRepository).Returns(accountMangerRepositoryMock.Object);
		var uut = new GetAccountManagersQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetAccountManagersQuery accountManagersQuery = new GetAccountManagersQuery();
		//Act
		_ = await uut.Handle(accountManagersQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.AccountManagerRepository.FindAllWithIncludes(), Times.Once());
	}
}
