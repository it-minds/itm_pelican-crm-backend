using System.Linq.Expressions;
using Bogus;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
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
	public async Task ToDeal_ResponseMissingHubSpotId_ThrowsException()
	{
		// Arrange
		DealResponse defaultResponse = new();
		defaultResponse.Properties.HubSpotOwnerId = OWNERID;

		// Act
		Exception result = await Record.ExceptionAsync(() => defaultResponse.ToDeal(_unitOfWorkMock.Object, default));

		// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public async Task ToDeal_WithoutAssociations_ReturnCorrectPropertiesAndAssociations()
	{
		// Arrange
		_unitOfWorkMock
			.Setup(u => u
				.AccountManagerRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<AccountManager, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((AccountManager)null!);

		// Act
		Deal result = await response.ToDeal(_unitOfWorkMock.Object, default);

		// Assert
		Assert.Equal(DEALSTAGE, result.Status);
		Assert.Equal(new DateTimeOffset(Convert.ToDateTime(CLOSEDATE)).ToUnixTimeMilliseconds(), result.EndDate);
		Assert.Equal(new DateTimeOffset(Convert.ToDateTime(STARTDATE)).ToUnixTimeMilliseconds(), result.StartDate);
		Assert.Equal(new DateTimeOffset(Convert.ToDateTime(LASTCONTACTDATE)).ToUnixTimeMilliseconds(), result.LastContactDate);
		Assert.Equal(ID, result.SourceId);
		Assert.Equal(OWNERID, result.SourceOwnerId);
		Assert.Equal(DEALNAME, result.Name);
		Assert.Equal(DEALDESCRIPTION, result.Description);
		Assert.Equal(Sources.HubSpot, result.Source);

		Assert.Equal(0, result.DealContacts!.Count);
		Assert.Null(result.Client!);
	}

	[Fact]
	public async Task ToDeal_WithDefaultAssociations_ReturnEmptyDealContacts()
	{
		// arrange
		response.Associations.Contacts.AssociationList = new List<Association>()
		{
			new(),
		};

		_unitOfWorkMock
			.Setup(u => u
				.AccountManagerRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<AccountManager, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((AccountManager)null!);

		// act
		Deal result = await response.ToDeal(_unitOfWorkMock.Object, default);

		// assert
		Assert.Equal(0, result.DealContacts!.Count);
	}

	[Fact]
	public async Task ToDeal_WithDefaultAssociations_ReturnEmptyClient()
	{
		// arrange
		response.Associations.Companies.AssociationList = new List<Association>()
		{
			new(),
		};

		_unitOfWorkMock
			.Setup(u => u
				.AccountManagerRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<AccountManager, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((AccountManager)null!);

		// act
		Deal result = await response.ToDeal(_unitOfWorkMock.Object, default);

		// assert
		Assert.Null(result.Client);
	}

	[Fact]
	public async Task ToDeal_WithnotMatchingAssociations_ReturnEmptyDealContacts()
	{
		// arrange
		response.Associations.Contacts.AssociationList = new List<Association>()
		{
			new()
			{
				Type= "not_matching",
				Id = "2"
			},
		};

		_unitOfWorkMock
			.Setup(u => u
				.AccountManagerRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<AccountManager, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((AccountManager)null!);

		// act
		Deal result = await response.ToDeal(_unitOfWorkMock.Object, default);

		// assert
		Assert.Equal(0, result.DealContacts!.Count);
	}

	[Fact]
	public async Task ToDeal_WithnotMatchingAssociations_ReturnEmptyClient()
	{
		// arrange
		response.Associations.Companies.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "not_matching",
				Id = "2"
			},
		};

		_unitOfWorkMock
			.Setup(u => u
				.AccountManagerRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<AccountManager, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((AccountManager)null!);

		// act
		Deal result = await response.ToDeal(_unitOfWorkMock.Object, default);

		// assert
		Assert.Null(result.Client);
	}

	[Fact]
	public async Task ToDeal_WithMatchingAssociations_ReturnWithDealContacts()
	{
		// arrange
		response.Associations.Contacts.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "deal_to_contact",
				Id = "1"
			},
		};

		_unitOfWorkMock
			.Setup(u => u
				.AccountManagerRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<AccountManager, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((AccountManager)null!);

		_unitOfWorkMock
			.Setup(u => u
				.ContactRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Contact, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Contact() { SourceId = "1" });

		// act
		Deal result = await response.ToDeal(_unitOfWorkMock.Object, default);

		// assert
		Assert.Equal(1, result.DealContacts!.Count);
		Assert.Equal("1", result.DealContacts.First().SourceContactId);
		Assert.Equal(result, result.DealContacts.First().Deal);
		Assert.Equal(result.Id, result.DealContacts.First().DealId);
		Assert.True(result.DealContacts.First().IsActive);
	}

	[Fact]
	public async Task ToDeal_WithMatchingAssociations_ReturnWithClient()
	{
		// arrange
		response.Associations.Companies.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "deal_to_company",
				Id = "1"
			},
		};

		_unitOfWorkMock
			.Setup(u => u
				.AccountManagerRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<AccountManager, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((AccountManager)null!);

		_unitOfWorkMock
			.Setup(u => u
				.ClientRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Client, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Client() { SourceId = "1" });

		// act
		Deal result = await response.ToDeal(_unitOfWorkMock.Object, default);

		// assert
		Assert.Equal("1", result.Client!.SourceId);
		Assert.Equal(result, result.Client!.Deals!.First());
	}
}
