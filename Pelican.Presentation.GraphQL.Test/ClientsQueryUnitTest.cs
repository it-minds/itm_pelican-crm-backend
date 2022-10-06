using HotChocolate.Execution.Configuration;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pelican.Application.Common.Interfaces;
using Pelican.Presentation.GraphQL.Clients;
using Xunit;
namespace Pelican.Presentation.GraphQL.Test;

public class ClientsQueryUnitTest
{
	private readonly ClientsQuery uut;
	private readonly IRequestExecutorBuilder _serviceCollectionMock;
	private readonly IDbContextFactory<Infrastructure.Persistence.IDbContext> _contextFactoryMock;
	private readonly IPelicanContext _contextMock;
	private ClientsQueryUnitTest()
	{
		uut = new ClientsQuery();
		_serviceCollectionMock = new Mock<IRequestExecutorBuilder>().Object;
		_contextFactoryMock = new Mock<IDbContextFactory<Infrastructure.Persistence.IDbContext>>().Object;
		_contextMock = new Mock<IPelicanContext>().Object;
	}
	[Fact]
	public void Test1()
	{
		uut.GetClientAsync(Guid.NewGuid(), )
	}
}
