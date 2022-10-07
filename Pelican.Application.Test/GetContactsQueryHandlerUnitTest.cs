using Moq;
using Pelican.Application.Contacts.Queries.GetContacts;
using Pelican.Domain.Repositories;
using Xunit;
namespace Pelican.Application.Test;
public class GetContacrsQueryHandlerUnitTest
{
	private GetContactsQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledRepositoryIsCalled()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var contactRepositoryMock = new Mock<IContactRepository>();
		unitOfWorkMock.Setup(x => x.ContactRepository).Returns(contactRepositoryMock.Object);
		uut = new GetContactsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetContactsQuery contactsQuery = new GetContactsQuery();
		//Act
		uut.Handle(contactsQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.ContactRepository.FindAll(), Times.Once());
	}
}
