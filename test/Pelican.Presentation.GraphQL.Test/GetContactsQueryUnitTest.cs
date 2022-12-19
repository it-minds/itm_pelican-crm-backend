namespace Pelican.Presentation.GraphQL.Test;
public class GetContactsQueryUnitTest
{
	private readonly ContactsQuery _uut = new();
	private readonly Mock<IMediator> _mediatorMock = new();

	[Fact]
	public void If_GetContacts_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken()
	{
		//Act
		_ = _uut.GetContacts(_mediatorMock.Object, default);

		//Assert
		_mediatorMock.Verify(
			x => x.Send(It.IsAny<GetContactsQuery>(), default),
			Times.Once);
	}

	[Fact]
	public async void If_GetContactAsync_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken_And_Input()
	{
		//Arrange
		Guid id = Guid.NewGuid();
		GetContactByIdQuery input = new(id);

		_mediatorMock
			.Setup(x => x.Send(input, default))
			.ReturnsAsync(new Contact() { Id = id });

		//Act
		var result = await _uut.GetContactAsync(
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
