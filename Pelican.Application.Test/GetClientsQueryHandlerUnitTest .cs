﻿using Moq;
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
		uut.Handle(client, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.ClientRepository.FindAll(), Times.Once());
	}
}
