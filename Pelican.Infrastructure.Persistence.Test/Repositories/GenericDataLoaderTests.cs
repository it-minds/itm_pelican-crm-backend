using GreenDonut;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain.Primitives;
using Pelican.Infrastructure.Persistence.DataLoader;
using Xunit;

namespace Pelican.Infrastructure.Persistence.Test.Repositories;

public class GenericDataLoaderTests
{
	[Fact]
	public void GenericDataLoader_ArgumentNull_ThrowsException()
	{
		// Arrange
		Mock<IBatchScheduler> batchSchedulerMock = new();

		// Act
		var result = Record.Exception(() => new GenericDataLoader<Entity>(
			batchSchedulerMock.Object,
			null!));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"unitOfWork",
			result.Message);
	}

	[Fact]
	public void GenericDataLoader_ArgumentNotNull_ThrowsNoException()
	{
		// Arrange
		Mock<IBatchScheduler> batchSchedulerMock = new();
		Mock<IUnitOfWork> unitOfWorkMock = new();

		// Act
		var result = Record.Exception(() => new GenericDataLoader<Entity>(
			batchSchedulerMock.Object,
			unitOfWorkMock.Object));

		// Assert
		Assert.Null(result);
	}
}
