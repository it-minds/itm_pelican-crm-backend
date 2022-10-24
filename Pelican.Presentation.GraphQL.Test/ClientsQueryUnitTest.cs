using MediatR;
using Moq;
using Pelican.Application.Clients.Queries.GetClientById;
using Pelican.Application.Clients.Queries.GetClients;
using Pelican.Domain.Entities;
using Pelican.Presentation.GraphQL.Clients;
using Xunit;

namespace Pelican.Presentation.GraphQL.Test;

public class ClientsQueryUnitTest
{
	private ClientsQuery uut;

	[Fact]
	public void IfGetClientsIsCalledMediatorCallsSendWithCorrectCancellationToken()
	{
		//Arrange
		uut = new ClientsQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		//Act
		_ = uut.GetClients(mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(It.IsAny<GetClientsQuery>(), cancellationToken), Times.Once());
	}
	[Fact]
	public async void IfGetClientAsyncIsCalledMediatorCallsSendWithCorrectCancellationTokenAndInput()
	{
		//Arrange
		uut = new ClientsQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		Guid id = Guid.NewGuid();
		GetClientByIdQuery input = new GetClientByIdQuery(id);
		mediatorMock.Setup(x => x.Send(input, cancellationToken)).ReturnsAsync(new Client(id));
		//Act
		var result = await uut.GetClientAsync(input.Id, mediatorMock.Object, cancellationToken);
		//Assert
		Assert.Equal(id, result.Id);
		mediatorMock.Verify(x => x.Send(input, cancellationToken), Times.Exactly(1));
	}
}
