using System.Linq.Expressions;
using Bogus;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
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
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly CancellationToken cancellationToken = new();

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
		var result = Record.ExceptionAsync(() => defaultResponse.ToClient(_unitOfWorkMock.Object, cancellationToken));

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.Result.GetType());
	}

	[Fact]
	public void ToClient_ResponseMissingName_ThrowsException()
	{
		/// Arrange
		CompanyResponse defaultResponse = new();
		defaultResponse.Properties.HubSpotObjectId = ID;

		/// Act
		var result = Record.ExceptionAsync(() => defaultResponse.ToClient(_unitOfWorkMock.Object, cancellationToken));

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.Result.GetType());
	}

	[Fact]
	public async void ToClient_WithoutAssociations_ReturnCorrectProperties()
	{
		/// Act
		var result = await response.ToClient(_unitOfWorkMock.Object, cancellationToken);

		/// Assert
		Assert.Equal(NAME, result.Name);
		Assert.Equal(LOCATION, result.OfficeLocation);
		Assert.Equal(ID, result.SourceId);
		Assert.Equal(Sources.HubSpot, result.Source);
	}

	[Fact]
	public async void ToClient_NameStringTooLong_NameShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.Name = faker.Lorem.Letter(StringLengths.Name * 2);

		/// Act
		Client result = await response.ToClient(_unitOfWorkMock.Object, cancellationToken);

		/// Assert
		Assert.Equal(StringLengths.Name, result.Name.Length);
		Assert.Equal("...", result.Name.Substring(StringLengths.Name - 3));
		Assert.Equal(response.Properties.Name.Substring(0, StringLengths.Name - 3), result.Name.Substring(0, StringLengths.Name - 3));
	}

	[Fact]
	public async void ToClient_OfficeLocationStringTooLong_OfficeLocationShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.City = faker.Lorem.Letter(StringLengths.OfficeLocation * 2);

		/// Act
		Client result = await response.ToClient(_unitOfWorkMock.Object, cancellationToken);

		/// Assert
		Assert.Equal(StringLengths.OfficeLocation, result.OfficeLocation!.Length);
		Assert.Equal("...", result.OfficeLocation.Substring(StringLengths.OfficeLocation - 3));
		Assert.Equal(response.Properties.City.Substring(0, StringLengths.OfficeLocation - 3), result.OfficeLocation.Substring(0, StringLengths.OfficeLocation - 3));

	}

	[Fact]
	public async void ToClient_DomainStringTooLong_DomainShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Properties.Domain = faker.Lorem.Letter(StringLengths.Url * 2);

		/// Act
		Client result = await response.ToClient(_unitOfWorkMock.Object, cancellationToken);

		/// Assert
		Assert.Equal(StringLengths.Url, result.Website!.Length);
		Assert.Equal("...", result.Website.Substring(StringLengths.Url - 3));
		Assert.Equal(response.Properties.Domain.Substring(0, StringLengths.Url - 3), result.Website.Substring(0, StringLengths.Url - 3));
	}
	[Fact]
	public async void ToClient_WithoutAssociations_ReturnClientWithEmptyDeals()
	{
		/// Act
		Client result = await response.ToClient(_unitOfWorkMock.Object, cancellationToken);

		/// Assert
		Assert.Equal(0, result.Deals!.Count);
	}

	[Fact]
	public async void ToClient_WithoutAssociations_ReturnClientWithEmptyClientContacts()
	{
		/// Act
		Client result = await response.ToClient(_unitOfWorkMock.Object, cancellationToken);

		/// Assert
		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public async void ToClient_WithDefaultDealsAndContacts_ReturnClientEmptyDeals()
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
		Client result = await response.ToClient(_unitOfWorkMock.Object, cancellationToken);

		/// Assert
		Assert.Equal(0, result.Deals!.Count);
	}

	[Fact]
	public async void ToClient_WithDefaultDealsAndContacts_ReturnClientEmptyClientContacts()
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
		Client result = await response.ToClient(_unitOfWorkMock.Object, cancellationToken);

		/// Assert
		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public async void ToClient_WithNotMatchingAssociations_ReturnClientEmptyDeals()
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
		Client result = await response.ToClient(_unitOfWorkMock.Object, cancellationToken);

		/// Assert
		Assert.Equal(0, result.Deals!.Count);
	}

	[Fact]
	public async void ToClient_WithNotMatchingAssociations_ReturnClientEmptyClientContacts()
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
		Client result = await response.ToClient(_unitOfWorkMock.Object, cancellationToken);

		/// Assert
		Assert.Equal(0, result.ClientContacts!.Count);
	}

	[Fact]
	public async void ToClient_WithMatchingAssociations_ReturnClientWithDeals()
	{
		/// Arrange
		Mock<Deal> dealMock = new();

		response.Associations.Deals.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "company_to_deal_unlabeled",
				Id = "1"
			},
		};
		dealMock.Object.SourceId = "1";
		dealMock.Object.Source = Sources.HubSpot;

		_unitOfWorkMock.Setup(
			u => u.DealRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Deal, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(dealMock.Object);

		/// Act
		Client result = await response.ToClient(_unitOfWorkMock.Object, cancellationToken);

		/// Assert
		Assert.Equal(1, result.Deals!.Count);
		Assert.Equal("1", result.Deals.First().SourceId);
		Assert.Equal(result, result.Deals.First().Client);
		Assert.Equal(result.Id, result.Deals.First().ClientId);
	}

	[Fact]
	public async void ToClient_WithMatchingAssociations_ReturnClientWithClientContacts()
	{
		/// Arrange
		Mock<Contact> contactMock = new();

		response.Associations.Contacts.AssociationList = new List<Association>()
		{
			new()
			{
				Type = "company_to_contact_unlabeled",
				Id = "1",
			},
		};
		contactMock.Object.SourceId = "1";
		contactMock.Object.Source = Sources.HubSpot;

		_unitOfWorkMock.Setup(
			u => u.ContactRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Contact, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync(contactMock.Object);

		/// Act
		Client result = await response.ToClient(_unitOfWorkMock.Object, cancellationToken);

		/// Assert
		Assert.Equal(1, result.ClientContacts!.Count);
		Assert.Equal("1", result.ClientContacts.First().SourceContactId);
		Assert.Equal(result, result.ClientContacts.First().Client);
		Assert.Equal(result.Id, result.ClientContacts.First().ClientId);
		Assert.True(result.ClientContacts.First().IsActive);
	}
}
