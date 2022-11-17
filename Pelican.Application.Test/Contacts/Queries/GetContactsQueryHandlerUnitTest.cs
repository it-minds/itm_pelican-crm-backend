using Moq;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.Contacts.Queries.GetContacts;
using Pelican.Domain.Entities;
using Xunit;
namespace Pelican.Application.Test.Contacts.Queries;
public class GetContactsQueryHandlerUnitTest
{
	[Fact]
	public async void Test_If_When_Handle_Is_Called_Repository_Is_Called()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var contactRepositoryMock = new Mock<IGenericRepository<Contact>>();
		unitOfWorkMock.Setup(x => x.ContactRepository).Returns(contactRepositoryMock.Object);
		var uut = new GetContactsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetContactsQuery contactsQuery = new GetContactsQuery();
		//Act
		_ = await uut.Handle(contactsQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.ContactRepository.FindAll(), Times.Once());
	}
}
