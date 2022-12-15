using Bogus;
using Moq;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;
public class ClientUnitTest
{
	private readonly Client _uut = new Client(Guid.NewGuid())
	{
		SourceId = "uutHubSpotId",
		Source = Sources.HubSpot
	};

	[Fact]
	public void SetName_NameStringNotToLong_NameEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Name);

		// Act
		_uut.Name = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Name, _uut.Name!.Length);
		Assert.Equal(propertyValue, _uut.Name);
	}

	[Fact]
	public void SetOfficeLocation_OfficeLocationStringNotToLong_LastNameEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.OfficeLocation);

		// Act
		_uut.OfficeLocation = propertyValue;

		// Assert
		Assert.Equal(StringLengths.OfficeLocation, _uut.OfficeLocation!.Length);
		Assert.Equal(propertyValue, _uut.OfficeLocation);
	}

	[Fact]
	public void SetWebsite_WebsiteStringNotToLong_WebsiteEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Url);

		// Act
		_uut.Website = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Url, _uut.Website!.Length);
		Assert.Equal(propertyValue, _uut.Website);
	}

	[Fact]
	public void SetName_NameStringToLong_NameShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Name * 2);

		// Act
		_uut.Name = propertyValue;

		// Assert

		Assert.Equal(StringLengths.Name, _uut.Name!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.Name - 3) + "...", _uut.Name);
	}

	[Fact]
	public void SetOfficeLocation_OfficeLocationStringToLong_OfficeLocationShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.OfficeLocation * 2);

		// Act
		_uut.OfficeLocation = propertyValue;

		// Assert
		Assert.Equal(StringLengths.OfficeLocation, _uut.OfficeLocation!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.OfficeLocation - 3) + "...", _uut.OfficeLocation);
	}

	[Fact]
	public void SetWebsite_WebsiteStringToLong_WebsiteShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Url * 2);

		// Act
		_uut.Website = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Url, _uut.Website!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.Url - 3) + "...", _uut.Website);
	}

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
	public void UpdateClientContacts_EmptyExistingClientContactArgumentNotNull_NewClientContactAdded()
	{
		// Arrange
		Contact newContact = new(Guid.NewGuid())
		{
			SourceId = "newHubSpotId",
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
			SourceId = "hsId",
			Source = Sources.HubSpot
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
		Assert.False(
			_uut.ClientContacts
				.First(
					d => d.SourceContactId == existingContact.SourceId
					&& existingContact.Source == Sources.HubSpot)
				.IsActive);

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
			SourceId = "hsId",
			Source = Sources.HubSpot,
		};

		_uut.ClientContacts.Add(ClientContact.Create(_uut, existingContact));

		Contact newContact = new(Guid.NewGuid())
		{
			SourceId = "hsId",
			Source = Sources.HubSpot,
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

		Assert.True(
			_uut.ClientContacts
				.First(
					dc => dc.SourceContactId == "hsId"
					&& dc.Contact.Source == Sources.HubSpot)
				.IsActive);
	}

	[Theory]
	[InlineData("testName", "testOfficeLocation", "testWebSite")]
	public void UpdatePropertiesFromClient_PropertiesSet(string testName, string testOfficeLocation, string testWebsite)
	{
		//Arrange
		Mock<Client> clientMock = new();
		clientMock.Object.Name = testName;
		clientMock.Object.OfficeLocation = testOfficeLocation;
		clientMock.Object.Website = testWebsite;

		//Act
		_uut.UpdatePropertiesFromClient(clientMock.Object);

		//Assert
		Assert.Equal(testName, _uut.Name);
		Assert.Equal(testOfficeLocation, _uut.OfficeLocation);
		Assert.Equal(testWebsite, _uut.Website);
	}

	[Fact]
	public void SetDeal_DealListEmpty_NewListDealCreated()
	{
		//Act
		_uut.SetDeals(null);

		//Assert
		Assert.Equal(new List<Deal>(), _uut.Deals);
	}

	[Fact]
	public void SetDeal_DealListNotEmpty_DealsEqualToDealList()
	{
		//Arrange
		List<Deal> deals = new List<Deal>()
		{
			new Deal()
			{
				Name = "testDealName",
			}
		};
		//Act
		_uut.SetDeals(deals);

		//Assert
		Assert.Equal(deals, _uut.Deals);
	}
	[Fact]
	public void SetClientContacts_ClientContactsListEmpty_NewListClientContactsCreated()
	{
		//Act
		_uut.SetClientContacts(null);

		//Assert
		Assert.Equal(new List<ClientContact>(), _uut.ClientContacts);
	}

	[Fact]
	public void SetClientContacts_ClientContactsListNotEmpty_ClientContactsDealsEqualToDealList()
	{
		//Arrange
		List<Contact> contacts = new()
		{
			new Contact()
			{
				FirstName = "testFirstName",
			}
		};
		//Act
		_uut.SetClientContacts(contacts);

		//Assert
		Assert.Equal(
			contacts.First().FirstName,
			_uut.ClientContacts.First().Contact.FirstName);
	}
}
