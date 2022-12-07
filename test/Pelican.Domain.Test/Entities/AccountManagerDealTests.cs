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
			SourceId = DEAL_HUBSPOTID,
			Source = Sources.HubSpot,
		};

		AccountManager accountManager = new(Guid.NewGuid())
		{
			SourceId = ACCOUNTMANAGER_HUBSPOTID,
			Source = Sources.HubSpot
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
			result.SourceDealId);

		Assert.Equal(
			accountManager,
			result.AccountManager);

		Assert.Equal(
			accountManager.Id,
			result.AccountManagerId);

		Assert.Equal(
			ACCOUNTMANAGER_HUBSPOTID,
			result.SourceAccountManagerId);

		Assert.True(result.IsActive);
	}

	[Fact]
	public void Deactivate_IsActiveIsTrue_SetIsActiveToFalse()
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

	[Fact]
	public void Deactivate_IsActiveIsFalse_IsActiveStillFalse()
	{
		// Arrange
		AccountManagerDeal accountManagerDeal = new(Guid.NewGuid())
		{
			IsActive = false,
		};

		// Act
		accountManagerDeal.Deactivate();

		// Assert
		Assert.False(accountManagerDeal.IsActive);
	}
}
