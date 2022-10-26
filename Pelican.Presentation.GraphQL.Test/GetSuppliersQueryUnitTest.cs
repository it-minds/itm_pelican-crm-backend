using MediatR;
using Moq;
using Pelican.Application.Suppliers.Queries.GetSupplierById;
using Pelican.Application.Suppliers.Queries.GetSuppliers;
using Pelican.Domain.Entities;
using Pelican.Presentation.GraphQL.Suppliers;
using Xunit;

namespace Pelican.Presentation.GraphQL.Test;
public class GetSuppliersQueryUnitTest
{
	private SuppliersQuery uut;
	[Fact]
	public void If_GetSuppliers_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken()
	{
		//Arrange
		uut = new SuppliersQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		//Act
		_ = uut.GetSuppliers(mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(It.IsAny<GetSuppliersQuery>(), cancellationToken), Times.Once());
	}
	[Fact]
	public async void If_GetSupplierAsync_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken_And_Input()
	{
		//Arrange
		uut = new SuppliersQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		Guid id = Guid.NewGuid();
		GetSupplierByIdQuery input = new GetSupplierByIdQuery(id);
		mediatorMock.Setup(x => x.Send(input, cancellationToken)).ReturnsAsync(new Supplier(id));
		//Act
		var result = await uut.GetSupplierAsync(input.Id, mediatorMock.Object, cancellationToken);
		//Assert
		Assert.Equal(id, result.Id);
		mediatorMock.Verify(x => x.Send(input, cancellationToken), Times.Once());
	}
}
