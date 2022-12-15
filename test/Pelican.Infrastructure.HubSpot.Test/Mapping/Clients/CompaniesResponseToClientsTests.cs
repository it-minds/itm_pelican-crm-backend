using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;
using Pelican.Infrastructure.HubSpot.Mapping.Clients;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Clients;

public class CompaniesResponseToClientsTests
{
	const string ID = "id";
	const string NAME = "name";
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly CancellationToken cancellationToken = new();

	readonly CompanyResponse response = new()
	{
		Properties = new()
		{
			HubSpotObjectId = ID,
			Name = NAME,
		}
	};

	readonly CompaniesResponse responses = new();

	[Fact]
	public async void ToClients_ArgResultsNull_ThrowException()
	{
		/// Arrange
		responses.Results = null!;

		/// Act
		var result = await Record.ExceptionAsync(() => responses.ToClients(_unitOfWorkMock.Object, cancellationToken));

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public async void ToClients_ArgResultsNotNull_ThrowNoException()
	{
		/// Arrange 
		responses.Results = new List<CompanyResponse>();

		/// Act
		var result = await Record.ExceptionAsync(() => responses.ToClients(_unitOfWorkMock.Object, cancellationToken));

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public async void ToClients_ArgResultsNotNullNotEmpty_ThrowNoException()
	{
		/// Arrange 
		responses.Results = new List<CompanyResponse>() { response };

		/// Act
		var result = await Record.ExceptionAsync(() => responses.ToClients(_unitOfWorkMock.Object, cancellationToken));

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public async void ToClients_SingleResponse_ReturnSingle()
	{
		/// Arrange
		responses.Results = new List<CompanyResponse>() { response };

		/// Act
		var result = await responses.ToClients(_unitOfWorkMock.Object, cancellationToken);

		/// Assert
		Assert.Equal(ID, result.First().SourceId);
		Assert.Equal(NAME, result.First().Name);
		Assert.Equal(Sources.HubSpot, result.First().Source);
	}
}
