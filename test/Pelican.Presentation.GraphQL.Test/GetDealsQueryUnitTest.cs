using MediatR;
using Moq;
using Pelican.Application.Deals.Queries.GetDealById;
using Pelican.Application.Deals.Queries.GetDeals;
using Pelican.Domain.Entities;
using Pelican.Presentation.GraphQL.Deals;
using Xunit;

namespace Pelican.Presentation.GraphQL.Test;
public class GetDealsQueryUnitTest
{
	private readonly DealsQuery uut = new();
	private readonly Mock<IMediator> _mediatorMock = new();

	[Fact]
	public void If_GetDeals_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken()
	{
		//Act
		_ = uut.GetDeals(
			_mediatorMock.Object,
			default);

		//Assert
		_mediatorMock.Verify(
			x => x.Send(It.IsAny<GetDealsQuery>(), default),
			Times.Once);
	}

	[Fact]
	public async void If_GetDealAsync_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken_And_Input()
	{
		//Arrange
		Guid id = Guid.NewGuid();
		GetDealByIdQuery input = new(id);

		_mediatorMock
			.Setup(x => x.Send(input, default))
			.ReturnsAsync(new Deal(id));

		//Act
		var result = await uut.GetDealAsync(
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
