using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;

public class AccountManagerDealTests
{
	[Fact]
	public void Create_ReturnCorrectProperties()
	{
		// Arrange 
		const string DEAL_HUBSPOTID = "dealhubspotid";
		const string ACCOUNTMANAGER_HUBSPOTID = "accountmanagerhubspotid";

		Deal deal = new(Guid.NewGuid())
		{
			HubSpotId = DEAL_HUBSPOTID,
		};

		AccountManager accountManager = new(Guid.NewGuid())
		{
			HubSpotId = ACCOUNTMANAGER_HUBSPOTID,
		};

		// Act
		var result = AccountManagerDeal.Create(deal, accountManager);

		// Assert
		Assert.Equal(
			deal,
			result.Deal);

		Assert.Equal(
			deal.Id,
			result.DealId);

		Assert.Equal(
			DEAL_HUBSPOTID,
			result.HubSpotDealId);

		Assert.Equal(
			accountManager,
			result.AccountManager);

		Assert.Equal(
			accountManager.Id,
			result.AccountManagerId);

		Assert.Equal(
			ACCOUNTMANAGER_HUBSPOTID,
			result.HubSpotAccountManagerId);

		Assert.True(result.IsActive);
	}

	[Fact]
	public void Deactivate()
	{
		// Arrange
		AccountManagerDeal accountManagerDeal = new(Guid.NewGuid())
		{
			IsActive = true,
		};

		// Act
		accountManagerDeal.Deactivate();

		// Assert
		Assert.False(accountManagerDeal.IsActive);
	}
}
