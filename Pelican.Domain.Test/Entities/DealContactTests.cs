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
		const string CONTACT_HUBSPOTID = "accountmanagerhubspotid";

		Deal deal = new(Guid.NewGuid())
		{
			HubSpotId = DEAL_HUBSPOTID,
		};

		Contact contact = new(Guid.NewGuid())
		{
			HubSpotId = CONTACT_HUBSPOTID,
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
			result.HubSpotDealId);

		Assert.Equal(
			contact,
			result.Contact);

		Assert.Equal(
			contact.Id,
			result.ContactId);

		Assert.Equal(
			CONTACT_HUBSPOTID,
			result.HubSpotContactId);

		Assert.True(result.IsActive);
	}
}
