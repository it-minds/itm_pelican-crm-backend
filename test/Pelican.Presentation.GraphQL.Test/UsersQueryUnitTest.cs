namespace Pelican.Presentation.GraphQL.Test;
public class UsersQueryUnitTest
{
	private readonly UsersQuery _uut = new();
	private readonly Mock<IMediator> _mediatorMock = new();

	[Fact]
	public void GetUsersAsync_MediatorRecievesSendCallWithAGetUsersQueryasParam_NoErrors()
	{
		//Act
		_ = _uut.GetUsersAsync(_mediatorMock.Object, default);

		//Assert
		_mediatorMock.Verify(
			x => x.Send(It.IsAny<GetUsersQuery>(), default),
			Times.Once);
	}
}
