using Bogus;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;
public class DealTests
{
	private readonly Deal _uut = new Deal(Guid.NewGuid())
	{
		HubSpotId = "uutHubSpotId",
	};

	[Fact]
	public void SetName_NameStringNotToLong_nameEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.DealName);

		// Act
		_uut.Name = propertyValue;

		// Assert
		Assert.Equal(StringLengths.DealName, _uut.Name!.Length);
		Assert.Equal(propertyValue, _uut.Name);
	}

	[Fact]
	public void SetDealStatus_DealStatusStringNotToLong_DealStatusEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.DealStatus);

		// Act
		_uut.DealStatus = propertyValue;

		// Assert
		Assert.Equal(StringLengths.DealStatus, _uut.DealStatus!.Length);
		Assert.Equal(propertyValue, _uut.DealStatus);
	}

	[Fact]
	public void SetDescription_DescriptionStringNotToLong_DescriptionEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.DealDescription);

		// Act
		_uut.Description = propertyValue;

		// Assert
		Assert.Equal(StringLengths.DealDescription, _uut.Description!.Length);
		Assert.Equal(propertyValue, _uut.Description);
	}



	[Fact]
	public void SetName_NameStringToLong_NameShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.DealName * 2);

		// Act
		_uut.Name = propertyValue;

		// Assert

		Assert.Equal(StringLengths.DealName, _uut.Name!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.DealName - 3) + "...", _uut.Name);
	}

	[Fact]
	public void SetDealStatus_DealStatusStringToLong_DealStatusShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.DealStatus * 2);

		// Act
		_uut.DealStatus = propertyValue;

		// Assert
		Assert.Equal(StringLengths.DealStatus, _uut.DealStatus!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.DealStatus - 3) + "...", _uut.DealStatus);
	}

	[Fact]
	public void SetDescription_DescriptionStringToLong_DescriptionShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.DealDescription * 2);

		// Act
		_uut.Description = propertyValue;

		// Assert
		Assert.Equal(StringLengths.DealDescription, _uut.Description!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.DealDescription - 3) + "...", _uut.Description);
	}

	[Fact]
	public void UpdateProperty_NoUpdates_ThrowsNoException()
	{
		/// Arrange
		string name = "";

		string value = "";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Exception exceptionResult = Record.Exception(() => inputDeal.UpdateProperty(name, value));

		/// Assert
		Assert.Equal(
			typeof(InvalidOperationException),
			exceptionResult.GetType());

		Assert.Equal(
			"Invalid field",
			exceptionResult.Message);
	}

	[Fact]
	public void UpdateProperty_EndDateUpdatedInvalidValueFormat_ThrowsInvalidOperationException()
	{
		/// Arrange
		string name = "enddate";

		string value = "Hello";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Exception exceptionResult = Record.Exception(() => inputDeal.UpdateProperty(name, value));

		/// Assert
		Assert.Equal(
			typeof(FormatException),
			exceptionResult.GetType());
	}

	[Fact]
	public void UpdateProperty_EndDateUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		DateTime date = new(2022, 11, 25);

		long ticks = date.Ticks;

		string name = "enddate";

		string value = date.ToString();

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			ticks,
			returnDeal.EndDate);
	}
	[Fact]
	public void UpdateProperty_StartDateUpdatedInvalidValueFormat_ThrowsInvalidOperationException()
	{
		/// Arrange
		string name = "startdate";

		string value = "Hello";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Exception exceptionResult = Record.Exception(() => inputDeal.UpdateProperty(name, value));

		/// Assert
		Assert.Equal(
			typeof(FormatException),
			exceptionResult.GetType());
	}

	[Fact]
	public void UpdateProperty_StartDateUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		DateTime date = new(2022, 11, 25);

		long ticks = date.Ticks;

		string name = "startdate";

		string value = date.ToString();

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			ticks,
			returnDeal.StartDate);
	}
	[Fact]
	public void UpdateProperty_LastContactedDateUpdatedInvalidValueFormat_ThrowsInvalidOperationException()
	{
		/// Arrange
		string name = "notes_last_contacted";

		string value = "Hello";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Exception exceptionResult = Record.Exception(() => inputDeal.UpdateProperty(name, value));

		/// Assert
		Assert.Equal(
			typeof(FormatException),
			exceptionResult.GetType());
	}

	[Fact]
	public void UpdateProperty_LastContactedDateUpdated_ReturnsUpdatedDeal()
	{
		// Arrange
		DateTime date = new(2022, 11, 25);

		long ticks = date.Ticks;

		string name = "notes_last_contacted";

		string value = date.ToString();

		Deal inputDeal = new(Guid.NewGuid());

		// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		// Assert
		Assert.Equal(
			ticks,
			returnDeal.LastContactDate);
	}

	[Fact]
	public void UpdateProperty_DealStatusUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		string name = "dealstage";
		string value = "newStatus";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			value,
			returnDeal.DealStatus);
	}

	[Fact]
	public void UpdateProperty_DealDescriptionNotToLongUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		string name = "description";
		string value = "newDescription";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			value,
			returnDeal.Description);
	}

	[Fact]
	public void UpdateProperty_DealNameUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		string name = "dealname";
		string value = "newName";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			value,
			returnDeal.Name);
	}

	[Fact]
	public void UpdateProperty_DealStatusStringToLongDealStatusShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyName = "dealstage";
		string propertyValue = faker.Lorem.Letter(StringLengths.DealStatus * 2);

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(StringLengths.DealStatus, _uut.DealStatus!.Length);
		Assert.Equal("...", _uut.DealStatus.Substring(StringLengths.DealStatus - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.DealStatus - 3), _uut.DealStatus.Substring(0, StringLengths.DealStatus - 3));
	}

	[Fact]
	public void UpdateProperty_DealDescriptionStringToLong_DealDescriptionShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyName = "description";
		string propertyValue = faker.Lorem.Letter(StringLengths.DealDescription * 2);

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(StringLengths.DealDescription, _uut.Description!.Length);
		Assert.Equal("...", _uut.Description.Substring(StringLengths.DealDescription - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.DealDescription - 3), _uut.Description.Substring(0, StringLengths.DealDescription - 3));
	}

	[Fact]
	public void UpdateProperty_DealNameStringToLong_DealNameShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyName = "dealname";
		string propertyValue = faker.Lorem.Letter(StringLengths.DealName * 2);

		// Act
		_uut.UpdateProperty(propertyName, propertyValue);

		// Assert
		Assert.Equal(StringLengths.DealName, _uut.Name!.Length);
		Assert.Equal("...", _uut.Name.Substring(StringLengths.DealName - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.DealName - 3), _uut.Name.Substring(0, StringLengths.DealName - 3));
	}

	[Fact]
	public void FillOutAssociations_NullAccountManager_EmptyAccountManagerDeals()
	{
		// Arrange
		Deal inputDeal = new(Guid.NewGuid());

		// Act
		inputDeal.FillOutAssociations(null, null, null);

		// Assert
		Assert.Equal(
			0,
			inputDeal.AccountManagerDeals.Count);
	}

	[Fact]
	public void FillOutAssociations_NullClient_NullClient()
	{
		// Arrange
		Deal inputDeal = new(Guid.NewGuid());

		// Act
		inputDeal.FillOutAssociations(null, null, null);

		// Assert
		Assert.Null(inputDeal.Client);
	}

	[Fact]
	public void FillOutAssociations_NullContacts_EmptyDealContacts()
	{
		// Arrange
		Deal inputDeal = new(Guid.NewGuid());

		// Act
		inputDeal.FillOutAssociations(null, null, null);

		// Assert
		Assert.Equal(
			0,
			inputDeal.DealContacts.Count);
	}

	[Fact]
	public void FillOutAssociations_WithAccountManager_AccountManagerDealsContainsAccountManager()
	{
		// Arrange
		Deal inputDeal = new(Guid.NewGuid());

		AccountManager accountManager = new(Guid.NewGuid());

		// Act
		inputDeal.FillOutAssociations(accountManager, null, null);

		// Assert
		Assert.Equal(
			accountManager,
			inputDeal.AccountManagerDeals.First().AccountManager);
	}

	[Fact]
	public void FillOutAssociations_WithClient_ClientAssignet()
	{
		// Arrange
		Deal inputDeal = new(Guid.NewGuid());

		Client client = new(Guid.NewGuid());

		// Act
		inputDeal.FillOutAssociations(null, client, null);

		// Assert
		Assert.Equal(
			client,
			inputDeal.Client);
	}

	[Fact]
	public void FillOutAssociations_WithEmptyContacts_ContactsAssignet()
	{
		// Arrange
		Deal inputDeal = new(Guid.NewGuid());

		List<Contact> contacts = new();

		// Act
		inputDeal.FillOutAssociations(null, null, contacts);

		// Assert
		Assert.Equal(
			0,
			inputDeal.DealContacts.Count);
	}

	[Fact]
	public void FillOutAssociations_WithContactsEmptyDealContacts_EmptyDealContacts()
	{
		// Arrange
		Deal inputDeal = new(Guid.NewGuid());

		List<Contact> contacts = new()
		{
			new Contact(Guid.NewGuid()),
		};

		// Act
		inputDeal.FillOutAssociations(null, null, contacts);

		// Assert
		Assert.Equal(
			0,
			inputDeal.DealContacts.Count);
	}

	[Fact]
	public void FillOutAssociations_WithContactsExistingInDealContacts_AssignedToDealContacts()
	{
		// Arrange
		Contact contact = new(Guid.NewGuid())
		{
			HubSpotId = "id",
		};

		Deal inputDeal = new(Guid.NewGuid());

		DealContact dealContact = new(Guid.NewGuid())
		{
			HubSpotContactId = contact.HubSpotId,
		};

		inputDeal.DealContacts.Add(dealContact);

		// Act
		inputDeal.FillOutAssociations(null, null, new List<Contact>() { contact });

		// Assert
		Assert.Equal(
			1,
			inputDeal.DealContacts.Count);

		Assert.Equal(
			contact,
			inputDeal.DealContacts.First().Contact);
	}

	[Fact]
	public void FillOutAssociations_WithContactsNotExistingInDealContacts_EmptyDealContacts()
	{
		// Arrange
		Contact contact = new(Guid.NewGuid())
		{
			HubSpotId = "id",
		};

		Deal inputDeal = new(Guid.NewGuid());

		DealContact dealContact = new(Guid.NewGuid())
		{
			HubSpotContactId = "another id",
		};

		inputDeal.DealContacts.Add(dealContact);

		// Act
		inputDeal.FillOutAssociations(null, null, new List<Contact>() { contact });

		// Assert
		Assert.Equal(
			0,
			inputDeal.DealContacts.Count);
	}

	[Fact]
	public void FillOutAccountManager_AccountManangerNull_ThrowsNoException()
	{
		/// Arrange
		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		var result = Record.Exception(() => inputDeal.FillOutAccountManager(null));

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public void FillOutAccountManager_EmptyAccountManangerDeals_NewAccountManagerAdded()
	{
		/// Arrange
		Deal inputDeal = new(Guid.NewGuid());

		AccountManager? accountManager = new(Guid.NewGuid());

		/// Act
		inputDeal.FillOutAccountManager(accountManager);

		/// Assert
		Assert.Equal(
			1,
			inputDeal.AccountManagerDeals.Count);

		Assert.Equal(
			accountManager,
			inputDeal.AccountManagerDeals.First().AccountManager);
	}

	[Fact]
	public void FillOutAccountManager_AccountManagerDealExists_OldAccountManagerDeactivated()
	{
		// Arrange
		Deal inputDeal = new(Guid.NewGuid());

		AccountManager? oldAccountManager = new(Guid.NewGuid())
		{
			HubSpotId = "old",
		};

		inputDeal.AccountManagerDeals = new List<AccountManagerDeal>()
		{
			AccountManagerDeal.Create(inputDeal,oldAccountManager),
		};

		AccountManager? newAccountManager = new(Guid.NewGuid())
		{
			HubSpotId = "new",
		};

		/// Act
		inputDeal.FillOutAccountManager(newAccountManager);

		/// Assert
		Assert.False(inputDeal.AccountManagerDeals.First(a => a.AccountManager == oldAccountManager).IsActive);
	}

	[Fact]
	public void FillOutAccountManager_AccountManagerDealExists_NewAccountManagerAdded()
	{
		/// Arrange
		Deal inputDeal = new(Guid.NewGuid());

		AccountManager? oldAccountManager = new(Guid.NewGuid())
		{
			HubSpotId = "old",
		};

		inputDeal.AccountManagerDeals = new List<AccountManagerDeal>()
		{
			AccountManagerDeal.Create(inputDeal,oldAccountManager),
		};

		AccountManager? newAccountManager = new(Guid.NewGuid())
		{
			HubSpotId = "new",
		};

		/// Act
		inputDeal.FillOutAccountManager(newAccountManager);

		/// Assert
		Assert.Equal(
			2,
			inputDeal.AccountManagerDeals.Count);

		Assert.Equal(
			newAccountManager,
			inputDeal.AccountManagerDeals.First(a => a.AccountManager == newAccountManager).AccountManager);
	}

	[Fact]
	public void FillOutAccountManager_SameAccountMaangerDealAlreadyExists_NewAccountManagerAdded()
	{
		/// Arrange
		Deal inputDeal = new(Guid.NewGuid());

		AccountManager? oldAccountManager = new(Guid.NewGuid())
		{
			HubSpotId = "old",
		};

		inputDeal.AccountManagerDeals = new List<AccountManagerDeal>()
		{
			AccountManagerDeal.Create(inputDeal,oldAccountManager),
		};

		AccountManager? newAccountManager = new(Guid.NewGuid())
		{
			HubSpotId = "old",
		};

		/// Act
		inputDeal.FillOutAccountManager(newAccountManager);

		/// Assert
		Assert.Equal(
			1,
			inputDeal.AccountManagerDeals.Count);

		Assert.Equal(
			newAccountManager,
			inputDeal.AccountManagerDeals.First(a => a.AccountManager == newAccountManager).AccountManager);
	}
}
