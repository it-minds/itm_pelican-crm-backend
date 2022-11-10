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
		string name = "invalidName";
		string value = "value";

		// Act
		var result = Record.Exception(() => _uut.UpdateProperty(name, value));

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
		string name = "firstname";
		string value = "newName";

		// Act
		_uut.UpdateProperty(name, value);

		// Assert
		Assert.Equal(
		   value,
		   _uut.Firstname);
	}

	[Fact]
	public void UpdateProperty_LastnameUpdated_LastnameEqualsValue()
	{
		// Arragne
		string name = "lastname";
		string value = "newName";

		// Act
		_uut.UpdateProperty(name, value);

		// Assert
		Assert.Equal(
		   value,
		   _uut.Lastname);
	}

	[Fact]
	public void UpdateProperty_EmailUpdated_EmailEqualsValue()
	{
		// Arragne
		string name = "email";
		string value = "newEmail";

		// Act
		_uut.UpdateProperty(name, value);

		// Assert
		Assert.Equal(
		   value,
		   _uut.Email);
	}

	[Fact]
	public void UpdateProperty_PhoneUpdated_PhoneNumberEqualsValue()
	{
		// Arragne
		string name = "phone";
		string value = "phonenumber";

		// Act
		_uut.UpdateProperty(name, value);

		// Assert
		Assert.Equal(
		   value,
		   _uut.PhoneNumber);
	}

	[Fact]
	public void UpdateProperty_MobilephoneUpdated_PhoneNumberEqualsValue()
	{
		// Arragne
		string name = "mobilephone";
		string value = "mobilephonenumber";

		// Act
		_uut.UpdateProperty(name, value);

		// Assert
		Assert.Equal(
		   value,
		   _uut.PhoneNumber);
	}

	[Fact]
	public void UpdateProperty_MobilephoneUpdated_JobTitleEqualsValue()
	{
		// Arragne
		string name = "jobtitle";
		string value = "jobtitle";

		// Act
		_uut.UpdateProperty(name, value);

		// Assert
		Assert.Equal(
		   value,
		   _uut.JobTitle);
	}

	[Fact]
	public void UpdateProperty_OwnerUpdated_OwnerEqualsValue()
	{
		// Arragne
		string name = "hs_all_owner_ids";
		string value = "owner";

		// Act
		_uut.UpdateProperty(name, value);

		// Assert
		Assert.Equal(
		   value,
		   _uut.HubSpotOwnerId);
	}

	[Fact]
	public void UpdateDealContacts_ArgumentNull_ReturnsWithoutException()
	{
		// Act 
		var result = Record.Exception(() => _uut.UpdateDealContacts(null));

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void UpdateDealContacts_EmptyExistingDealContactEmptyArgument_DealContactsIsEmpty()
	{
		// Act 
		_uut.UpdateDealContacts(null);

		// Assert
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
	public void UpdateDealContacts_OneExistingDealContactNotInArgument_NewDealContactAdded()
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
		Assert.Equal(
		   2,
		   _uut.DealContacts.Count);
	}

	[Fact]
	public void UpdateDealContacts_OneExistingDealContactNotInArgument_OldDealContactDeactivated()
	{
		// Arrange
		Deal existingDeal = new(Guid.NewGuid())
		{
			HubSpotId = "hsId",
		};

		_uut.DealContacts.Add(DealContact.Create(existingDeal, _uut));

		Deal newDeal = new(Guid.NewGuid());
		ICollection<DealContact> newDealContacts = new List<DealContact>()
		{
			DealContact.Create(newDeal, _uut),
		};

		// Act 
		_uut.UpdateDealContacts(newDealContacts);

		// Assert
		Assert.False(_uut
			.DealContacts
			.First(dc => dc.HubSpotDealId == "hsId")
			.IsActive);
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
	public void FillOutAssociations_ClientsAndDealsNull_ThrowNoException()
	{
		// Act
		var result = Record.Exception(() => _uut.FillOutAssociations(null, null));

		// Assert
		Assert.Null(result);
	}

	[Fact]
	public void FillOutAssociations_ClientsNull_EmptyClientContacts()
	{
		// Act
		_uut.FillOutAssociations(null, null);

		// Assert
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
	public void FillOutAssociations_ExistingClientMatchingArgument_ClientContactsCountOne()
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
	}

	[Fact]
	public void FillOutAssociations_ExistingClientMatchingArgument_ClientContactsFilledWithClientAndClientId()
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
		Assert.Empty(_uut.ClientContacts);
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
	public void FillOutAssociations_ExistingDealMatchingArgument_DealContactsCountOne()
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
	}

	[Fact]
	public void FillOutAssociations_ExistingDealMatchingArgument_DealContactsFilledWithDealAndDealId()
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
			newDeal,
			_uut.DealContacts.First().Deal);

		Assert.Equal(
			newDeal.Id,
			_uut.DealContacts.First().DealId);
	}
}
