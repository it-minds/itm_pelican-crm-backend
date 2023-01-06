using MediatR;
using Moq;
using Pelican.Application.Users.Queries.GetAdmins;
using Pelican.Presentation.GraphQL.Users;
using Xunit;

namespace Pelican.Presentation.GraphQL.Test;
public class AdminsQueryUnitTest
{
	private readonly AdminsQuery _uut = new();
	private readonly Mock<IMediator> _mediatorMock = new();
	[Fact]
	public void GetUsers()
	{
		//Act
		_ = _uut.GetAdminsAsync(_mediatorMock.Object, default);

		//Assert
		_mediatorMock.Verify(
			x => x.Send(It.IsAny<GetAdminsQuery>(), default),
			Times.Once);
	}
}
