using MediatR;
using Moq;
using Pelican.Application.Contacts.Queries.GetContactById;
using Pelican.Application.Contacts.Queries.GetContacts;
using Pelican.Domain.Entities;
using Pelican.Presentation.GraphQL.Contacts;
using Xunit;

namespace Pelican.Presentation.GraphQL.Test;
public class GetContactsQueryUnitTest
{
	private ContactsQuery uut;
	[Fact]
	public void If_GetContacts_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken()
	{
		//Arrange
		uut = new ContactsQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		//Act
		_ = uut.GetContacts(mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(It.IsAny<GetContactsQuery>(), cancellationToken), Times.Once());
	}
	[Fact]
	public async void If_GetContactAsync_Is_Called_Mediator_Calls_Send_With_Correct_CancellationToken_And_Input()
	{
		//Arrange
		uut = new ContactsQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		Guid id = Guid.NewGuid();
		GetContactByIdQuery input = new GetContactByIdQuery(id);
		mediatorMock.Setup(x => x.Send(input, cancellationToken)).ReturnsAsync(new Contact(id));
		//Act
		var result = await uut.GetContactAsync(input.Id, mediatorMock.Object, cancellationToken);
		//Assert
		Assert.Equal(id, result.Id);
		mediatorMock.Verify(x => x.Send(input, cancellationToken), Times.Once());
	}
}
