using MediatR;
using Moq;
using Pelican.Application.AccountManagers.Queries.GetAccountManagerById;
using Pelican.Application.AccountManagers.Queries.GetAccountManagers;
using Pelican.Presentation.GraphQL.AccountManagers;
using Xunit;

namespace Pelican.Presentation.GraphQL.Test;
public class AccountManagersQueryUnitTest
{
	private AccountManagersQuery uut;
	[Fact]
	public void IfGetAccountManagersIsCalledMediatorCallsSendWithCorrectCancellationToken()
	{
		//Arrange
		uut = new AccountManagersQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		//Act
		_ = uut.GetAccountManagers(mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(It.IsAny<GetAccountManagersQuery>(), cancellationToken), Times.Exactly(1));
	}
	[Fact]
	public void IfGetAccountManagerAsyncIsCalledMediatorCallsSendWithCorrectCancellationTokenAndInput()
	{
		//Arrange
		uut = new AccountManagersQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		Guid id = Guid.NewGuid();
		GetAccountManagerByIdQuery input = new GetAccountManagerByIdQuery(id);
		//Act
		uut.GetAccountManagerAsync(input, mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(input, cancellationToken), Times.Exactly(1));
	}
}
