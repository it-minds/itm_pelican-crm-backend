using Moq;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.Contacts.Queries.GetContacts;
using Pelican.Domain.Entities;
using Xunit;
namespace Pelican.Application.Test.Contacts.Queries;
public class GetContacrsQueryHandlerUnitTest
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
		var cancellationToken = new CancellationToken();
		var contactsQuery = new GetContactsQuery();
		//Act
		_ = uut.Handle(contactsQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.ContactRepository.FindAll(), Times.Once());
	}
	[Fact]
	public void TestIfWhenHandleIsCalledMultipleTimesRepositoryIsCalledMultipleTimes()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var contactRepositoryMock = new Mock<IGenericRepository<Contact>>();
		unitOfWorkMock.Setup(x => x.ContactRepository).Returns(contactRepositoryMock.Object);
		uut = new GetContactsQueryHandler(unitOfWorkMock.Object);
		var cancellationToken = new CancellationToken();
		var contactsQuery = new GetContactsQuery();
		//Act
		for (var i = 0; i < 50; i++)
		{
			_ = uut.Handle(contactsQuery, cancellationToken);
		}

		//Assert
		unitOfWorkMock.Verify(x => x.ContactRepository.FindAll(), Times.Exactly(50));
	}
}
