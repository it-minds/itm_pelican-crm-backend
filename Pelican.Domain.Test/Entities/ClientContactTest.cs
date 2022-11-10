using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;

public class ClientContactTest
{
	[Fact]
	public void Create_ReturnCorrectProperties()
	{
		// Arrange 
		const string CLIENT_HUBSPOTID = "clienthubspotid";
		const string CONTACT_HUBSPOTID = "contacthubspotid";

		Client client = new(Guid.NewGuid())
		{
			HubSpotId = CLIENT_HUBSPOTID,
		};

		Contact contact = new(Guid.NewGuid())
		{
			HubSpotId = CONTACT_HUBSPOTID,
		};

		// Act
		var result = ClientContact.Create(client, contact);

		// Assert
		Assert.Equal(
			client,
			result.Client);

		Assert.Equal(
			client.Id,
			result.ClientId);

		Assert.Equal(
			CLIENT_HUBSPOTID,
			result.HubSpotClientId);

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
