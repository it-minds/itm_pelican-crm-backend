namespace Pelican.Presentation.GraphQL.Test;
public class GetSuppliersQueryUnitTest
{
	private readonly SuppliersQuery uut = new();
	private readonly Mock<IMediator> _mediatorMock = new();

	[Fact]
	public void If_GetSuppliers_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken()
	{
		//Act
		_ = uut.GetSuppliers(
			_mediatorMock.Object,
			default);

		//Assert
		_mediatorMock.Verify(
			x => x.Send(It.IsAny<GetSuppliersQuery>(), default),
			Times.Once);
	}

	[Fact]
	public async void If_GetSupplierAsync_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken_And_Input()
	{
		//Arrange
		Guid id = Guid.NewGuid();
		GetSupplierByIdQuery input = new(id);

		_mediatorMock
			.Setup(x => x.Send(input, default))
			.ReturnsAsync(new Supplier() { Id = id });

		//Act
		var result = await uut.GetSupplierAsync(
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
