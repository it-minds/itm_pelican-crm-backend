using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using Pelican.Application.Abstractions.Data;
using Pelican.Domain.Primitives;
using Pelican.Infrastructure.Persistence.Repositories;
using Xunit;
using static HotChocolate.ErrorCodes;

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
	public async void CreateAsync_ArgumentNotEmpty_AddCalled()
	{
		// Arrange
		Mock<Entity> entityMock = new();

		ValueTask<EntityEntry<Entity>> returnValue = new();

		_pelicanContextMock
			.Setup(p => p.Set<Entity>().AddAsync(It.IsAny<Entity>(), default))
			.Returns(returnValue);

		// Act
		var result = await _uut.CreateAsync(entityMock.Object, default);

		// Assert
		_pelicanContextMock.Verify(
			p => p.Set<Entity>().AddAsync(entityMock.Object, default),
			Times.Once);

		Assert.Equal(
			entityMock.Object,
			result);
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

	[Fact]
	public async void CreateRangeAsync_ArgumentNotEmpty_AddRangeCalled()
	{
		// Arrange
		IEnumerable<Entity> entities = new List<Entity>();

		_pelicanContextMock
			.Setup(p => p.Set<Entity>().AddRangeAsync(It.IsAny<IEnumerable<Entity>>(), default))
			.Returns(Task.CompletedTask);

		// Act
		var result = await _uut.CreateRangeAsync(entities, default);

		// Assert
		_pelicanContextMock.Verify(
			p => p.Set<Entity>().AddRangeAsync(entities, default),
			Times.Once);

		Assert.Equal(
			entities,
			result);
	}

	[Fact]
	public void Update_ArgumentNotEmpty_UpdateCalled()
	{
		// Arrange
		Mock<Entity> entityMock = new();

		Mock<DbSet<Entity>> dbSetMock = new();

		_pelicanContextMock
			.Setup(p => p.Set<Entity>())
			.Returns(dbSetMock.Object);

		// Act
		_uut.Update(entityMock.Object);

		// Assert
		_pelicanContextMock.Verify(
			p => p.Set<Entity>().Update(entityMock.Object),
			Times.Once);
	}

	[Fact]
	public void Update_ArgumentNotEmpty_RemoveCalled()
	{
		// Arrange
		Mock<Entity> entityMock = new();

		Mock<DbSet<Entity>> dbSetMock = new();

		_pelicanContextMock
			.Setup(p => p.Set<Entity>())
			.Returns(dbSetMock.Object);

		// Act
		_uut.Delete(entityMock.Object);

		// Assert
		_pelicanContextMock.Verify(
			p => p.Set<Entity>().Remove(entityMock.Object),
			Times.Once);
	}
}
