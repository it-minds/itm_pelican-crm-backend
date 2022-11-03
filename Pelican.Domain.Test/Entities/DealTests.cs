using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;
public class DealTests
{
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
		Assert.Null(exceptionResult);
	}

	[Fact]
	public void UpdateProperty_EndDateUpdatedInvalidValueFormat_ThrowsException()
	{
		/// Arrange
		string name = "closedate";

		string value = "Hello";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Exception exceptionResult = Record.Exception(() => inputDeal.UpdateProperty(name, value));

		/// Assert
		Assert.Equal(
			typeof(InvalidOperationException),
			exceptionResult.GetType());
	}

	[Fact]
	public void UpdateProperty_EndDateUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		DateTime date = new(2022, 11, 25);

		long ticks = 1669382373249; //Timestamp in milliseconds Friday, November 25, 2022 1:19:33.249 PM

		string name = "closedate";

		string value = ticks.ToString();

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			date,
			returnDeal.EndDate);
	}

	[Fact]
	public void UpdateProperty_RevenueUpdatedInvalidValueFormat_ThrowsException()
	{
		/// Arrange
		string name = "amount";

		string value = "hello";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Exception exceptionResult = Record.Exception(() => inputDeal.UpdateProperty(name, value));

		/// Assert
		Assert.Equal(
			typeof(InvalidOperationException),
			exceptionResult.GetType());
	}

	[Fact]
	public void UpdateProperty_RevenueUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		decimal value = (decimal)10.4;

		string name = "amount";

		string stringValue = value.ToString();

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, stringValue);

		/// Assert
		Assert.Equal(
			value,
			returnDeal.Revenue);
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
	public void UpdateProperty_CurrencyCodeUpdated_ReturnsUpdatedDeal()
	{
		/// Arrange
		string name = "deal_currency_code";

		string value = "newCurrencyCode";

		Deal inputDeal = new(Guid.NewGuid());

		/// Act
		Deal returnDeal = inputDeal.UpdateProperty(name, value);

		/// Assert
		Assert.Equal(
			value,
			returnDeal.CurrencyCode);
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
			inputDeal.AccountManagerDeals.Count);
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
	public void FillOutAccountmManager_EmptyAccountManangerDeals_NewAccountManagerAdded()
	{
		/// Arrange
		Deal inputDeal = new(Guid.NewGuid());

		AccountManager? accountManager = new(Guid.NewGuid());

		/// Act
		inputDeal.FillOutAccountmManager(accountManager);

		/// Assert
		Assert.Equal(
			1,
			inputDeal.AccountManagerDeals.Count);

		Assert.Equal(
			accountManager,
			inputDeal.AccountManagerDeals.First().AccountManager);
	}

	[Fact]
	public void FillOutAccountmManager_AccountMaangerDealExists_OldAccountManagerDeactivated()
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
		inputDeal.FillOutAccountmManager(newAccountManager);

		/// Assert
		Assert.False(inputDeal.AccountManagerDeals.First(a => a.AccountManager == oldAccountManager).IsActive);
	}

	[Fact]
	public void FillOutAccountmManager_AccountMaangerDealExists_NewAccountManagerAdded()
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
		inputDeal.FillOutAccountmManager(newAccountManager);

		/// Assert
		Assert.Equal(
			2,
			inputDeal.AccountManagerDeals.Count);

		Assert.Equal(
			newAccountManager,
			inputDeal.AccountManagerDeals.First(a => a.AccountManager == newAccountManager).AccountManager);
	}
}
