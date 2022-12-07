using Bogus;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Mapping.Clients;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Clients;
public class CompanyResponseToClientTests
{
	private const string ID = "id";
	private const string NAME = "name";
	private const string LOCATION = "location";

	private readonly CompanyResponse response = new()
	{
		Properties = new()
		{
			Name = NAME,
			HubSpotObjectId = ID,
			City = LOCATION,
		},
	};

	[Fact]
	public void ToClient_ResponseMissingHubSpotId_ThrowsException()
	{
		/// Arrange
		CompanyResponse defaultResponse = new();
		defaultResponse.Properties.Name = NAME;

		/// Act
		Exception result = Record.Exception(() => defaultResponse.ToClient());

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public void ToClient_ResponseMissingName_ThrowsException()
	{
		/// Arrange
		CompanyResponse defaultResponse = new();
		defaultResponse.Properties.HubSpotObjectId = ID;

		/// Act
		Exception result = Record.Exception(() => defaultResponse.ToClient());

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public void ToClient_WithoutAssociations_ReturnCorrectProperties()
	{
		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(NAME, result.Name);
		Assert.Equal(LOCATION, result.OfficeLocation);
		Assert.Equal(ID, result.SourceId);
		Assert.Equal(Sources.HubSpot, result.Source);
	}

	[Fact]
	public void ToClient_NameStringTooLong_NameShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.Name = faker.Lorem.Letter(StringLengths.Name * 2);

		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(StringLengths.Name, result.Name.Length);
		Assert.Equal("...", result.Name.Substring(StringLengths.Name - 3));
		Assert.Equal(response.Properties.Name.Substring(0, StringLengths.Name - 3), result.Name.Substring(0, StringLengths.Name - 3));
	}

	[Fact]
	public void ToClient_OfficeLocationStringTooLong_OfficeLocationShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.City = faker.Lorem.Letter(StringLengths.OfficeLocation * 2);

		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(StringLengths.OfficeLocation, result.OfficeLocation!.Length);
		Assert.Equal("...", result.OfficeLocation.Substring(StringLengths.OfficeLocation - 3));
		Assert.Equal(response.Properties.City.Substring(0, StringLengths.OfficeLocation - 3), result.OfficeLocation.Substring(0, StringLengths.OfficeLocation - 3));

	}

	[Fact]
	public void ToClient_DomainStringTooLong_DomainShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.Domain = faker.Lorem.Letter(StringLengths.Url * 2);

		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(StringLengths.Url, result.Website!.Length);
		Assert.Equal("...", result.Website.Substring(StringLengths.Url - 3));
		Assert.Equal(response.Properties.Domain.Substring(0, StringLengths.Url - 3), result.Website.Substring(0, StringLengths.Url - 3));
	}

	[Fact]
	public void ToClient_WithoutAssociations_ReturnClientWithEmptyDeals()
	{
		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(0, result.Deals!.Count);
	}

	[Fact]
	public void ToClient_WithoutAssociations_ReturnClientWithEmptyClientContacts()
	{
		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public void ToClient_WithDefaultDealsAndContacts_ReturnClientEmptyDeals()
	{
		/// Arrange
		response.Associations.Deals.AssociationList = new List<Association>()
		{
			new(),
		};

		response.Associations.Contacts.AssociationList = new List<Association>()
		{
			new(),
		};

		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(0, result.Deals!.Count);
	}

	[Fact]
	public void ToClient_WithDefaultDealsAndContacts_ReturnClientEmptyClientContacts()
	{
		/// Arrange
		response.Associations.Deals.AssociationList = new List<Association>()
		{
			new(),
		};

		response.Associations.Contacts.AssociationList = new List<Association>()
		{
			new(),
		};

		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public void ToClient_WithNotMatchingAssociations_ReturnClientEmptyDeals()
	{
		/// Arrange
		response.Associations.Deals.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "not_matching",
				Id = "2"
			},
		};

		response.Associations.Contacts.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "not_matching",
				Id = "2"
			},
		};

		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(0, result.Deals!.Count);
	}

	[Fact]
	public void ToClient_WithNotMatchingAssociations_ReturnClientEmptyClientContacts()
	{
		/// Arrange
		response.Associations.Deals.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "not_matching",
				Id = "2"
			},
		};

		response.Associations.Contacts.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "not_matching",
				Id = "2"
			},
		};

		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public void ToClient_WithMatchingAssociations_ReturnClientWithDeals()
	{
		/// Arrange
		response.Associations.Deals.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "company_to_deal",
				Id = "1"
			},
		};

		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(1, result.Deals!.Count);
		Assert.Equal("1", result.Deals.First().SourceId);
		Assert.Equal(result, result.Deals.First().Client);
		Assert.Equal(result.Id, result.Deals.First().ClientId);
	}

	[Fact]
	public void ToClient_WithMatchingAssociations_ReturnClientWithClientContacts()
	{
		/// Arrange
		response.Associations.Contacts.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "company_to_contact",
				Id = "1"
			},
		};

		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(1, result.ClientContacts!.Count);
		Assert.Equal("1", result.ClientContacts.First().SourceContactId);
		Assert.Equal(result, result.ClientContacts.First().Client);
		Assert.Equal(result.Id, result.ClientContacts.First().ClientId);
		Assert.True(result.ClientContacts.First().IsActive);
	}
}
