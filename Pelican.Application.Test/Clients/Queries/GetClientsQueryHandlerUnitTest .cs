﻿using Moq;
using Pelican.Application.Clients.Queries.GetClients;
using Pelican.Application.Clients.Queries.GetCLients;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.Clients.Queries;
public class GetClientsQueryHandlerUnitTest
{
	private GetClientsQueryHandler uut;
	[Fact]
	public async void Test_If_When_Handle_Is_Called_Repository_Is_Called()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var clientRepositoryMock = new Mock<IGenericRepository<Client>>();
		unitOfWorkMock.Setup(x => x.ClientRepository).Returns(clientRepositoryMock.Object);
		uut = new GetClientsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetClientsQuery client = new GetClientsQuery();
		//Act
		_ = await uut.Handle(client, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.ClientRepository.FindAllWithIncludes(), Times.Once());
	}
}
