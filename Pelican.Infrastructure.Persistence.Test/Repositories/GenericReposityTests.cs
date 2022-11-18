using Microsoft.EntityFrameworkCore;
using Moq;
using Pelican.Application.Abstractions.Data;
using Pelican.Domain.Primitives;
using Pelican.Infrastructure.Persistence.Repositories;
using Xunit;

namespace Pelican.Infrastructure.Persistence.Test.Repositories;

public class GenericReposityTests
{
	private readonly GenericRepository<Entity> _uut;

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

	[Fact]
	public void GenericRepository_NullArgument_ThrowNoException()
	{
		// Arrange
		Mock<IPelicanContext> iPelicanContextMock = new();

		// Act
		var result = Record.Exception(() => new GenericRepository<Entity>(iPelicanContextMock.Object));

		// Assert
		Assert.Null(result);
	}
}
