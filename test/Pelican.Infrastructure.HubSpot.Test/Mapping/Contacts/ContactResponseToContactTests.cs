using System.Linq.Expressions;
using Bogus;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
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

	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

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
	public async Task ToContact_ResponseMissingHubSpotId_ThrowsException()
	{
		/// Arrange
		ContactResponse defaultResponse = new();
		defaultResponse.Properties.HubSpotOwnerId = OWNERID;

		/// Act
		Exception result = await Record.ExceptionAsync(() => defaultResponse.ToContact(_unitOfWorkMock.Object, default));

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public async Task ToContact_WithoutAssociations_ReturnCorrectPropertiesAndAssociations()
	{
		/// Act
		Contact result = await response.ToContact(_unitOfWorkMock.Object, default);

		/// Assert
		Assert.Equal(FIRSTNAME, result.FirstName);
		Assert.Equal(LASTNAME, result.LastName);
		Assert.Equal(EMAIL, result.Email);
		Assert.Equal(PHONE, result.PhoneNumber);
		Assert.Equal(ID, result.SourceId);
		Assert.Equal(JOBTITLE, result.JobTitle);
		Assert.Equal(OWNERID, result.SourceOwnerId);
		Assert.Equal(Sources.HubSpot, result.Source);

		Assert.Equal(0, result.DealContacts!.Count);
		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public async Task ToContact_WithDefaultDeals_ReturnContactEmptyDealContactsAndEmptyClientContacts()
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
		Contact result = await response.ToContact(_unitOfWorkMock.Object, default);

		/// Assert
		Assert.Equal(0, result.DealContacts!.Count);
		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public async Task ToContact_WithNotMatchingAssociations_ReturnContactEmptyDealContacts()
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
		Contact result = await response.ToContact(_unitOfWorkMock.Object, default);

		/// Assert
		Assert.Equal(0, result.DealContacts!.Count);
	}

	[Fact]
	public async Task ToContact_WithNotMatchingAssociations_ReturnContactEmptyClientContacts()
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
		Contact result = await response.ToContact(_unitOfWorkMock.Object, default);

		/// Assert
		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public async Task ToContact_WithMatchingAssociations_ReturnContactWithDeals()
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

		_unitOfWorkMock
			.Setup(u => u
				.DealRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Deal, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Deal() { SourceId = "1" });

		/// Act
		Contact result = await response.ToContact(_unitOfWorkMock.Object, default);

		/// Assert
		Assert.Equal(1, result.DealContacts!.Count);
		Assert.Equal("1", result.DealContacts.First().SourceDealId);
		Assert.Equal(result, result.DealContacts.First().Contact);
		Assert.Equal(result.Id, result.DealContacts.First().ContactId);
	}

	[Fact]
	public async Task ToContact_WithMatchingAssociations_ReturnContactWithClientContacts()
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

		_unitOfWorkMock
			.Setup(u => u
				.ClientRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Client, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Client() { SourceId = "1" });

		/// Act
		Contact result = await response.ToContact(_unitOfWorkMock.Object, default);

		/// Assert
		Assert.Equal(1, result.ClientContacts!.Count);
		Assert.Equal("1", result.ClientContacts.First().SourceClientId);
		Assert.Equal(result, result.ClientContacts.First().Contact);
		Assert.Equal(result.Id, result.ClientContacts.First().ContactId);
		Assert.True(result.ClientContacts.First().IsActive);
	}
}
