﻿using MediatR;
using Moq;
using Pelican.Application.Deals.Queries.GetDealById;
using Pelican.Application.Deals.Queries.GetDeals;
using Pelican.Domain.Entities;
using Pelican.Presentation.GraphQL.Deals;
using Xunit;

namespace Pelican.Presentation.GraphQL.Test;
public class GetDealsQueryUnitTest
{
	private DealsQuery uut;
	[Fact]
	public void If_GetDeals_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken()
	{
		//Arrange
		uut = new DealsQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		//Act
		_ = uut.GetDeals(mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(It.IsAny<GetDealsQuery>(), cancellationToken), Times.Once());
	}
	[Fact]
	public async void If_GetDealAsync_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken_And_Input()
	{
		//Arrange
		uut = new DealsQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		Guid id = Guid.NewGuid();
		GetDealByIdQuery input = new GetDealByIdQuery(id);
		mediatorMock.Setup(x => x.Send(input, cancellationToken)).ReturnsAsync(new Deal(id));
		//Act
		var result = await uut.GetDealAsync(input.Id, mediatorMock.Object, cancellationToken);
		//Assert
		Assert.Equal(id, result.Id);
		mediatorMock.Verify(x => x.Send(input, cancellationToken), Times.Once());
	}
}