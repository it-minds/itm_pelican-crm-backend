using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;

public class DealContactTests
{
	[Fact]
	public void Create_ReturnCorrectProperties()
	{
		// Arrange 
		const string DEAL_HUBSPOTID = "dealhubspotid";
		const string CONTACT_HUBSPOTID = "contacthubspotid";

		Deal deal = new()
		{
			SourceId = DEAL_HUBSPOTID,
			Source = Sources.HubSpot,
		};

		Contact contact = new()
		{
			SourceId = CONTACT_HUBSPOTID,
			Source = Sources.HubSpot,
		};

		// Act
		var result = DealContact.Create(deal, contact);

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
			contact,
			result.Contact);

		Assert.Equal(
			contact.Id,
			result.ContactId);

		Assert.Equal(
			CONTACT_HUBSPOTID,
			result.SourceContactId);

		Assert.True(result.IsActive);
	}

	[Fact]
	public void Deactivate_IsActiveIsTrue_SetIsActiveToFalse()
	{
		// Arrange
		DealContact dealContact = new()
		{
			IsActive = true,
		};

		// Act
		dealContact.Deactivate();

		// Assert
		Assert.False(dealContact.IsActive);
	}

	[Fact]
	public void Deactivate_IsActiveIsFalse_IsActiveStillFalse()
	{
		// Arrange
		DealContact dealContact = new()
		{
			IsActive = false,
		};

		// Act
		dealContact.Deactivate();

		// Assert
		Assert.False(dealContact.IsActive);
	}
}
