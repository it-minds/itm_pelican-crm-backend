using MediatR;
using Moq;
using Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
using Pelican.Application.AccountManagers.Queries.GetAccountManagers;
using Pelican.Domain.Entities;
using Pelican.Presentation.GraphQL.AccountManagers;
using Xunit;

namespace Pelican.Presentation.GraphQL.Test;
public class AccountManagersQueryUnitTest
{
	private AccountManagersQuery uut;
	[Fact]
	public void If_GetAccountManagers_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken()
	{
		//Arrange
		uut = new AccountManagersQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		//Act
		_ = uut.GetAccountManagers(mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(It.IsAny<GetAccountManagersQuery>(), cancellationToken), Times.Once());
	}
	[Fact]
	public async void If_GetAccountManagerAsync_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken_And_Input()
	{
		//Arrange
		uut = new AccountManagersQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		Guid id = Guid.NewGuid();
		GetAccountManagerByIdQuery input = new GetAccountManagerByIdQuery(id);
		mediatorMock.Setup(x => x.Send(input, cancellationToken)).ReturnsAsync(new AccountManager(id));
		//Act
		var result = await uut.GetAccountManagerAsync(input.Id, mediatorMock.Object, cancellationToken);
		//Assert
		Assert.Equal(id, result.Id);
		mediatorMock.Verify(x => x.Send(input, cancellationToken), Times.Once);
	}
}
