using Pelican.Application.AccountManagers.Commands.ValidateWebhookUserId;
using Pelican.Application.Clients.Commands.DeleteClient;
using Pelican.Application.Clients.Commands.UpdateClient;
using Pelican.Application.Contacts.Commands.UpdateContact;
using Pelican.Application.Deals.Commands.DeleteDeal;
using Pelican.Application.Deals.Commands.UpdateDeal;
using Pelican.Presentation.Api.Contracts;
using Pelican.Presentation.Api.Mapping;
using Xunit;

namespace Pelican.Presentation.Api.Test.Mapping;

public class HubSpotWebHookRequestsToCommandsTests
{
	private const long OBJECT_ID = 123;
	private const long SUPPLIER_HUBSPOT_ID = 456;
	private const string PROPERTY_NAME = "name";
	private const string PROPERTY_VALUE = "value";

	private readonly HubSpotWebHookRequestsToCommands _uut = new();

	[Fact]
	public void ConvertToCommands_NullCollection_ReturnEmptyCollection()
	{
		// Act 
		var result = _uut.ConvertToCommands(null);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void ConvertToCommands_EmptyCollection_ReturnEmptyCollection()
	{
		// Arrange
		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>();

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void ConvertToCommands_SubscriptionTypeInvalid_ThrowsInvalidDataException()
	{
		// Arrange
		HubSpotWebHookRequest webHookRequest = new()
		{
			SubscriptionType = "invalid",
		};

		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>()
		{
			webHookRequest,
		};

		// Act 
		var result = Record.Exception(() => _uut.ConvertToCommands(webHookRequests));

		// Assert
		Assert.Equal(
			"Receiving unhandled event",
			result.Message);
	}

	[Fact]
	public void ConvertToCommands_SubscriptionTypeDealDeletion_ReturnDeleteDealCommand()
	{
		// Arrange
		HubSpotWebHookRequest webHookRequest = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
		};

		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>()
		{
			webHookRequest,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Equal(
			OBJECT_ID,
			((DeleteDealHubSpotCommand)result.First()).ObjectId);
	}

	[Fact]
	public void ConvertToCommands_SubscriptionTypeCompanyDeletion_ReturnDeleteClientCommand()
	{
		// Arrange
		HubSpotWebHookRequest webHookRequest = new()
		{
			SubscriptionType = "company.deletion",
			ObjectId = OBJECT_ID,
		};

		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>()
		{
			webHookRequest,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Equal(
			OBJECT_ID,
			((DeleteClientCommand)result.First()).ObjectId);
	}

	[Fact]
	public void ConvertToCommands_SubscriptionTypeContactPropertyChange_ReturnUpdateContactCommand()
	{
		// Arrange
		HubSpotWebHookRequest webHookRequest = new()
		{
			SubscriptionType = "contact.propertyChange",
			ObjectId = OBJECT_ID,
			SupplierHubSpotId = SUPPLIER_HUBSPOT_ID,
			PropertyName = PROPERTY_NAME,
			PropertyValue = PROPERTY_VALUE,
		};

		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>()
		{
			webHookRequest,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Equal(
			OBJECT_ID,
			((UpdateContactCommand)result.First()).ObjectId);
		Assert.Equal(
			SUPPLIER_HUBSPOT_ID,
			((UpdateContactCommand)result.First()).SupplierHubSpotId);
		Assert.Equal(
			PROPERTY_NAME,
			((UpdateContactCommand)result.First()).PropertyName);
		Assert.Equal(
			PROPERTY_VALUE,
			((UpdateContactCommand)result.First()).PropertyValue);
	}

	[Fact]
	public void ConvertToCommands_SubscriptionTypeDealPropertyChange_ReturnUpdateDealCommand()
	{
		// Arrange
		HubSpotWebHookRequest webHookRequest = new()
		{
			SubscriptionType = "deal.propertyChange",
			ObjectId = OBJECT_ID,
			SupplierHubSpotId = SUPPLIER_HUBSPOT_ID,
			PropertyName = PROPERTY_NAME,
			PropertyValue = PROPERTY_VALUE,
		};

		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>()
		{
			webHookRequest,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Equal(
			OBJECT_ID,
			((UpdateDealHubSpotCommand)result.First()).ObjectId);
		Assert.Equal(
			SUPPLIER_HUBSPOT_ID,
			((UpdateDealHubSpotCommand)result.First()).SupplierHubSpotId);
		Assert.Equal(
			PROPERTY_NAME,
			((UpdateDealHubSpotCommand)result.First()).PropertyName);
		Assert.Equal(
			PROPERTY_VALUE,
			((UpdateDealHubSpotCommand)result.First()).PropertyValue);
	}

	[Fact]
	public void ConvertToCommands_SubscriptionTypeCompanyPropertyChange_ReturnUpdateClientCommand()
	{
		// Arrange
		HubSpotWebHookRequest webHookRequest = new()
		{
			SubscriptionType = "company.propertyChange",
			ObjectId = OBJECT_ID,
			SupplierHubSpotId = SUPPLIER_HUBSPOT_ID,
			PropertyName = PROPERTY_NAME,
			PropertyValue = PROPERTY_VALUE,
		};

		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>()
		{
			webHookRequest,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Equal(
			OBJECT_ID,
			((UpdateClientCommand)result.First()).ObjectId);
		Assert.Equal(
			SUPPLIER_HUBSPOT_ID,
			((UpdateClientCommand)result.First()).PortalId);
		Assert.Equal(
			PROPERTY_NAME,
			((UpdateClientCommand)result.First()).PropertyName);
		Assert.Equal(
			PROPERTY_VALUE,
			((UpdateClientCommand)result.First()).PropertyValue);
	}

	[Fact]
	public void ConvertToCommands_MultipleValidRequests_ReturnMultipleCommands()
	{
		// Arrange
		HubSpotWebHookRequest webHookRequest = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
		};

		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>()
		{
			webHookRequest,
			webHookRequest,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Equal(
			2,
			result.Count);
	}

	[Fact]
	public void ConvertToCommands_MultipleValidRequestsWithoutSourceIds_ReturnNoValidateWebhookUserIdCommand()
	{
		// Arrange
		HubSpotWebHookRequest webHookRequest = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
		};

		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>()
		{
			webHookRequest,
			webHookRequest,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Collection(
			result,
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r),
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r));
	}

	[Fact]
	public void ConvertToCommands_MultipleValidRequestsWithoutUserIdsInSourceIds_ReturnNoValidateWebhookUserIdCommand()
	{
		// Arrange
		HubSpotWebHookRequest webHookRequest = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SourceId = "notUserId",
		};

		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>()
		{
			webHookRequest,
			webHookRequest,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Collection(
			result,
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r),
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r));
	}

	[Fact]
	public void ConvertToCommands_MultipleValidRequestsWithInvallidUserIdInSourceIds_ReturnNoValidateWebhookUserIdCommand()
	{
		// Arrange
		HubSpotWebHookRequest webHookRequest = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SourceId = "userId:invalid",
		};

		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>()
		{
			webHookRequest,
			webHookRequest,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Collection(
			result,
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r),
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r));
	}

	[Fact]
	public void ConvertToCommands_MultipleValidRequestsWithOneUserId_ReturnOneValidateWebhookUserIdCommand()
	{
		// Arrange
		HubSpotWebHookRequest webHookRequest1 = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SourceId = "notUserId",
			SupplierHubSpotId = 123,
		};
		HubSpotWebHookRequest webHookRequest2 = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SourceId = "userId:47115417",
			SupplierHubSpotId = 456,
		};

		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>()
		{
			webHookRequest1,
			webHookRequest2,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Collection(
			result,
			r =>
			{
				Assert.IsType<ValidateWebhookUserIdCommand>(r);
				Assert.Equal(47115417, (r as ValidateWebhookUserIdCommand)!.UserId);
				Assert.Equal(456, (r as ValidateWebhookUserIdCommand)!.SupplierHubSpotId);
			},
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r),
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r));
	}

	[Fact]
	public void ConvertToCommands_MultipleValidRequestsWithSameUserId_ReturnOneValidateWebhookUserIdCommand()
	{
		// Arrange
		HubSpotWebHookRequest webHookRequest1 = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SourceId = "userId:47115417",
			SupplierHubSpotId = 123,
		};
		HubSpotWebHookRequest webHookRequest2 = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SourceId = "userId:47115417",
			SupplierHubSpotId = 123
		};

		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>()
		{
			webHookRequest1,
			webHookRequest2,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Collection(
			result,
			r =>
			{
				Assert.IsType<ValidateWebhookUserIdCommand>(r);
				Assert.Equal(47115417, (r as ValidateWebhookUserIdCommand)!.UserId);
				Assert.Equal(123, (r as ValidateWebhookUserIdCommand)!.SupplierHubSpotId);
			},
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r),
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r));
	}

	[Fact]
	public void ConvertToCommands_MultipleValidRequestsWithDifferentUserId_ReturnTwoValidateWebhookUserIdCommand()
	{
		// Arrange
		HubSpotWebHookRequest webHookRequest1 = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SupplierHubSpotId = 123,
			SourceId = "userId:123456",
		};
		HubSpotWebHookRequest webHookRequest2 = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SupplierHubSpotId = 456,
			SourceId = "userId:47115417",
		};

		IReadOnlyCollection<HubSpotWebHookRequest> webHookRequests = new List<HubSpotWebHookRequest>()
		{
			webHookRequest1,
			webHookRequest2,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Collection(
			result,
			r =>
			{
				Assert.IsType<ValidateWebhookUserIdCommand>(r);
				Assert.Equal(123456, (r as ValidateWebhookUserIdCommand)!.UserId);
				Assert.Equal(123, (r as ValidateWebhookUserIdCommand)!.SupplierHubSpotId);
			},
			r =>
			{
				Assert.IsType<ValidateWebhookUserIdCommand>(r);
				Assert.Equal(47115417, (r as ValidateWebhookUserIdCommand)!.UserId);
				Assert.Equal(456, (r as ValidateWebhookUserIdCommand)!.SupplierHubSpotId);
			},
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r),
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r));
	}
}
