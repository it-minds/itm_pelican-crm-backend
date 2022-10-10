using MediatR;
using Moq;
using Pelican.Application.Contacts.Queries.GetContactById;
using Pelican.Application.Contacts.Queries.GetContacts;
using Pelican.Presentation.GraphQL.Contacts;
using Xunit;

namespace Pelican.Presentation.GraphQL.Test;
public class GetContactsQueryUnitTest
{
	private ContactsQuery uut;
	[Fact]
	public void IfGetContactsIsCalledMediatorCallsSendWithCorrectCancellationToken()
	{
		//Arrange
		uut = new ContactsQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		//Act
		uut.GetContacts(mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(It.IsAny<GetContactsQuery>(), cancellationToken), Times.Exactly(1));
	}
	[Fact]
	public void IfGetContactAsyncIsCalledMediatorCallsSendWithCorrectCancellationTokenAndInput()
	{
		//Arrange
		uut = new ContactsQuery();
		var mediatorMock = new Mock<IMediator>();
		CancellationToken cancellationToken = new CancellationToken();
		Guid id = Guid.NewGuid();
		GetContactByIdQuery input = new GetContactByIdQuery(id);
		//Act
		uut.GetContactAsync(input, mediatorMock.Object, cancellationToken);
		//Assert
		mediatorMock.Verify(x => x.Send(input, cancellationToken), Times.Exactly(1));
	}
}
