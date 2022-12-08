using Bogus;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain;
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
	private const string OWNERID = "ownerid";
	private const string CLOSEDATE = "1999-04-25T00:00:00.000Z";
	private const string STARTDATE = "1999-04-25T00:00:00.000Z";
	private const string LASTCONTACTDATE = "1999-04-25T00:00:00.000Z";
	private const string DEALNAME = "dealname";
	private const string DEALDESCRIPTION = "dealdescription";
	private const string DEALSOURCE = "HubSpot";

	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

	private readonly DealResponse response = new()
	{
		Properties = new()
		{
			DealStage = DEALSTAGE,
			EndDate = CLOSEDATE,
			StartDate = STARTDATE,
			HubSpotObjectId = ID,
			HubSpotOwnerId = OWNERID,
			LastContactDate = LASTCONTACTDATE,
			DealName = DEALNAME,
			Description = DEALDESCRIPTION,
		},
	};

	[Fact]
	public void ToDeal_ResponseMissingHubSpotId_ThrowsException()
	{
		/// Arrange
		DealResponse defaultResponse = new();
		defaultResponse.Properties.HubSpotOwnerId = OWNERID;

		/// Act
		Exception result = Record.Exception(() => defaultResponse.ToDeal(_unitOfWorkMock.Object));

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public void ToDeal_WithoutAssociations_ReturnCorrectProperties()
	{
		/// Act
		Deal result = response.ToDeal(_unitOfWorkMock.Object);

		/// Assert
		Assert.Equal(DEALSTAGE, result.Status);
		Assert.Equal(Convert.ToDateTime(CLOSEDATE).Ticks, result.EndDate);
		Assert.Equal(Convert.ToDateTime(STARTDATE).Ticks, result.StartDate);
		Assert.Equal(Convert.ToDateTime(LASTCONTACTDATE).Ticks, result.LastContactDate);
		Assert.Equal(ID, result.SourceId);
		Assert.Equal(OWNERID, result.SourceOwnerId);
		Assert.Equal(DEALNAME, result.Name);
		Assert.Equal(DEALDESCRIPTION, result.Description);
		Assert.Equal(Sources.HubSpot, result.Source);
	}

	[Fact]
	public void ToDeal_WithoutAssociations_ReturnDealWithEmptyDealContacts()
	{
		/// Act
		Deal result = response.ToDeal(_unitOfWorkMock.Object);

		/// Assert
		Assert.Equal(0, result.DealContacts!.Count);
	}

	[Fact]
	public void ToDeal_WithoutAssociations_ReturnDealWithEmptyClient()
	{
		/// Act
		Deal result = response.ToDeal(_unitOfWorkMock.Object);

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
		Deal result = response.ToDeal(_unitOfWorkMock.Object);

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
		Deal result = response.ToDeal(_unitOfWorkMock.Object);

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
		Deal result = response.ToDeal(_unitOfWorkMock.Object);

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
		Deal result = response.ToDeal(_unitOfWorkMock.Object);

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
		Deal result = response.ToDeal(_unitOfWorkMock.Object);

		/// assert
		Assert.Equal(1, result.DealContacts!.Count);
		Assert.Equal("1", result.DealContacts.First().SourceContactId);
		Assert.Equal(result, result.DealContacts.First().Deal);
		Assert.Equal(result.Id, result.DealContacts.First().DealId);
		Assert.True(result.DealContacts.First().IsActive);
	}

	[Fact]
	public void ToDeal_NameStringTooLong_NameShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.DealName = faker.Lorem.Letter(StringLengths.DealName * 2);

		/// Act
		Deal result = response.ToDeal(_unitOfWorkMock.Object);

		/// Assert
		Assert.Equal(StringLengths.DealName, result.Name!.Length);
		Assert.Equal("...", result.Name.Substring(StringLengths.DealName - 3));
		Assert.Equal(response.Properties.DealName.Substring(0, StringLengths.DealName - 3), result.Name.Substring(0, StringLengths.DealName - 3));
	}

	[Fact]
	public void ToDeal_DealStatusStringTooLong_DealStatusShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.DealStage = faker.Lorem.Letter(StringLengths.DealStatus * 2);

		/// Act
		Deal result = response.ToDeal(_unitOfWorkMock.Object);

		/// Assert
		Assert.Equal(StringLengths.DealStatus, result.Status!.Length);
		Assert.Equal("...", result.Status.Substring(StringLengths.DealStatus - 3));
		Assert.Equal(response.Properties.DealStage.Substring(0, StringLengths.DealStatus - 3), result.Status.Substring(0, StringLengths.DealStatus - 3));
	}

	[Fact]
	public void ToDeal_DescriptionStringTooLong_DescriptionShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.Description = faker.Lorem.Letter(StringLengths.DealDescription * 2);

		/// Act
		Deal result = response.ToDeal(_unitOfWorkMock.Object);

		/// Assert
		Assert.Equal(StringLengths.DealDescription, result.Description!.Length);
		Assert.Equal("...", result.Description.Substring(StringLengths.DealDescription - 3));
		Assert.Equal(response.Properties.Description.Substring(0, StringLengths.DealDescription - 3), result.Description.Substring(0, StringLengths.DealDescription - 3));
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
		Deal result = response.ToDeal(_unitOfWorkMock.Object);

		/// assert
		Assert.Equal("1", result.Client!.SourceId);
		Assert.Equal(result, result.Client!.Deals!.First());
	}
}
