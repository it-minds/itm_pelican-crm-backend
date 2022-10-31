using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;
using Pelican.Infrastructure.HubSpot.Mapping.Deals;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Deals;

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
			HubSpotOwnerId = OWNERID,
		},
	};

	[Fact]
	public void ToDeal_ResponseMissingHubSpotId_ThrowsException()
	{
		/// Arrange
		DealResponse defaultResponse = new();
		defaultResponse.Properties.HubSpotOwnerId = OWNERID;

		/// Act
		Exception result = Record.Exception(() => defaultResponse.ToDeal());

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public void ToDeal_ResponseMissingHubSpotOwnerId_ThrowsException()
	{
		/// Arrange
		DealResponse defaultResponse = new();
		defaultResponse.Properties.HubSpotObjectId = ID;

		/// Act
		Exception result = Record.Exception(() => defaultResponse.ToDeal());

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public void ToDeal_ResponseInvalidAmountFormat_ThrowsException()
	{
		/// Arrange
		response.Properties.Amount = "invalidFormat";

		/// Act
		Exception result = Record.Exception(() => response.ToDeal());

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(InvalidOperationException),
			result.GetType());
	}

	[Fact]
	public void ToDeal_WithoutAssociations_ReturnCorrectProperties()
	{
		/// Act
		Deal result = response.ToDeal();

		/// Assert
		Assert.Equal(DEALSTAGE, result.DealStatus);
		Assert.Equal(DateTime.Today, result.EndDate);
		Assert.Equal(ID, result.HubSpotId);
		Assert.Equal(AMOUNTDECIMAL, result.Revenue);
		Assert.Equal(OWNERID, result.HubSpotOwnerId);
	}

	[Fact]
	public void ToDeal_WithoutAssociationsNoAmount_ReturnCorrectProperties()
	{
		/// Arragne 
		response.Properties.Amount = string.Empty;

		/// Act
		Deal result = response.ToDeal();

		/// Assert
		Assert.Equal(DEALSTAGE, result.DealStatus);
		Assert.Equal(DateTime.Today, result.EndDate);
		Assert.Equal(ID, result.HubSpotId);
		Assert.Null(result.Revenue);
		Assert.Equal(OWNERID, result.HubSpotOwnerId);
	}

	[Fact]
	public void ToDeal_WithoutAssociations_ReturnDealWithEmptyDealContacts()
	{
		/// Act
		Deal result = response.ToDeal();

		/// Assert
		Assert.Equal(0, result.DealContacts!.Count);
	}

	[Fact]
	public void ToDeal_WithoutAssociations_ReturnDealWithEmptyClient()
	{
		/// Act
		Deal result = response.ToDeal();

		/// Assert
		Assert.Null(result.Client!);
	}

	[Fact]
	public void ToDeal_WithDefaultAssociations_ReturnEmptyDealContacts()
	{
		/// arrange
		response.Associations.Contacts.AssociationList = new List<Association>()
		{
			new(),
		};

		/// act
		Deal result = response.ToDeal();

		/// assert
		Assert.Equal(0, result.DealContacts!.Count);
	}

	[Fact]
	public void ToDeal_WithDefaultAssociations_ReturnEmptyClient()
	{
		/// arrange
		response.Associations.Companies.AssociationList = new List<Association>()
		{
			new(),
		};

		/// act
		Deal result = response.ToDeal();

		/// assert
		Assert.Null(result.Client);
	}

	[Fact]
	public void ToDeal_WithnotMatchingAssociations_ReturnEmptyDealContacts()
	{
		/// arrange
		response.Associations.Contacts.AssociationList = new List<Association>()
		{
			new()
			{
				Type= "not_matching",
				Id = "2"
			},
		};

		/// act
		Deal result = response.ToDeal();

		/// assert
		Assert.Equal(0, result.DealContacts!.Count);
	}

	[Fact]
	public void ToDeal_WithnotMatchingAssociations_ReturnEmptyClient()
	{
		/// arrange
		response.Associations.Companies.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "not_matching",
				Id = "2"
			},
		};

		/// act
		Deal result = response.ToDeal();

		/// assert
		Assert.Null(result.Client);
	}

	[Fact]
	public void ToDeal_WithMatchingAssociations_ReturnWithDealContacts()
	{
		/// arrange
		response.Associations.Contacts.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "deal_to_contact",
				Id = "1"
			},
		};

		/// act
		Deal result = response.ToDeal();

		/// assert
		Assert.Equal(1, result.DealContacts!.Count);
		Assert.Equal("1", result.DealContacts.First().HubSpotContactId);
		Assert.Equal(result, result.DealContacts.First().Deal);
		Assert.Equal(result.Id, result.DealContacts.First().DealId);
		Assert.True(result.DealContacts.First().IsActive);
	}

	[Fact]
	public void ToDeal_WithMatchingAssociations_ReturnWithClient()
	{
		/// arrange
		response.Associations.Companies.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "deal_to_company",
				Id = "1"
			},
		};

		/// act
		Deal result = response.ToDeal();

		/// assert
		Assert.Equal("1", result.Client!.HubSpotId);
		Assert.Equal(result, result.Client!.Deals!.First());
	}
}
