using Bogus;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;
using Pelican.Infrastructure.HubSpot.Mapping.Contacts;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Contacts;

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
			FirstName = FIRSTNAME,
			LastName = LASTNAME,
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

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public void ToContact_WithoutAssociations_ReturnCorrectProperties()
	{
		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(FIRSTNAME, result.FirstName);
		Assert.Equal(LASTNAME, result.LastName);
		Assert.Equal(EMAIL, result.Email);
		Assert.Equal(PHONE, result.PhoneNumber);
		Assert.Equal(ID, result.SourceId);
		Assert.Equal(JOBTITLE, result.JobTitle);
		Assert.Equal(OWNERID, result.SourceOwnerId);
		Assert.Equal(Sources.HubSpot, result.Source);
	}

	[Fact]
	public void ToContact_WithoutAssociations_ReturnContactWithEmptyDealContacts()
	{
		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(0, result.DealContacts!.Count);
	}

	[Fact]
	public void ToContact_WithoutAssociations_ReturnContactWithEmptyClientContacts()
	{
		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public void ToContact_WithDefaultDeals_ReturnContactEmptyDealContacts()
	{
		/// Arrange
		response.Associations.Deals.AssociationList = new List<Association>()
		{
			new(),
		};

		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(0, result.DealContacts!.Count);
	}

	[Fact]
	public void ToContact_WithDefaultCompanies_ReturnContactEmptyClientContacts()
	{
		/// Arrange
		response.Associations.Companies.AssociationList = new List<Association>()
		{
			new(),
		};

		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public void ToContact_WithNotMatchingAssociations_ReturnContactEmptyDealContacts()
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

		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(0, result.DealContacts!.Count);
	}

	[Fact]
	public void ToContact_WithNotMatchingAssociations_ReturnContactEmptyClientContacts()
	{
		/// Arrange
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
		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public void ToContact_WithMatchingAssociations_ReturnContactWithDeals()
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

		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(1, result.DealContacts!.Count);
		Assert.Equal("1", result.DealContacts.First().SourceDealId);
		Assert.Equal(result, result.DealContacts.First().Contact);
		Assert.Equal(result.Id, result.DealContacts.First().ContactId);
	}

	[Fact]
	public void ToContact_FirstNameStringTooLong_FirstNameShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.FirstName = faker.Lorem.Letter(StringLengths.Name * 2);

		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(StringLengths.Name, result.FirstName!.Length);
		Assert.Equal("...", result.FirstName.Substring(StringLengths.Name - 3));
		Assert.Equal(response.Properties.FirstName.Substring(0, StringLengths.Name - 3), result.FirstName.Substring(0, StringLengths.Email - 3));
	}

	[Fact]
	public void ToContact_LastNameStringTooLong_LastNameShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.LastName = faker.Lorem.Letter(StringLengths.Name * 2);

		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(StringLengths.Name, result.LastName!.Length);
		Assert.Equal("...", result.LastName.Substring(StringLengths.Name - 3));
		Assert.Equal(response.Properties.LastName.Substring(0, StringLengths.Name - 3), result.LastName.Substring(0, StringLengths.Name - 3));
	}

	[Fact]
	public void ToContact_EmailStringTooLong_EmailShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.Email = faker.Lorem.Letter(StringLengths.Email * 2);

		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(StringLengths.Email, result.Email!.Length);
		Assert.Equal("...", result.Email.Substring(StringLengths.Email - 3));
		Assert.Equal(response.Properties.Email.Substring(0, StringLengths.Email - 3), result.Email.Substring(0, StringLengths.Email - 3));
	}

	[Fact]
	public void ToContact_PhoneStringTooLong_PhoneShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.Phone = faker.Lorem.Letter(StringLengths.PhoneNumber * 2);

		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(StringLengths.PhoneNumber, result.PhoneNumber!.Length);
		Assert.Equal("...", result.PhoneNumber.Substring(StringLengths.PhoneNumber - 3));
		Assert.Equal(response.Properties.Phone.Substring(0, StringLengths.PhoneNumber - 3), result.PhoneNumber.Substring(0, StringLengths.PhoneNumber - 3));
	}

	[Fact]
	public void ToContact_JobTitleStringTooLong_JobTitleShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.JobTitle = faker.Lorem.Letter(StringLengths.JobTitle * 2);

		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(StringLengths.JobTitle, result.JobTitle!.Length);
		Assert.Equal("...", result.JobTitle.Substring(StringLengths.JobTitle - 3));
		Assert.Equal(response.Properties.JobTitle.Substring(0, StringLengths.JobTitle - 3), result.JobTitle.Substring(0, StringLengths.JobTitle - 3));
	}

	[Fact]
	public void ToContact_WithMatchingAssociations_ReturnContactWithClientContacts()
	{
		/// Arrange
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
		Assert.Equal(1, result.ClientContacts!.Count);
		Assert.Equal("1", result.ClientContacts.First().SourceClientId);
		Assert.Equal(result, result.ClientContacts.First().Contact);
		Assert.Equal(result.Id, result.ClientContacts.First().ContactId);
		Assert.True(result.ClientContacts.First().IsActive);
	}
}
