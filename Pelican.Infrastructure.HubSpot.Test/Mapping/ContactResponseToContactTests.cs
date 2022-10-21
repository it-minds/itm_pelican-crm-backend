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
	private const string OBJECTID = "id";
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
			HubSpotObjectId = OBJECTID,
			JobTitle = JOBTITLE,
			HubSpotOwnerId = OWNERID,
		},
	};

	[Fact]
	public void ToContact_WithOutAssociations_ReturnContactWithEmptyDealContactsAndClientContacts()
	{
		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(FIRSTNAME, result.Firstname);
		Assert.Equal(LASTNAME, result.Lastname);
		Assert.Equal(EMAIL, result.Email);
		Assert.Equal(PHONE, result.PhoneNumber);
		Assert.Equal(OBJECTID, result.HubSpotId);
		Assert.Equal(JOBTITLE, result.JobTitle);
		Assert.Equal(OWNERID, result.HubSpotOwnerId);

		Assert.Equal(0, result.DealContacts.Count);

		Assert.Equal(0, result.ClientContacts.Count);
	}

	[Fact]
	public void ToContact_WithAssociations_ReturnClientWithDealsAndClientContacts()
	{

		/// Arrange
		response.Associations = new()
		{
			Deals = new()
			{
				AssociationList = new List<Association>()
					{
						new()
						{
							Type = "contact_to_deal",
							Id = "1"
						},
						new()
						{
							Type = "contact_to_deal_unlabeled",
							Id = "2"
						},
					},
			},
			Companies = new()
			{
				AssociationList = new List<Association>()
					{
						new()
						{
							Type = "contact_to_company",
							Id = "1"
						},
						new()
						{
							Type = "contact_to_company_unlabeled",
							Id = "2"
						},
					},
			}
		};

		/// Act
		Contact result = response.ToContact();

		/// Assert
		Assert.Equal(FIRSTNAME, result.Firstname);
		Assert.Equal(LASTNAME, result.Lastname);
		Assert.Equal(EMAIL, result.Email);
		Assert.Equal(PHONE, result.PhoneNumber);
		Assert.Equal(OBJECTID, result.HubSpotId);
		Assert.Equal(JOBTITLE, result.JobTitle);
		Assert.Equal(OWNERID, result.HubSpotOwnerId);

		Assert.Equal(1, result.DealContacts.Count);
		Assert.Equal("1", result.DealContacts.First().HubSpotDealId);
		Assert.Equal(result, result.DealContacts.First().Contact);
		Assert.Equal(result.Id, result.DealContacts.First().ContactId);
		Assert.True(result.DealContacts.First().IsActive);

		Assert.Equal(1, result.ClientContacts.Count);
		Assert.Equal("1", result.ClientContacts.First().HubSpotClientId);
		Assert.Equal(result, result.ClientContacts.First().Contact);
		Assert.Equal(result.Id, result.ClientContacts.First().ContactId);
		Assert.True(result.ClientContacts.First().IsActive);
	}
}
