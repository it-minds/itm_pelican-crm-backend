using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Clients.Queries.GetClients;
using Pelican.Application.Clients.Queries.GetLocations;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.Clients.Queries;

public class GetLocationsQueryHandlerTests
{
	private readonly IQueryHandler<GetLocationsQuery, IQueryable<Client>> _uut;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IGenericRepository<Client>> _clientRepositoryMock = new();

	public GetLocationsQueryHandlerTests()
	{
		_unitOfWorkMock
			.Setup(x => x.ClientRepository)
			.Returns(_clientRepositoryMock.Object);

		_uut = new GetLocationsQueryHandler(_unitOfWorkMock.Object);
	}


	[Fact]
	public void GetLocationsQueryHandler_UnitOfWorkNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new GetLocationsQueryHandler(null!));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"unitOfWork",
			result.Message);
	}

	[Fact]
	public async void Handle_VerifyCallToRepository()
	{
		// Arrange
		GetLocationsQuery query = new();

		// Act
		var result = await _uut.Handle(query, default);

		// Assert
		_unitOfWorkMock.Verify(
			x => x.ClientRepository
				.FindByCondition(x => x.OfficeLocation != null),
			Times.Once);
	}
}
