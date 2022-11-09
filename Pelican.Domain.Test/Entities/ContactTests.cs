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

	private DealContact GetExistingDealContact()
	{
		Deal existingDeal = new Deal(Guid.NewGuid())
		{
			HubSpotId = "hsId",
		};

		return DealContact.Create(existingDeal, _uut);
	}

	[Fact]
	public void UpdateDealContacts_OneExistingDealContactNotInArgument_NewDealContactAdded()
	{
		// Arrange
		_uut.DealContacts.Add(GetExistingDealContact());

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
		_uut.DealContacts.Add(GetExistingDealContact());

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
}
