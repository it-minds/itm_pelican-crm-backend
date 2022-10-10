using Moq;
using Pelican.Application.Clients.Queries.GetClients;
using Pelican.Application.Clients.Queries.GetCLients;
using Pelican.Domain.Repositories;
using Xunit;
namespace Pelican.Application.Test;
public class GetClientsQueryHandlerUnitTest
{
	private GetClientsQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledRepositoryIsCalled()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var clientRepositoryMock = new Mock<IClientRepository>();
		unitOfWorkMock.Setup(x => x.ClientRepository).Returns(clientRepositoryMock.Object);
		uut = new GetClientsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetClientsQuery client = new GetClientsQuery();
		//Act
		_ = uut.Handle(client, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.ClientRepository.FindAll(), Times.Once());
	}
	[Fact]
	public void TestIfWhenHandleIsCalledMultipleTimesRepositoryIsCalledMultipleTimes()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var clientRepositoryMock = new Mock<IClientRepository>();
		unitOfWorkMock.Setup(x => x.ClientRepository).Returns(clientRepositoryMock.Object);
		uut = new GetClientsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetClientsQuery clientsQuery = new GetClientsQuery();
		//Act
		for (int i = 0; i < 50; i++)
		{
			_ = uut.Handle(clientsQuery, cancellationToken);
		}

		//Assert
		unitOfWorkMock.Verify(x => x.ClientRepository.FindAll(), Times.Exactly(50));
	}
}
