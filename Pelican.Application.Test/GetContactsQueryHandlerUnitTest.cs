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
		_ = uut.Handle(contactsQuery, cancellationToken);
		//Assert
		unitOfWorkMock.Verify(x => x.ContactRepository.FindAll(), Times.Once());
	}
	[Fact]
	public void TestIfWhenHandleIsCalledMultipleTimesRepositoryIsCalledMultipleTimes()
	{
		//Arrange
		var unitOfWorkMock = new Mock<IUnitOfWork>();
		var contactRepositoryMock = new Mock<IContactRepository>();
		unitOfWorkMock.Setup(x => x.ContactRepository).Returns(contactRepositoryMock.Object);
		uut = new GetContactsQueryHandler(unitOfWorkMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		GetContactsQuery contactsQuery = new GetContactsQuery();
		//Act
		for (int i = 0; i < 50; i++)
		{
			_ = uut.Handle(contactsQuery, cancellationToken);
		}

		//Assert
		unitOfWorkMock.Verify(x => x.ContactRepository.FindAll(), Times.Exactly(50));
	}
}
