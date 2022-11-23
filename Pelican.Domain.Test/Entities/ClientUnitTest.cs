using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;
public class ClientUnitTest
{
	private readonly Client _uut = new Client(Guid.NewGuid())
	{
		HubSpotId = "uutHubSpotId",
	};

	[Fact]
	public void UpdateProperty_InvalidPropertyName_ThrowInvalidOperationException()
	{
		// Arragne
		string propertyName = "invalidName";
		string propertyValue = "value";

		// Act
		var result = Record.Exception(() => _uut.UpdateProperty(propertyName, propertyValue));

		// Assert
		Assert.Equal(
			typeof(InvalidOperationException),
			result.GetType());

		Assert.Equal(
			$"{propertyName} is not a valid property on Client",
			result.Message);
	}

	[Fact]
	public void UpdateProperty_NameUpdated_NameEqualsValue()
	{
		// Arragne
		string propertyName = "name";
		string propertyValue = "newName";

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(
			propertyValue,
			_uut.Name);
	}

	[Fact]
	public void UpdateProperty_OfficeLocationUpdated_OfficeLocationEqualsValue()
	{
		// Arragne
		string propertyName = "city";
		string propertyValue = "newCity";

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(
			propertyValue,
			_uut.OfficeLocation);
	}

	[Fact]
	public void UpdateProperty_WebsiteUpdated_WebsiteEqualsValue()
	{
		// Arragne
		string propertyName = "website";
		string propertyValue = "newWebsite";

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(
			propertyValue,
			_uut.Website);
	}

	[Fact]
	public void UpdateClientContacts_ArgumentNull_ReturnsWithoutException()
	{
		// Act 
		var result = Record.Exception(() => _uut.UpdateClientContacts(null));

		// Assert
		Assert.Null(result);

		Assert.Empty(_uut.ClientContacts);
	}

	[Fact]
	public void UpdateClientContacts_EmptyExistingClientContactArgumentNotNull_NewClientContactAdded()
	{
		// Arrange
		Contact newContact = new(Guid.NewGuid())
		{
			HubSpotId = "newHubSpotId",
		};

		ICollection<ClientContact> clientContacts = new List<ClientContact>()
		{
			ClientContact.Create(_uut, newContact),
		};

		// Act 
		_uut.UpdateClientContacts(clientContacts);

		// Assert
		Assert.Equal(
			1,
			_uut.ClientContacts.Count);
	}

	[Fact]
	public void UpdateClientContacts_OneExistingClientContactNotInArgument_ClientContactsUpdated()
	{
		// Arrange
		Contact existingContact = new(Guid.NewGuid())
		{
			HubSpotId = "hsId",
		};

		_uut.ClientContacts.Add(ClientContact.Create(_uut, existingContact));

		ICollection<ClientContact> clientContacts = new List<ClientContact>()
		{
			new(Guid.NewGuid())
			{
				Contact=new(Guid.NewGuid()),
			}
		};

		// Act 
		_uut.UpdateClientContacts(clientContacts);

		// Assert
		Assert.False(_uut.ClientContacts.First(d => d.HubSpotContactId == existingContact.HubSpotId).IsActive);

		Assert.Equal(
			2,
			_uut.ClientContacts.Count);
	}

	[Fact]
	public void UpdateClientContacts_OneExistingClientContactMatchInArgument_NoClientContactAddedExistingClientContactStillActive()
	{
		// Arrange
		Contact existingContact = new(Guid.NewGuid())
		{
			HubSpotId = "hsId",
		};

		_uut.ClientContacts.Add(ClientContact.Create(_uut, existingContact));

		Contact newContact = new(Guid.NewGuid())
		{
			HubSpotId = "hsId",
		};

		ICollection<ClientContact> newClientContacts = new List<ClientContact>()
		{
			ClientContact.Create(_uut, newContact),
		};

		// Act 
		_uut.UpdateClientContacts(newClientContacts);

		// Assert
		Assert.Equal(
			1,
			_uut.ClientContacts.Count);

		Assert.True(_uut
			.ClientContacts
			.First(dc => dc.HubSpotContactId == "hsId")
			.IsActive);
	}

	[Fact]
	public void FillOutAssociations_ClientsAndDealsNull_ThrowNoExceptionEmptyClientContacts()
	{
		// Act
		var result = Record.Exception(() => _uut.FillOutClientContacts(null));

		// Assert
		Assert.Null(result);

		Assert.Empty(_uut.ClientContacts);
	}

	[Fact]
	public void FillOutAssociations_ClientsEmpty_EmptyClientContacts()
	{
		// Act
		_uut.FillOutClientContacts(Enumerable.Empty<Contact>());

		// Assert
		Assert.Empty(_uut.ClientContacts);
	}

	[Fact]
	public void FillOutAssociations_ExistingClientNotMatchingArgument_EmptyClientContacts()
	{
		// Arrange
		ClientContact existingClientContact = new(Guid.NewGuid())
		{
			Contact = new(Guid.NewGuid()),
			HubSpotContactId = "hsID",
			IsActive = true,
		};
		existingClientContact.ContactId = existingClientContact.Contact.Id;

		_uut.ClientContacts.Add(existingClientContact);

		Contact newContact = new(Guid.NewGuid())
		{
			HubSpotId = "another_hsId",
		};

		// Act
		_uut.FillOutClientContacts(new List<Contact>() { newContact });

		// Assert
		Assert.Equal(
			1,
			_uut.ClientContacts.Count);

		Assert.Equal(
			existingClientContact.Contact,
			_uut.ClientContacts.First().Contact);

		Assert.Equal(
			existingClientContact.ContactId,
			_uut.ClientContacts.First().Contact.Id);
	}

	[Fact]
	public void FillOutAssociations_ExistingClientMatchingArgument_ClientContactsUpdated()
	{
		// Arrange
		ClientContact existingClientContact = new(Guid.NewGuid())
		{
			HubSpotContactId = "hsID",
			Client = _uut,
			ClientId = _uut.Id,
			HubSpotClientId = _uut.HubSpotId,
			IsActive = true,
		};

		_uut.ClientContacts.Add(existingClientContact);

		Contact newContact = new(Guid.NewGuid())
		{
			HubSpotId = "hsID",
		};

		// Act
		_uut.FillOutClientContacts(new List<Contact>() { newContact });

		// Assert
		Assert.Equal(
			1,
			_uut.ClientContacts.Count);

		Assert.Equal(
			newContact,
			_uut.ClientContacts.First().Contact);

		Assert.Equal(
			newContact.Id,
			_uut.ClientContacts.First().Contact.Id);
	}
}
