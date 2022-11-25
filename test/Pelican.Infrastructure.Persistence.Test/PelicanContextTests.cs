using Microsoft.EntityFrameworkCore;
using Moq;
using Pelican.Domain.Primitives;
using Xunit;

namespace Pelican.Infrastructure.Persistence.Test;

public class PelicanContextTests
{
	[Fact]
	public void PelicanContext_ThrowNoException()
	{
		// Arrange
		Mock<DbContextOptions<PelicanContext>> optionsMock = new();
		optionsMock
			.Setup(o => o.ContextType.IsAssignableFrom(typeof(PelicanContext)))
			.Returns(true);

		// Act
		var result = Record.Exception(() => new PelicanContext(optionsMock.Object));

		// Assert
		Assert.Null(result);
	}
}
