using Moq;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Application.Contacts.Queries.GetContactById;
using Xunit;
namespace Pelican.Application.Test;
public class GetContactByIdQueryHandlerUnitTest
{
	private GetContactByIdQueryHandler uut;
	[Fact]
	public void TestIfWhenHandleIsCalledDataLoaderIsCalledWithCorrectParameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IContactByIdDataLoader>();
		uut = new GetContactByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetContactByIdQuery getContactByIdQuery = new GetContactByIdQuery(guid);
		//Act
		uut.Handle(getContactByIdQuery, cancellationToken);
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
	}
}
