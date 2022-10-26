using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;
using Pelican.Infrastructure.HubSpot.Mapping.Contacts;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping;

public class ContactResponseToContactTests
{
	private const string FIRSTNAME = "fname";
	private const string LASTNAME = "lname";
	private const string EMAIL = "email";
	private const string PHONE = "phone";
	private const string ID = "id";
	private const string JOBTITLE = "jobtitle";
	private const string OWNERID = "ownerid";

	private readonly ContactResponse response = new()
	{
		Properties = new()
		{
			Firstname = FIRSTNAME,
			Lastname = LASTNAME,
			Email = EMAIL,
			Phone = PHONE,
			HubSpotObjectId = ID,
			JobTitle = JOBTITLE,
			HubSpotOwnerId = OWNERID,
		},
	};

	[Fact]
	public void ToContact_ResponseMissingHubSpotId_ThrowsException()
	{
		/// Arrange
		ContactResponse defaultResponse = new();
		defaultResponse.Properties.HubSpotOwnerId = OWNERID;

		/// Act
		Exception result = Record.Exception(() => defaultResponse.ToContact());

		/// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public void ToContact_ResponseMissingHubSpotOwnerId_ThrowsException()
	{
		/// Arrange
		ContactResponse defaultResponse = new();
		defaultResponse.Properties.HubSpotObjectId = ID;

		/// Act
		Exception result = Record.Exception(() => defaultResponse.ToContact());

		/// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public void ToContact_WithoutAssociations_ReturnCorrectProperties()
	{
		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(FIRSTNAME, result.Firstname);
		Assert.Equal(LASTNAME, result.Lastname);
		Assert.Equal(EMAIL, result.Email);
		Assert.Equal(PHONE, result.PhoneNumber);
		Assert.Equal(ID, result.HubSpotId);
		Assert.Equal(JOBTITLE, result.JobTitle);
		Assert.Equal(OWNERID, result.HubSpotOwnerId);
	}

	[Fact]
	public void ToContact_WithoutAssociations_ReturnContactWithEmptyDealContactsAndClientContacts()
	{
		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(0, result.DealContacts!.Count);

		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public void ToContact_WithDefaultDealsAndCompanies_ReturnContactEmptyDealContactsAndClientContacts()
	{
		/// Arrange
		response.Associations.Deals.AssociationList = new List<Association>()
		{
			new(),
		};

		response.Associations.Companies.AssociationList = new List<Association>()
		{
			new(),
		};

		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(0, result.DealContacts!.Count);

		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public void ToContact_WithNotMatchingAssociations_ReturnContactEmptyDealContactsAndContactContacts()
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

		response.Associations.Companies.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "not_matching",
				Id = "2"
			},
		};

		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(0, result.DealContacts!.Count);

		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public void ToContact_WithMatchingAssociations_ReturnContactWithDealsAndContactContacts()
	{
		/// Arrange
		response.Associations.Deals.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "contact_to_deal",
				Id = "1"
			},
		};

		response.Associations.Companies.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "contact_to_company",
				Id = "1"
			},
		};

		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(1, result.DealContacts!.Count);
		Assert.Equal("1", result.DealContacts.First().HubSpotDealId);
		Assert.Equal(result, result.DealContacts.First().Contact);
		Assert.Equal(result.Id, result.DealContacts.First().ContactId);

		Assert.Equal(1, result.ClientContacts!.Count);
		Assert.Equal("1", result.ClientContacts.First().HubSpotClientId);
		Assert.Equal(result, result.ClientContacts.First().Contact);
		Assert.Equal(result.Id, result.ClientContacts.First().ContactId);
		Assert.True(result.ClientContacts.First().IsActive);
	}
}
