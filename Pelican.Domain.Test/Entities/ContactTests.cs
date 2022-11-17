using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;

public class ContactTests
{
	private readonly Contact _uut = new Contact(Guid.NewGuid())
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
			"Invalid field",
			result.Message);
	}

	[Fact]
	public void UpdateProperty_FirstnameUpdated_FirstnameEqualsValue()
	{
		// Arragne
		string propertyName = "firstname";
		string propertyValue = "newName";

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(
			propertyValue,
			_uut.Firstname);
	}

	[Fact]
	public void UpdateProperty_LastnameUpdated_LastnameEqualsValue()
	{
		// Arragne
		string propertyName = "lastname";
		string propertyValue = "newName";

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(
			propertyValue,
			_uut.Lastname);
	}

	[Fact]
	public void UpdateProperty_EmailUpdated_EmailEqualsValue()
	{
		// Arragne
		string propertyName = "email";
		string propertyValue = "newEmail";

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(
			propertyValue,
			_uut.Email);
	}

	[Fact]
	public void UpdateProperty_PhoneUpdated_PhoneNumberEqualsValue()
	{
		// Arragne
		string propertyName = "phone";
		string propertyValue = "12345678";

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(
			propertyValue,
			_uut.PhoneNumber);
	}

	[Fact]
	public void UpdateProperty_MobilephoneUpdated_PhoneNumberEqualsValue()
	{
		// Arragne
		string propertyName = "mobilephone";
		string propertyValue = "12345678";

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(
			propertyValue,
			_uut.PhoneNumber);
	}

	[Fact]
	public void UpdateProperty_MobilephoneUpdated_JobTitleEqualsValue()
	{
		// Arragne
		string propertyName = "jobtitle";
		string propertyValue = "jobtitle";

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(
			propertyValue,
			_uut.JobTitle);
	}

	[Fact]
	public void UpdateProperty_OwnerUpdated_OwnerEqualsValue()
	{
		// Arragne
		string propertyName = "hs_all_owner_ids";
		string propertyValue = "owner";

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(
			propertyValue,
			_uut.HubSpotOwnerId);
	}

	[Fact]
	public void UpdateDealContacts_ArgumentNull_ReturnsWithoutException()
	{
		// Act 
		var result = Record.Exception(() => _uut.UpdateDealContacts(null));

		// Assert
		Assert.Null(result);

		Assert.Empty(_uut.DealContacts);
	}

	[Fact]
	public void UpdateDealContacts_EmptyExistingDealContactArgumentNotNull_NewDealContactAdded()
	{
		// Arrange
		Deal newDeal = new(Guid.NewGuid())
		{
			HubSpotId = "newHubSpotId",
		};

		ICollection<DealContact> dealContacts = new List<DealContact>()
		{
			DealContact.Create(newDeal, _uut),
		};

		// Act 
		_uut.UpdateDealContacts(dealContacts);

		// Assert
		Assert.Equal(
			1,
			_uut.DealContacts.Count);
	}

	[Fact]
	public void UpdateDealContacts_OneExistingDealContactNotInArgument_DealContactsUpdated()
	{
		// Arrange
		Deal existingDeal = new(Guid.NewGuid())
		{
			HubSpotId = "hsId",
		};

		_uut.DealContacts.Add(DealContact.Create(existingDeal, _uut));

		ICollection<DealContact> dealContacts = new List<DealContact>()
		{
			new(Guid.NewGuid())
			{
				Deal=new(Guid.NewGuid()),
			}
		};

		// Act 
		_uut.UpdateDealContacts(dealContacts);

		// Assert
		Assert.False(_uut.DealContacts.First(d => d.HubSpotDealId == existingDeal.HubSpotId).IsActive);

		Assert.Equal(
			2,
			_uut.DealContacts.Count);
	}

	[Fact]
	public void UpdateDealContacts_OneExistingDealContactMatchInArgument_NoDealContactAdded()
	{
		// Arrange
		Deal existingDeal = new(Guid.NewGuid())
		{
			HubSpotId = "hsId",
		};

		_uut.DealContacts.Add(DealContact.Create(existingDeal, _uut));

		Deal newDeal = new(Guid.NewGuid())
		{
			HubSpotId = "hsId",
		};

		ICollection<DealContact> newDealContacts = new List<DealContact>()
		{
			DealContact.Create(newDeal, _uut),
		};

		// Act 
		_uut.UpdateDealContacts(newDealContacts);

		// Assert
		Assert.Equal(
			1,
			_uut.DealContacts.Count);
	}

	[Fact]
	public void UpdateDealContacts_OneExistingDealContactMatchInArgument_DealContactStillActive()
	{
		// Arrange
		Deal existingDeal = new(Guid.NewGuid())
		{
			HubSpotId = "hsId",
		};

		_uut.DealContacts.Add(DealContact.Create(existingDeal, _uut));

		Deal newDeal = new(Guid.NewGuid())
		{
			HubSpotId = "hsId",
		};

		ICollection<DealContact> newDealContacts = new List<DealContact>()
		{
			DealContact.Create(newDeal, _uut),
		};

		// Act 
		_uut.UpdateDealContacts(newDealContacts);

		// Assert
		Assert.True(_uut
			.DealContacts
			.First(dc => dc.HubSpotDealId == "hsId")
			.IsActive);
	}

	[Fact]
	public void FillOutAssociations_ClientsAndDealsNull_ThrowNoExceptionEmptyClientContacts()
	{
		// Act
		var result = Record.Exception(() => _uut.FillOutAssociations(null, null));

		// Assert
		Assert.Null(result);

		Assert.Empty(_uut.ClientContacts);
	}

	[Fact]
	public void FillOutAssociations_ClientsEmpty_EmptyClientContacts()
	{
		// Act
		_uut.FillOutAssociations(Enumerable.Empty<Client>(), null);

		// Assert
		Assert.Empty(_uut.ClientContacts);
	}

	[Fact]
	public void FillOutAssociations_ExistingClientNotMatchingArgument_EmptyClientContacts()
	{
		// Arrange
		ClientContact existingClientContact = new(Guid.NewGuid())
		{
			HubSpotClientId = "hsID",
			Contact = _uut,
			ContactId = _uut.Id,
			HubSpotContactId = _uut.HubSpotId,
			IsActive = true,
		};

		_uut.ClientContacts.Add(existingClientContact);

		Client newClient = new(Guid.NewGuid())
		{
			HubSpotId = "another_hsId",
		};

		// Act
		_uut.FillOutAssociations(new List<Client>() { newClient }, null);

		// Assert
		Assert.Empty(_uut.ClientContacts);
	}

	[Fact]
	public void FillOutAssociations_ExistingClientMatchingArgument_ClientContactsUpdated()
	{
		// Arrange
		ClientContact existingClientContact = new(Guid.NewGuid())
		{
			HubSpotClientId = "hsID",
			Contact = _uut,
			ContactId = _uut.Id,
			HubSpotContactId = _uut.HubSpotId,
			IsActive = true,
		};

		_uut.ClientContacts.Add(existingClientContact);

		Client newClient = new(Guid.NewGuid())
		{
			HubSpotId = "hsID",
		};

		// Act
		_uut.FillOutAssociations(new List<Client>() { newClient }, null);

		// Assert
		Assert.Equal(
			1,
			_uut.ClientContacts.Count);

		Assert.Equal(
			newClient,
			_uut.ClientContacts.First().Client);

		Assert.Equal(
			newClient.Id,
			_uut.ClientContacts.First().ClientId);
	}

	[Fact]
	public void FillOutAssociations_DealsNull_EmptyDealContacts()
	{
		// Act
		_uut.FillOutAssociations(null, null);

		// Assert
		Assert.Empty(_uut.DealContacts);
	}

	[Fact]
	public void FillOutAssociations_DealsEmpty_EmptyDealContacts()
	{
		// Act
		_uut.FillOutAssociations(null, Enumerable.Empty<Deal>());

		// Assert
		Assert.Empty(_uut.DealContacts);
	}

	[Fact]
	public void FillOutAssociations_ExistingDealNotMatchingArgument_EmptyDealContacts()
	{
		// Arrange
		DealContact existingDealContact = new(Guid.NewGuid())
		{
			HubSpotDealId = "hsID",
			Contact = _uut,
			ContactId = _uut.Id,
			HubSpotContactId = _uut.HubSpotId,
			IsActive = true,
		};

		_uut.DealContacts.Add(existingDealContact);

		Deal newDeal = new(Guid.NewGuid())
		{
			HubSpotId = "another_hsId",
		};

		// Act
		_uut.FillOutAssociations(null, new List<Deal>() { newDeal });

		// Assert
		Assert.Empty(_uut.DealContacts);
	}

	[Fact]
	public void FillOutAssociations_ExistingDealMatchingArgument_DealContactsUpdated()
	{
		// Arrange
		DealContact existingDealContact = new(Guid.NewGuid())
		{
			HubSpotDealId = "hsID",
			Contact = _uut,
			ContactId = _uut.Id,
			HubSpotContactId = _uut.HubSpotId,
			IsActive = true,
		};

		_uut.DealContacts.Add(existingDealContact);

		Deal newDeal = new(Guid.NewGuid())
		{
			HubSpotId = "hsID",
		};

		// Act
		_uut.FillOutAssociations(null, new List<Deal>() { newDeal });

		// Assert
		Assert.Equal(
			1,
			_uut.DealContacts.Count);

		Assert.Equal(
			newDeal,
			_uut.DealContacts.First().Deal);

		Assert.Equal(
			newDeal.Id,
			_uut.DealContacts.First().DealId);
	}
}
