using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;
using Pelican.Infrastructure.HubSpot.Mapping.Deals;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping;

public class DealResponseToDealTests
{
	private const string ID = "id";
	private const string DEALSTAGE = "dealstage";
	private const string AMOUNTSTRING = "10";
	private const decimal AMOUNTDECIMAL = 10;
	private const string OWNERID = "ownerid";

	private readonly DealResponse response = new()
	{
		Properties = new()
		{
			Dealstage = DEALSTAGE,
			CloseDate = DateTime.Today,
			HubSpotObjectId = ID,
			Amount = AMOUNTSTRING,
			HubspotOwnerId = OWNERID,
		},
	};

	[Fact]
	public void ToDeal_WithOutAssociationsAndAmount_ReturnDealWithEmptyDealContactsAndClient()
	{
		/// Arrange
		response.Properties.Amount = "";

		/// Act
		Deal result = response.ToDeal();

		/// Assert
		Assert.Equal(ID, result.HubSpotId);
		Assert.Equal(DEALSTAGE, result.DealStatus);
		Assert.Equal(DateTime.Today, result.EndDate);
		Assert.Equal(null, result.Revenue);
		Assert.Equal(OWNERID, result.HubSpotOwnerId);

		Assert.Equal(0, result.DealContacts.Count);

		Assert.Equal(1, result.AccountManagerDeals.Count);
		Assert.Equal(OWNERID, result.AccountManagerDeals.First().HubSpotAccountManagerId);
		Assert.Equal(result, result.AccountManagerDeals.First().Deal);
		Assert.Equal(result.Id, result.AccountManagerDeals.First().DealId);
		Assert.Equal(result.HubSpotId, result.AccountManagerDeals.First().HubSpotDealId);
	}


	[Fact]
	public void ToDeal_WithEmptyAssociations_ReturnClientWithoutDealsAndClientContacts()
	{

		/// Arrange
		response.Associations = new()
		{
			Companies = null,
			Contacts = null,
		};

		/// Act
		Deal result = response.ToDeal();

		/// Assert
		/// Assert
		Assert.Equal(ID, result.HubSpotId);
		Assert.Equal(DEALSTAGE, result.DealStatus);
		Assert.Equal(DateTime.Today, result.EndDate);
		Assert.Equal(AMOUNTDECIMAL, result.Revenue);
		Assert.Equal(OWNERID, result.HubSpotOwnerId);

		Assert.Equal(null, result.Client);

		Assert.Equal(0, result.DealContacts.Count);
	}

	[Fact]
	public void ToDeal_WithNotMatchingAssociations_ReturnClientWithoutDealsAndClientContacts()
	{
		/// Arrange
		response.Associations = new()
		{
			Companies = new()
			{
				AssociationList = new List<Association>()
					{
						new()
						{
							Type = "deal_to_company_unlabeled",
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
		Deal result = response.ToDeal();

		/// Assert
		/// Assert
		Assert.Equal(ID, result.HubSpotId);
		Assert.Equal(DEALSTAGE, result.DealStatus);
		Assert.Equal(DateTime.Today, result.EndDate);
		Assert.Equal(AMOUNTDECIMAL, result.Revenue);
		Assert.Equal(OWNERID, result.HubSpotOwnerId);

		Assert.Equal(null, result.Client);

		Assert.Equal(0, result.DealContacts.Count);
	}



	[Fact]
	public void ToDeal_WithAssociations_ReturnDealWithDealContactsAndClient()
	{
		/// Arrange
		response.Associations = new()
		{
			Companies = new()
			{
				AssociationList = new List<Association>()
					{
						new()
						{
							Type = "deal_to_company",
							Id = "1"
						},
						new()
						{
							Type = "deal_to_company_unlabeled",
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
							Type = "deal_to_contact",
							Id = "1"
						},
						new()
						{
							Type = "deal_to_contact_unlabeled",
							Id = "2"
						},
					},
			}
		};

		/// Act
		Deal result = response.ToDeal();

		/// Assert
		Assert.Equal(ID, result.HubSpotId);
		Assert.Equal(DEALSTAGE, result.DealStatus);
		Assert.Equal(DateTime.Today, result.EndDate);
		Assert.Equal(AMOUNTDECIMAL, result.Revenue);
		Assert.Equal(OWNERID, result.HubSpotOwnerId);

		Assert.Equal(1, result.DealContacts.Count);
		Assert.Equal("1", result.DealContacts.First().HubSpotContactId);
		Assert.Equal(result, result.DealContacts.First().Deal);
		Assert.Equal(result.Id, result.DealContacts.First().DealId);
		Assert.Equal(result.HubSpotId, result.AccountManagerDeals.First().HubSpotDealId);

		Assert.Equal(1, result.AccountManagerDeals.Count);
		Assert.Equal(OWNERID, result.AccountManagerDeals.First().HubSpotAccountManagerId);
		Assert.Equal(result, result.AccountManagerDeals.First().Deal);
		Assert.Equal(result.Id, result.AccountManagerDeals.First().DealId);
		Assert.Equal(result.HubSpotId, result.AccountManagerDeals.First().HubSpotDealId);

		Assert.True(result.Client is not null);
		Assert.Equal("1", result.Client.HubSpotId);
		Assert.Equal(1, result.Client.Deals.Count);
		Assert.Equal(result, result.Client.Deals.First());
		Assert.Equal(result.Id, result.Client.Deals.First().Id);
	}
}
