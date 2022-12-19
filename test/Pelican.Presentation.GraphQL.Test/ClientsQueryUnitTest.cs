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
	private readonly ClientsQuery _uut = new();
	private readonly Mock<IMediator> _mediatorMock = new();

	[Fact]
	public void If_GetClients_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken()
	{
		//Act
		_ = _uut.GetClients(_mediatorMock.Object, default);

		//Assert
		_mediatorMock.Verify(
			x => x.Send(It.IsAny<GetClientsQuery>(), default),
			Times.Once);
	}

	[Fact]
	public async void If_GetClientAsync_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken_And_Input()
	{
		//Arrange
		Guid id = Guid.NewGuid();
		GetClientByIdQuery input = new(id);

		_mediatorMock
			.Setup(x => x.Send(input, default))
			.ReturnsAsync(new Client(id));

		//Act
		var result = await _uut.GetClientAsync(
			input.Id,
			_mediatorMock.Object,
			default);

		//Assert
		Assert.Equal(
			id,
			result.Id);

		_mediatorMock.Verify(
			x => x.Send(input, default),
			Times.Once);
	}
}
