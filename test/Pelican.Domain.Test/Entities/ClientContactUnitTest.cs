using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;

public class ClientContactUnitTest
{
	[Fact]
	public void Create_ReturnCorrectProperties()
	{
		// Arrange 
		const string CLIENT_HUBSPOTID = "clienthubspotid";
		const string CONTACT_HUBSPOTID = "contacthubspotid";

		Client client = new()
		{
			SourceId = CLIENT_HUBSPOTID,
			Source = Sources.HubSpot
		};

		Contact contact = new()
		{
			SourceId = CONTACT_HUBSPOTID,
			Source = Sources.HubSpot
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
			result.SourceClientId);

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
	public void Deactive_IsActiveSetToFalse()
	{
		// Arrange 
		ClientContact clientContact = new()
		{
			IsActive = true,
		};

		// Act
		clientContact.Deactivate();

		// Assert
		Assert.False(clientContact.IsActive);
	}
}
