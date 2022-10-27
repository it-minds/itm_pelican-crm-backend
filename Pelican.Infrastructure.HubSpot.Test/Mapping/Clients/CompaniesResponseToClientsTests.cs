using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;
using Pelican.Infrastructure.HubSpot.Mapping.Clients;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Clients;

public class CompaniesResponseToClientsTests
{
	const string ID = "id";
	const string NAME = "name";

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
	public void ToAccountManagers_ArgResultsNull_ThrowException()
	{
		/// Arrange
		responses.Results = null!;

		/// Act
		var result = Record.Exception(() => responses.ToClients());

		/// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public void ToAccountManagers_ArgResultsNotNull_ThrowNoException()
	{
		/// Arrange 
		responses.Results = new List<CompanyResponse>();

		/// Act
		var result = Record.Exception(() => responses.ToClients());

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public void ToAccountManagers_ArgResultsNotNullNotEmpty_ThrowNoException()
	{
		/// Arrange 
		responses.Results = new List<CompanyResponse>() { response };

		/// Act
		var result = Record.Exception(() => responses.ToClients());

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public void ToAccountManagers_SingleResponse_ReturnSingle()
	{
		/// Arrange
		responses.Results = new List<CompanyResponse>() { response };

		/// Act
		var result = responses.ToClients();

		/// Assert
		Assert.Equal(ID, result.First().HubSpotId);
		Assert.Equal(NAME, result.First().Name);
	}
}
