using Moq;
using Pelican.Application.Abstractions.Data.DataLoaders;
using Pelican.Application.Contacts.Queries.GetContactById;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.Contacts.Queries;
public class GetContactByIdQueryHandlerUnitTest
{
	[Fact]
	public async void Test_If_When_Handle_Is_Called_DataLoader_Is_Called_With_Correct_Parameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<Contact>>();
		var uut = new GetContactByIdQueryHandler(dataLoaderMock.Object);
		CancellationToken cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		GetContactByIdQuery getContactByIdQuery = new GetContactByIdQuery(guid);
		List<Contact> resultList = new List<Contact>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new Contact(guid));
		//Act
		resultList.Add(await uut.Handle(getContactByIdQuery, cancellationToken));
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
}
