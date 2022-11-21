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
	private readonly Mock<IPelicanContext> _pelicanContextMock = new();

	public GenericReposityTests()
	{
		_uut = new(_pelicanContextMock.Object);
	}

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

	//[Fact]
	//public void FindAll_SetCalledOnContext()
	//{
	//	// Arrange
	//	Mock<DbSet<Entity>> dbSetMock = new();

	//	_pelicanContextMock
	//		.Setup(p => p.Set<Entity>().AsNoTracking())
	//		.Returns(dbSetMock.Object.AsNoTracking());

	//	//dbSetMock
	//	//	.Setup(d => d.AsNoTracking())
	//	//	.Returns(dbSetMock.Object);

	//	// Act
	//	var result = _uut.FindAll();

	//	// Assert
	//	//_pelicanContextMock.Verify(
	//	//	p => p.Set<Entity>(),
	//	//	Times.Once);
	//}

	[Fact]
	public void GetByIdAsync_ArgumentEmpty_ThrowException()
	{
		// Act
		var result = Record.ExceptionAsync(() => _uut.GetByIdAsync(Guid.Empty, default));

		// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			result.Result.GetType());

		Assert.Contains(
			"id",
			result.Result.Message);
	}

	[Fact]
	public void CreateAsync_ArgumentEmpty_ThrowException()
	{
		// Act
		var result = Record.ExceptionAsync(() => _uut.CreateAsync(null!, default));

		// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			result.Result.GetType());

		Assert.Contains(
			"entity",
			result.Result.Message);
	}

	[Fact]
	public void CreateRangeAsync_ArgumentEmpty_ThrowException()
	{
		// Act
		var result = Record.ExceptionAsync(() => _uut.CreateRangeAsync(null!, default));

		// Assert
		Assert.Equal(
			typeof(ArgumentNullException),
			result.Result.GetType());

		Assert.Contains(
			"entities",
			result.Result.Message);
	}
}
