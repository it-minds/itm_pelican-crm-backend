using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;
using Pelican.Infrastructure.HubSpot.Mapping.Deals;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Deals;

public class DealsResponseToDealsTests
{
	const string ID = "id";
	const string OWNERID = "ownerid";

	readonly DealResponse response = new()
	{
		Properties = new()
		{
			HubSpotObjectId = ID,
			HubSpotOwnerId = OWNERID,
		}
	};

	readonly DealsResponse responses = new();

	[Fact]
	public void ToAccountManagers_ArgResultsNull_ThrowException()
	{
		/// Arrange
		responses.Results = null!;

		/// Act
		var result = Record.Exception(() => responses.ToDeals());

		/// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public void ToAccountManagers_ArgResultsNotNull_ThrowNoException()
	{
		/// Arrange 
		responses.Results = new List<DealResponse>();

		/// Act
		var result = Record.Exception(() => responses.ToDeals());

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public void ToAccountManagers_ArgResultsNotNullNotEmpty_ThrowNoException()
	{
		/// Arrange 
		responses.Results = new List<DealResponse>() { response };

		/// Act
		var result = Record.Exception(() => responses.ToDeals());

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public void ToAccountManagers_SingleResponse_ReturnSingle()
	{
		/// Arrange
		responses.Results = new List<DealResponse>() { response };

		/// Act
		var result = responses.ToDeals();

		/// Assert
		Assert.Equal(ID, result.First().HubSpotId);
		Assert.Equal(OWNERID, result.First().HubSpotOwnerId);
	}
}
