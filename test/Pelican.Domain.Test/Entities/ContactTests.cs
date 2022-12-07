using Bogus;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;

public class ContactTests
{
	private readonly Contact _uut = new Contact(Guid.NewGuid())
	{
		SourceId = "uutHubSpotId",
		Source = Sources.HubSpot,
	};

	[Fact]
	public void SetFirstName_FirstNameStringNotToLong_FirstnameEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Name);

		// Act
		_uut.FirstName = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Name, _uut.FirstName!.Length);
		Assert.Equal(propertyValue, _uut.FirstName);
	}

	[Fact]
	public void SetLastName_LastNameStringNotToLong_LastNameEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Name);

		// Act
		_uut.LastName = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Name, _uut.LastName!.Length);
		Assert.Equal(propertyValue, _uut.LastName);
	}

	[Fact]
	public void SetEmail_EmailStringNotToLong_EmailEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Email);

		// Act
		_uut.Email = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Email, _uut.Email!.Length);
		Assert.Equal(propertyValue, _uut.Email);
	}

	[Fact]
	public void SetPhoneNumber_PhoneNumberStringNotToLong_PhoneNumberEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.PhoneNumber);

		// Act
		_uut.PhoneNumber = propertyValue;

		// Assert
		Assert.Equal(StringLengths.PhoneNumber, _uut.PhoneNumber!.Length);
		Assert.Equal(propertyValue, _uut.PhoneNumber);
	}

	[Fact]
	public void SetJobTitle_JobTitleStringNotToLong_JobTitleEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.JobTitle);

		// Act
		_uut.JobTitle = propertyValue;

		// Assert
		Assert.Equal(StringLengths.JobTitle, _uut.JobTitle!.Length);
		Assert.Equal(propertyValue, _uut.JobTitle);
	}

	[Fact]
	public void SetFirstName_FirstNameStringToLong_FirstnameShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Name * 2);

		// Act
		_uut.FirstName = propertyValue;

		// Assert

		Assert.Equal(StringLengths.Name, _uut.FirstName!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.Name - 3) + "...", _uut.FirstName);
	}

	[Fact]
	public void SetLastName_LastNameStringToLong_LastNameShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Name * 2);

		// Act
		_uut.LastName = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Name, _uut.LastName!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.Name - 3) + "...", _uut.LastName);
	}

	[Fact]
	public void SetEmail_EmailStringToLong_EmailShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Email * 2);

		// Act
		_uut.Email = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Email, _uut.Email!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.Email - 3) + "...", _uut.Email);
	}

	[Fact]
	public void SetPhoneNumber_PhoneNumberStringToLong_PhoneShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.PhoneNumber * 2);

		// Act
		_uut.PhoneNumber = propertyValue;

		// Assert
		Assert.Equal(StringLengths.PhoneNumber, _uut.PhoneNumber!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.PhoneNumber - 3) + "...", _uut.PhoneNumber);
	}

	[Fact]
	public void SetJobTitle_JobTitleStringToLong_JobTitleShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.JobTitle * 2);

		// Act
		_uut.JobTitle = propertyValue;

		// Assert
		Assert.Equal(StringLengths.JobTitle, _uut.JobTitle!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.JobTitle - 3) + "...", _uut.JobTitle);
	}

	[Fact]
	public void UpdateProperty_InvalidPropertyName_ThrowInvalidOperationException()
	{
		// Arrange
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
		// Arrange
		string propertyName = "firstname";
		string propertyValue = "newName";

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(
			propertyValue,
			_uut.FirstName);
	}

	[Fact]
	public void UpdateProperty_LastnameUpdated_LastnameEqualsValue()
	{
		// Arrange
		string propertyName = "lastname";
		string propertyValue = "newName";

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(
			propertyValue,
			_uut.LastName);
	}

	[Fact]
	public void UpdateProperty_EmailUpdated_EmailEqualsValue()
	{
		// Arrange
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
		// Arrange
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
		// Arrange
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
	public void UpdateProperty_JobTitleUpdated_JobTitleEqualsValue()
	{
		// Arrange
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
	public void UpdateProperty_FirstNameStringToLong_FirstnameShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyName = "firstname";
		string propertyValue = faker.Lorem.Letter(StringLengths.Name * 2);

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert

		Assert.Equal(StringLengths.Name, _uut.FirstName!.Length);
		Assert.Equal("...", _uut.FirstName.Substring(StringLengths.Name - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.Name - 3), _uut.FirstName.Substring(0, StringLengths.Name - 3));
	}

	[Fact]
	public void UpdateProperty_LastNameStringToLong_LastNameShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyName = "lastname";
		string propertyValue = faker.Lorem.Letter(StringLengths.Name * 2);

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(StringLengths.Name, _uut.LastName!.Length);
		Assert.Equal("...", _uut.LastName.Substring(StringLengths.Name - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.Name - 3), _uut.LastName.Substring(0, StringLengths.Name - 3));
	}

	[Fact]
	public void UpdateProperty_EmailStringToLong_EmailShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyName = "email";
		string propertyValue = faker.Lorem.Letter(StringLengths.Email * 2);

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(StringLengths.Email, _uut.Email!.Length);
		Assert.Equal("...", _uut.Email.Substring(StringLengths.Email - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.Email - 3), _uut.Email.Substring(0, StringLengths.Email - 3));
	}

	[Fact]
	public void UpdateProperty_PhoneStringToLong_PhoneShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyName = "phone";
		string propertyValue = faker.Lorem.Letter(StringLengths.PhoneNumber * 2);

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(StringLengths.PhoneNumber, _uut.PhoneNumber!.Length);
		Assert.Equal("...", _uut.PhoneNumber.Substring(StringLengths.PhoneNumber - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.PhoneNumber - 3), _uut.PhoneNumber.Substring(0, StringLengths.PhoneNumber - 3));
	}

	[Fact]
	public void UpdateProperty_MobilePhoneStringToLong_MobilePhoneShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyName = "mobilephone";
		string propertyValue = faker.Lorem.Letter(StringLengths.PhoneNumber * 2);

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(StringLengths.PhoneNumber, _uut.PhoneNumber!.Length);
		Assert.Equal("...", _uut.PhoneNumber.Substring(StringLengths.PhoneNumber - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.PhoneNumber - 3), _uut.PhoneNumber.Substring(0, StringLengths.PhoneNumber - 3));
	}

	[Fact]
	public void UpdateProperty_JobTitleStringToLong_JobTitleShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyName = "jobtitle";
		string propertyValue = faker.Lorem.Letter(StringLengths.JobTitle * 2);

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(StringLengths.JobTitle, _uut.JobTitle!.Length);
		Assert.Equal("...", _uut.JobTitle.Substring(StringLengths.JobTitle - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.JobTitle - 3), _uut.JobTitle.Substring(0, StringLengths.JobTitle - 3));
	}

	[Fact]
	public void UpdateProperty_OwnerUpdated_OwnerEqualsValue()
	{
		// Arrange
		string propertyName = "hs_all_owner_ids";
		string propertyValue = "owner";

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(
			propertyValue,
			_uut.SourceOwnerId);
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
			SourceId = "newHubSpotId",
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
			SourceId = "hsId",
			Source = Sources.HubSpot,
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
		Assert.False(_uut.DealContacts.First(d => d.SourceDealId == existingDeal.SourceId && d.Contact.Source == Sources.HubSpot).IsActive);

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
			SourceId = "hsId",
			Source = Sources.HubSpot,
		};

		_uut.DealContacts.Add(DealContact.Create(existingDeal, _uut));

		Deal newDeal = new(Guid.NewGuid())
		{
			SourceId = "hsId",
			Source = Sources.HubSpot,
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
			SourceId = "hsId",
			Source = Sources.HubSpot,
		};

		_uut.DealContacts.Add(DealContact.Create(existingDeal, _uut));

		Deal newDeal = new(Guid.NewGuid())
		{
			SourceId = "hsId",
			Source = Sources.HubSpot,
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
			.First(dc => dc.SourceDealId == "hsId" && dc.Contact.Source == Sources.HubSpot)
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
			SourceClientId = "hsID",
			Contact = _uut,
			ContactId = _uut.Id,
			SourceContactId = _uut.SourceId,
			IsActive = true,
		};

		_uut.ClientContacts.Add(existingClientContact);

		Client newClient = new(Guid.NewGuid())
		{
			SourceId = "another_hsId",
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
			SourceClientId = "hsID",
			Contact = _uut,
			ContactId = _uut.Id,
			SourceContactId = _uut.SourceId,
			IsActive = true,
		};

		_uut.ClientContacts.Add(existingClientContact);

		Client newClient = new(Guid.NewGuid())
		{
			SourceId = "hsID",
			Source = Sources.HubSpot,
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
			SourceDealId = "hsID",
			Contact = _uut,
			ContactId = _uut.Id,
			SourceContactId = _uut.SourceId,
			IsActive = true,
		};

		_uut.DealContacts.Add(existingDealContact);

		Deal newDeal = new(Guid.NewGuid())
		{
			SourceId = "another_hsId",
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
			SourceDealId = "hsID",
			Contact = _uut,
			ContactId = _uut.Id,
			SourceContactId = _uut.SourceId,
			IsActive = true,
		};

		_uut.DealContacts.Add(existingDealContact);

		Deal newDeal = new(Guid.NewGuid())
		{
			SourceId = "hsID",
			Source = Sources.HubSpot,
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
