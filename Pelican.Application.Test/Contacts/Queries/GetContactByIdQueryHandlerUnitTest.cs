﻿using Moq;
using Pelican.Application.Common.Interfaces.DataLoaders;
using Pelican.Application.Contacts.Queries.GetContactById;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.Contacts.Queries;
public class GetContactByIdQueryHandlerUnitTest
{
	private GetContactByIdQueryHandler uut;
	[Fact]
	public async void TestIfWhenHandleIsCalledDataLoaderIsCalledWithCorrectParameters()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<Contact>>();
		uut = new GetContactByIdQueryHandler(dataLoaderMock.Object);
		var cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		var getContactByIdQuery = new GetContactByIdQuery(guid);
		var resultList = new List<Contact>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new Contact(guid));
		//Act
		resultList.Add(await uut.Handle(getContactByIdQuery, cancellationToken));
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Once());
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
	[Fact]
	public async void TestIfWhenHandleIsCalledMultipleTimesDataLoaderIsCalledWithCorrectParametersMultipleTimes()
	{
		//Arrange
		var dataLoaderMock = new Mock<IGenericDataLoader<Contact>>();
		uut = new GetContactByIdQueryHandler(dataLoaderMock.Object);
		var cancellationToken = new CancellationToken();
		var guid = Guid.NewGuid();
		var getContactByIdQuery = new GetContactByIdQuery(guid);
		var resultList = new List<Contact>();
		dataLoaderMock.Setup(x => x.LoadAsync(guid, cancellationToken)).ReturnsAsync(new Contact(guid));
		//Act
		for (var i = 0; i < 50; i++)
		{
			resultList.Add(await uut.Handle(getContactByIdQuery, cancellationToken));
		}
		//Assert
		dataLoaderMock.Verify(x => x.LoadAsync(guid, cancellationToken), Times.Exactly(50));
		Assert.All(resultList, item => item.Id.Equals(guid));
	}
}