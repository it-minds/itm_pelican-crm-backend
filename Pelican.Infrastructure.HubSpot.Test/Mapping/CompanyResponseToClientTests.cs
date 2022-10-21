using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Mapping.Clients;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping;
public class CompanyResponseToClientTests
{
	private const string ID = "id";
	private const string NAME = "name";
	private const string LOCATION = "location";
	private const string SEGMENT = "segment";

	private readonly CompanyResponse response = new()
	{
		Properties = new()
		{
			Name = NAME,
			HubSpotObjectId = ID,
			City = LOCATION,
			Industry = SEGMENT,
		},
	};

	[Fact]
	public void ToClient_WithOutAssociations_ReturnClientWithEmptyDealsAndClientContacts()
	{
		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(SEGMENT, result.Segment);
		Assert.Equal(NAME, result.Name);
		Assert.Equal(LOCATION, result.OfficeLocation);
		Assert.Equal(ID, result.HubSpotId);

		Assert.Equal(0, result.Deals.Count);

		Assert.Equal(0, result.ClientContacts.Count);
	}

	[Fact]
	public void ToClient_WithEmptyAssociations_ReturnClientWithoutDealsAndClientContacts()
	{

		/// Arrange
		response.Associations = new()
		{
			Deals = null,
			Contacts = null,
		};

		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(SEGMENT, result.Segment);
		Assert.Equal(NAME, result.Name);
		Assert.Equal(LOCATION, result.OfficeLocation);
		Assert.Equal(ID, result.HubSpotId);

		Assert.Equal(0, result.Deals.Count);

		Assert.Equal(0, result.ClientContacts.Count);
	}

	[Fact]
	public void ToClient_WithNotMatchingAssociations_ReturnClientWithoutDealsAndClientContacts()
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
							Type = "contact_to_deal_unlabeled",
							Id = "2"
						},
					},
			},
			Contacts = new()
			{
				AssociationList = new List<Association>()
					{
						new()
						{
							Type = "contact_to_company_unlabeled",
							Id = "2"
						},
					},
			}
		};

		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(SEGMENT, result.Segment);
		Assert.Equal(NAME, result.Name);
		Assert.Equal(LOCATION, result.OfficeLocation);
		Assert.Equal(ID, result.HubSpotId);

		Assert.Equal(0, result.Deals.Count);

		Assert.Equal(0, result.ClientContacts.Count);
	}


	[Fact]
	public void ToClient_WithAssociations_ReturnClientWithDealsAndClientContacts()
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
							Type = "company_to_deal",
							Id = "1"
						},
						new()
						{
							Type = "company_to_deal_unlabeled",
							Id = "2"
						},
					},
			},
			Contacts = new()
			{
				AssociationList = new List<Association>()
					{
						new()
						{
							Type = "company_to_contact",
							Id = "1"
						},
						new()
						{
							Type = "company_to_contact_unlabeled",
							Id = "2"
						},
					},
			}
		};

		/// Act
		Client result = response.ToClient();

		/// Assert
		Assert.Equal(SEGMENT, result.Segment);
		Assert.Equal(NAME, result.Name);
		Assert.Equal(LOCATION, result.OfficeLocation);
		Assert.Equal(ID, result.HubSpotId);

		Assert.Equal(1, result.Deals.Count);
		Assert.Equal("1", result.Deals.First().HubSpotId);
		Assert.Equal(result, result.Deals.First().Client);
		Assert.Equal(result.Id, result.Deals.First().ClientId);

		Assert.Equal(1, result.ClientContacts.Count);
		Assert.Equal("1", result.ClientContacts.First().HubSpotContactId);
		Assert.Equal(result, result.ClientContacts.First().Client);
		Assert.Equal(result.Id, result.ClientContacts.First().ClientId);
		Assert.True(result.ClientContacts.First().IsActive);
	}
}
