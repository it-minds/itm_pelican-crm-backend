using Moq;
using Pelican.Application.Abstractions.Data;
using Pelican.Domain.Primitives;
using Pelican.Infrastructure.Persistence.Repositories;
using Xunit;

namespace Pelican.Infrastructure.Persistence.Test.Repositories;

public class GenericReposityTests
{
	[Fact]
	public void GenericRepository_NullArgument_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new GenericRepository<Entity>(null!));

		// Assert
		Assert.Contains(
			"pelicanContext",
			result.Message);
	}

	public void GenericRepository_NullArgument_ThrowException()
	{
		// Arrange
		var contextMock = new Mock<PelicanContext>();

		// Act
		var result = Record.Exception(() => new GenericRepository<Entity>(contextMock.Object));

		// Assert
		Assert.Contains(
			"pelicanContext",
			result.Message);
	}
}
