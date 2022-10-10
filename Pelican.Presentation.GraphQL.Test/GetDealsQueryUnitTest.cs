using MediatR;
using Moq;
using Pelican.Application.Deals.Queries.GetDealById;
using Pelican.Application.Deals.Queries.GetDeals;
using Pelican.Presentation.GraphQL.Deals;
using Xunit;

namespace Pelican.Presentation.GraphQL.Test;
public class GetDealsQueryUnitTest
{
	private DealsQuery uut;
	[Fact]
	public void IfGetDealsIsCalledMediatorCallsSendWithCorrectCancellationToken()
	{
		//Arrange
		uut = new DealsQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		//Act
		uut.GetDeals(mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(It.IsAny<GetDealsQuery>(), cancellationToken), Times.Exactly(1));
	}
	[Fact]
	public void IfGetDealAsyncIsCalledMediatorCallsSendWithCorrectCancellationTokenAndInput()
	{
		//Arrange
		uut = new DealsQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		Guid id = Guid.NewGuid();
		GetDealByIdQuery input = new GetDealByIdQuery(id);
		//Act
		uut.GetDealAsync(input, mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(input, cancellationToken), Times.Exactly(1));
	}
}
