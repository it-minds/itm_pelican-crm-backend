using Bogus;
using Moq;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;

public class ContactTests
{
	private readonly Contact _uut = new()
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
		Assert.Equal(propertyValue[..(StringLengths.Name - 3)] + "...", _uut.FirstName);
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
		Assert.Equal(propertyValue[..(StringLengths.Name - 3)] + "...", _uut.LastName);
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
		Assert.Equal(propertyValue[..(StringLengths.Email - 3)] + "...", _uut.Email);
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
		Assert.Equal(propertyValue[..(StringLengths.PhoneNumber - 3)] + "...", _uut.PhoneNumber);
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
		Assert.Equal(propertyValue[..(StringLengths.JobTitle - 3)] + "...", _uut.JobTitle);
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
		Assert.Equal("...", _uut.FirstName[(StringLengths.Name - 3)..]);
		Assert.Equal(propertyValue[..(StringLengths.Name - 3)], _uut.FirstName[..(StringLengths.Name - 3)]);
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
		Assert.Equal("...", _uut.LastName[(StringLengths.Name - 3)..]);
		Assert.Equal(propertyValue[..(StringLengths.Name - 3)], _uut.LastName[..(StringLengths.Name - 3)]);
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
		Assert.Equal("...", _uut.Email[(StringLengths.Email - 3)..]);
		Assert.Equal(propertyValue[..(StringLengths.Email - 3)], _uut.Email[..(StringLengths.Email - 3)]);
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
		Assert.Equal("...", _uut.PhoneNumber[(StringLengths.PhoneNumber - 3)..]);
		Assert.Equal(propertyValue[..(StringLengths.PhoneNumber - 3)], _uut.PhoneNumber[..(StringLengths.PhoneNumber - 3)]);
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
		Assert.Equal("...", _uut.PhoneNumber[(StringLengths.PhoneNumber - 3)..]);
		Assert.Equal(propertyValue[..(StringLengths.PhoneNumber - 3)], _uut.PhoneNumber[..(StringLengths.PhoneNumber - 3)]);
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
		Assert.Equal("...", _uut.JobTitle[(StringLengths.JobTitle - 3)..]);
		Assert.Equal(propertyValue[..(StringLengths.JobTitle - 3)], _uut.JobTitle[..(StringLengths.JobTitle - 3)]);
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

	[Theory]
	[InlineData("testFirstName", "testLastName", "testEmail", "testPhoneNumber", "testJobTitle")]
	public void UpdatePropertiesFromContact_PropertiesSet(string testFirstName, string testLastName, string testEmail, string testPhoneNumber, string testJobtTitle)
	{
		//Arrange
		Mock<Contact> contactMock = new();
		contactMock.Object.FirstName = testFirstName;
		contactMock.Object.LastName = testLastName;
		contactMock.Object.Email = testEmail;
		contactMock.Object.PhoneNumber = testPhoneNumber;
		contactMock.Object.JobTitle = testJobtTitle;

		//Act
		_uut.UpdatePropertiesFromContact(contactMock.Object);

		//Assert
		Assert.Equal(testFirstName, _uut.FirstName);
		Assert.Equal(testLastName, _uut.LastName);
		Assert.Equal(testEmail, _uut.Email);
		Assert.Equal(testPhoneNumber, _uut.PhoneNumber);
		Assert.Equal(testJobtTitle, _uut.JobTitle);
	}

	[Fact]
	public void SetDealContacts_ArgsEmptyList_DealContactsEmpty()
	{
		// Arrange
		Contact input = new();

		// Act
		input.SetDealContacts(new List<Deal>());

		// Assert
		Assert.Empty(input.DealContacts);
	}

	[Fact]
	public void SetDealContacts_ArgsNonEmptyList_DealContactsSet()
	{
		// Arrange
		Contact input = new();

		// Act
		input.SetDealContacts(new List<Deal>() { new() });

		// Assert
		Assert.Equal(
			1,
			input.DealContacts.Count);
	}

	[Fact]
	public void SetDealContacts_ArgsListContainingNull_DealContactsSet()
	{
		// Arrange
		Contact input = new();

		// Act
		input.SetDealContacts(new List<Deal?>() { new(), null });

		// Assert
		Assert.Equal(
			1,
			input.DealContacts.Count);
	}

	[Fact]
	public void SetClientContacts_ArgsEmptyList_ClientContactsEmpty()
	{
		// Arrange
		Contact input = new();

		// Act
		input.SetClientContacts(new List<Client>());

		// Assert
		Assert.Empty(input.ClientContacts);
	}

	[Fact]
	public void SetClientContacts_ArgsNonEmptyList_ClientContactsSet()
	{
		// Arrange
		Contact input = new();

		// Act
		input.SetClientContacts(new List<Client>() { new() });

		// Assert
		Assert.Equal(
			1,
			input.ClientContacts.Count);
	}

	[Fact]
	public void SetClientContacts_ArgsListContainingNull_ClientContactsSet()
	{
		// Arrange
		Contact input = new();

		// Act
		input.SetClientContacts(new List<Client?>() { new(), null });

		// Assert
		Assert.Equal(
			1,
			input.ClientContacts.Count);
	}
}
