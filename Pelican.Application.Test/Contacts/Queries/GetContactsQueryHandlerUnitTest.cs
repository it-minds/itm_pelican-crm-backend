using Moq;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.Contacts.Queries.GetContacts;
using Pelican.Domain.Entities;
using Xunit;
namespace Pelican.Application.Test.Contacts.Queries;
public class GetContactsQueryHandlerUnitTest
{
	private GetContactsQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledRepositoryIsCalled()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var contactRepositoryMock = new Mock<IGenericRepository<Contact>>();
		unitOfWorkMock.Setup(x => x.ContactRepository).Returns(contactRepositoryMock.Object);
		uut = new GetContactsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetContactsQuery contactsQuery = new GetContactsQuery();
		//Act
		_ = uut.Handle(contactsQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.ContactRepository.FindAllWithIncludes(), Times.Once());
	}
}
