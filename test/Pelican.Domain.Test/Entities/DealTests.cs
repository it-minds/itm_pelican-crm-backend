using Bogus;
using Moq;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;
public class DealTests
{
	private readonly Deal _uut = new()
	{
		SourceId = "uutHubSpotId",
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
		_uut.Status = propertyValue;

		// Assert
		Assert.Equal(StringLengths.DealStatus, _uut.Status!.Length);
		Assert.Equal(propertyValue, _uut.Status);
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
		Assert.Equal(propertyValue[..(StringLengths.DealName - 3)] + "...", _uut.Name);
	}

	[Fact]
	public void SetDealStatus_DealStatusStringToLong_DealStatusShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.DealStatus * 2);

		// Act
		_uut.Status = propertyValue;

		// Assert
		Assert.Equal(StringLengths.DealStatus, _uut.Status!.Length);
		Assert.Equal(propertyValue[..(StringLengths.DealStatus - 3)] + "...", _uut.Status);
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
		Assert.Equal(propertyValue[..(StringLengths.DealDescription - 3)] + "...", _uut.Description);
	}

	[Fact]
	public void UpdateProperty_NoUpdates_ThrowsNoException()
	{
		/// Arrange
		string name = "";

		string value = "";

		Deal inputDeal = new();

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

		Deal inputDeal = new();

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

		string value = ticks.ToString();

		Deal inputDeal = new();

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

		Deal inputDeal = new();

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

		string value = ticks.ToString();

		Deal inputDeal = new();

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

		Deal inputDeal = new();

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

		string value = ticks.ToString();

		Deal inputDeal = new();

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

		Deal inputDeal = new();

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			value,
			returnDeal.Status);
	}

	[Fact]
	public void UpdateProperty_DealDescriptionNotToLongUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		string name = "description";
		string value = "newDescription";

		Deal inputDeal = new();

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

		Deal inputDeal = new();

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
		Assert.Equal(StringLengths.DealStatus, _uut.Status!.Length);
		Assert.Equal("...", _uut.Status[(StringLengths.DealStatus - 3)..]);
		Assert.Equal(propertyValue[..(StringLengths.DealStatus - 3)], _uut.Status[..(StringLengths.DealStatus - 3)]);
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
		Assert.Equal("...", _uut.Description[(StringLengths.DealDescription - 3)..]);
		Assert.Equal(propertyValue[..(StringLengths.DealDescription - 3)], _uut.Description[..(StringLengths.DealDescription - 3)]);
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
		Assert.Equal("...", _uut.Name[(StringLengths.DealName - 3)..]);
		Assert.Equal(propertyValue[..(StringLengths.DealName - 3)], _uut.Name[..(StringLengths.DealName - 3)]);
	}

	[Fact]
	public void UpdateAccountManager_AccountManagerDealsEmptyArgNull_EmptyAccountManagerDeals()
	{
		// Arrange
		Deal inputDeal = new();

		// Act
		inputDeal.UpdateAccountManager(null);

		// Assert
		Assert.Equal(
			0,
			inputDeal.AccountManagerDeals.Count);
	}

	[Fact]
	public void UpdateAccountManager_ActiveAccountManagerDealEmptyArgNull_NullActiveAccountManagerDeal()
	{
		// Arrange
		Deal inputDeal = new();
		inputDeal.AccountManagerDeals.Add(new() { IsActive = true });

		// Act
		inputDeal.UpdateAccountManager(null);

		// Assert
		Assert.Null(inputDeal.ActiveAccountManagerDeal);

		Assert.Equal(
			1,
			inputDeal.AccountManagerDeals.Count);
	}

	[Fact]
	public void UpdateAccountManager_AccountManagerDealsEmptyArgNewAccountManagerDeal_AccountManagerDealAddedAsActive()
	{
		// Arrange
		Deal inputDeal = new();

		AccountManager accountManager = new();

		// Act
		inputDeal.UpdateAccountManager(accountManager);

		// Assert
		Assert.Equal(
			accountManager,
			inputDeal.ActiveAccountManagerDeal!.AccountManager);

		Assert.Equal(
			1,
			inputDeal.AccountManagerDeals.Count);
	}

	[Fact]
	public void UpdateAccountManager_AccountManagerDealsNotEmptyArgNewAccountManagerDeal_AccountManagerDealAddedAsActive()
	{
		// Arrange
		Deal inputDeal = new()
		{
			AccountManagerDeals = new List<AccountManagerDeal>()
			{
				new()
				{
					IsActive = true,
					SourceAccountManagerId = "first",
				},
			}
		};

		AccountManager accountManager = new() { SourceId = "new" };

		// Act
		inputDeal.UpdateAccountManager(accountManager);

		// Assert
		Assert.Equal(
			accountManager,
			inputDeal.ActiveAccountManagerDeal!.AccountManager);

		Assert.Equal(
			2,
			inputDeal.AccountManagerDeals.Count);
	}

	[Fact]
	public void SetAccountManager_AccountManagerDealsEmptyArgNull_EmptyAccountManagerDeals()
	{
		// Arrange
		Deal inputDeal = new();

		// Act
		inputDeal.SetAccountManager(null);

		// Assert
		Assert.Equal(
			0,
			inputDeal.AccountManagerDeals.Count);
	}

	[Fact]
	public void SetAccountManager_AccountManagerDealsEmptyArgNewAccountManagerDeal_AccountManagerDealAddedAsActive()
	{
		// Arrange
		Deal inputDeal = new();

		AccountManager accountManager = new();

		// Act
		inputDeal.SetAccountManager(accountManager);

		// Assert
		Assert.Equal(
			accountManager,
			inputDeal.ActiveAccountManagerDeal!.AccountManager);

		Assert.Equal(
			1,
			inputDeal.AccountManagerDeals.Count);
	}

	[Fact]
	public void SetContacts_DealContactsEmptyArgsNull_DealContactsEmpty()
	{
		// Arrange
		Deal inputDeal = new();

		// Act
		inputDeal.SetContacts(null);

		// Assert
		Assert.Equal(
			0,
			inputDeal.DealContacts.Count);
	}

	[Fact]
	public void SetContacts_DealContactsEmptyArgsEmptyList_DealContactsEmpty()
	{
		// Arrange
		Deal inputDeal = new();

		// Act
		inputDeal.SetContacts(new List<Contact>());

		// Assert
		Assert.Equal(
			0,
			inputDeal.DealContacts.Count);
	}

	[Fact]
	public void SetContacts_DealContactsEmptyArgsNonEmptyList_DealContactsSet()
	{
		// Arrange
		Deal inputDeal = new();

		// Act
		inputDeal.SetContacts(new List<Contact>() { new() });

		// Assert
		Assert.Equal(
			1,
			inputDeal.DealContacts.Count);
	}

	[Fact]
	public void SetContacts_DealContactsEmptyArgsListContainingNull_DealContactsSet()
	{
		// Arrange
		Deal inputDeal = new();

		// Act
		inputDeal.SetContacts(new List<Contact?>() { new(), null });

		// Assert
		Assert.Equal(
			1,
			inputDeal.DealContacts.Count);
	}

	[Fact]
	public void SetClient_ArgNull_ClientAndClientIdNull()
	{
		// Arrange
		Deal inputDeal = new();

		// Act
		inputDeal.SetClient(null);

		// Assert
		Assert.Null(inputDeal.Client);
		Assert.Null(inputDeal.ClientId);
	}

	[Fact]
	public void SetClient_ArgNewClient_ClientAndClientIdSet()
	{
		// Arrange
		Deal inputDeal = new();

		Client client = new();
		// Act
		inputDeal.SetClient(client);

		// Assert
		Assert.Equal(
			client,
			inputDeal.Client);

		Assert.Equal(
			client.Id,
			inputDeal.ClientId);
	}

	[Theory]
	[InlineData(12, 12, "testDealStatus", 12, "testName", "testDescription")]
	public void UpdatePropertiesFromDeal_PropertiesSet(
		long testEndDate,
		long testStartDate,
		string testDealStatus,
		long testLastContactDate,
		string testName,
		string testDescription)
	{
		//Arrange
		Mock<Deal> dealMock = new();
		dealMock.Object.EndDate = testEndDate;
		dealMock.Object.StartDate = testStartDate;
		dealMock.Object.Status = testDealStatus;
		dealMock.Object.LastContactDate = testLastContactDate;
		dealMock.Object.Name = testName;
		dealMock.Object.Description = testDescription;

		//Act
		_uut.UpdatePropertiesFromDeal(dealMock.Object);

		//Assert
		Assert.Equal(testEndDate, _uut.EndDate);
		Assert.Equal(testStartDate, _uut.StartDate);
		Assert.Equal(testDealStatus, _uut.Status);
		Assert.Equal(testLastContactDate, _uut.LastContactDate);
		Assert.Equal(testName, _uut.Name);
		Assert.Equal(testDescription, _uut.Description);
	}
}
