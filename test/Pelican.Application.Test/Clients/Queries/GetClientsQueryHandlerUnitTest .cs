using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Clients.Queries.GetClients;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.Clients.Queries;
public class GetClientsQueryHandlerUnitTest
{
	[Fact]
	public async void Test_If_When_Handle_Is_Called_Repository_Is_Called()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var clientRepositoryMock = new Mock<IGenericRepository<Client>>();
		unitOfWorkMock.Setup(x => x.ClientRepository).Returns(clientRepositoryMock.Object);
		var uut = new GetClientsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetClientsQuery client = new GetClientsQuery();
		//Act
		_ = await uut.Handle(client, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.ClientRepository.FindAll(), Times.Once());
	}
}
