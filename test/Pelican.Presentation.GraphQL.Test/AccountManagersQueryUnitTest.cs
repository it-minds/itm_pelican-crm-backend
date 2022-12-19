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
	private readonly AccountManagersQuery _uut = new();
	private readonly Mock<IMediator> _mediatorMock = new();

	[Fact]
	public void If_GetAccountManagers_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken()
	{
		//Act
		_ = _uut.GetAccountManagers(_mediatorMock.Object, default);

		//Assert
		_mediatorMock.Verify(
			x => x.Send(It.IsAny<GetAccountManagersQuery>(), default),
			Times.Once);
	}

	[Fact]
	public async void If_GetAccountManagerAsync_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken_And_Input()
	{
		//Arrange
		Guid id = Guid.NewGuid();
		GetAccountManagerByIdQuery input = new(Guid.NewGuid());

		_mediatorMock
			.Setup(x => x.Send(input, default))
			.ReturnsAsync(new AccountManager(id));

		//Act
		var result = await _uut.GetAccountManagerAsync(
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
