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

public class WebHookRequestsToCommandsTests
{
	private const long OBJECT_ID = 123;
	private const long SUPPLIER_HUBSPOT_ID = 456;
	private const string PROPERTY_NAME = "name";
	private const string PROPERTY_VALUE = "value";

	private readonly WebHookRequestsToCommands _uut = new();

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
		IReadOnlyCollection<WebHookRequest> webHookRequests = new List<WebHookRequest>();

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Empty(result);
	}

	[Fact]
	public void ConvertToCommands_SubscriptionTypeInvalid_ThrowsInvalidDataException()
	{
		// Arrange
		WebHookRequest webHookRequest = new()
		{
			SubscriptionType = "invalid",
		};

		IReadOnlyCollection<WebHookRequest> webHookRequests = new List<WebHookRequest>()
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
		WebHookRequest webHookRequest = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
		};

		IReadOnlyCollection<WebHookRequest> webHookRequests = new List<WebHookRequest>()
		{
			webHookRequest,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Equal(
			OBJECT_ID,
			((DeleteDealCommand)result.First()).ObjectId);
	}

	[Fact]
	public void ConvertToCommands_SubscriptionTypeCompanyDeletion_ReturnDeleteClientCommand()
	{
		// Arrange
		WebHookRequest webHookRequest = new()
		{
			SubscriptionType = "company.deletion",
			ObjectId = OBJECT_ID,
		};

		IReadOnlyCollection<WebHookRequest> webHookRequests = new List<WebHookRequest>()
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
		WebHookRequest webHookRequest = new()
		{
			SubscriptionType = "contact.propertyChange",
			ObjectId = OBJECT_ID,
			SupplierHubSpotId = SUPPLIER_HUBSPOT_ID,
			PropertyName = PROPERTY_NAME,
			PropertyValue = PROPERTY_VALUE,
		};

		IReadOnlyCollection<WebHookRequest> webHookRequests = new List<WebHookRequest>()
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
		WebHookRequest webHookRequest = new()
		{
			SubscriptionType = "deal.propertyChange",
			ObjectId = OBJECT_ID,
			SupplierHubSpotId = SUPPLIER_HUBSPOT_ID,
			PropertyName = PROPERTY_NAME,
			PropertyValue = PROPERTY_VALUE,
		};

		IReadOnlyCollection<WebHookRequest> webHookRequests = new List<WebHookRequest>()
		{
			webHookRequest,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Equal(
			OBJECT_ID,
			((UpdateDealCommand)result.First()).ObjectId);
		Assert.Equal(
			SUPPLIER_HUBSPOT_ID,
			((UpdateDealCommand)result.First()).SupplierHubSpotId);
		Assert.Equal(
			PROPERTY_NAME,
			((UpdateDealCommand)result.First()).PropertyName);
		Assert.Equal(
			PROPERTY_VALUE,
			((UpdateDealCommand)result.First()).PropertyValue);
	}

	[Fact]
	public void ConvertToCommands_SubscriptionTypeCompanyPropertyChange_ReturnUpdateClientCommand()
	{
		// Arrange
		WebHookRequest webHookRequest = new()
		{
			SubscriptionType = "company.propertyChange",
			ObjectId = OBJECT_ID,
			SupplierHubSpotId = SUPPLIER_HUBSPOT_ID,
			PropertyName = PROPERTY_NAME,
			PropertyValue = PROPERTY_VALUE,
		};

		IReadOnlyCollection<WebHookRequest> webHookRequests = new List<WebHookRequest>()
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
		WebHookRequest webHookRequest = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
		};

		IReadOnlyCollection<WebHookRequest> webHookRequests = new List<WebHookRequest>()
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
		WebHookRequest webHookRequest = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
		};

		IReadOnlyCollection<WebHookRequest> webHookRequests = new List<WebHookRequest>()
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
		WebHookRequest webHookRequest = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SourceId = "notUserId",
		};

		IReadOnlyCollection<WebHookRequest> webHookRequests = new List<WebHookRequest>()
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
	public void ConvertToCommands_MultipleValidRequestsWithOneUserId_ReturnNoValidateWebhookUserIdCommand()
	{
		// Arrange
		WebHookRequest webHookRequest1 = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SourceId = "notUserId",
		};
		WebHookRequest webHookRequest2 = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SourceId = "userId:47115417",
		};

		IReadOnlyCollection<WebHookRequest> webHookRequests = new List<WebHookRequest>()
		{
			webHookRequest1,
			webHookRequest2,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Collection(
			result,
			r => Assert.IsType<ValidateWebhookUserIdCommand>(r),
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r),
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r));
	}

	[Fact]
	public void ConvertToCommands_MultipleValidRequestsWithSameUserId_ReturnNoValidateWebhookUserIdCommand()
	{
		// Arrange
		WebHookRequest webHookRequest1 = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SourceId = "userId:47115417",
		};
		WebHookRequest webHookRequest2 = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SourceId = "userId:47115417",
		};

		IReadOnlyCollection<WebHookRequest> webHookRequests = new List<WebHookRequest>()
		{
			webHookRequest1,
			webHookRequest2,
		};

		// Act 
		var result = _uut.ConvertToCommands(webHookRequests);

		// Assert
		Assert.Collection(
			result,
			r => Assert.IsType<ValidateWebhookUserIdCommand>(r),
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r),
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r));
	}

	[Fact]
	public void ConvertToCommands_MultipleValidRequestsWithDifferentUserId_ReturnNoValidateWebhookUserIdCommand()
	{
		// Arrange
		WebHookRequest webHookRequest1 = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SupplierHubSpotId = 123,
			SourceId = "userId:123456",
		};
		WebHookRequest webHookRequest2 = new()
		{
			SubscriptionType = "deal.deletion",
			ObjectId = OBJECT_ID,
			SupplierHubSpotId = 456,
			SourceId = "userId:47115417",
		};

		IReadOnlyCollection<WebHookRequest> webHookRequests = new List<WebHookRequest>()
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
				Assert.Equal(123, (r as ValidateWebhookUserIdCommand)!.SupplierHubSpotId);
			},
			r => Assert.IsType<ValidateWebhookUserIdCommand>(r),
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r),
			r => Assert.IsNotType<ValidateWebhookUserIdCommand>(r));

		Assert.Equal(
			123456,
			(result.Take(1).First() as ValidateWebhookUserIdCommand)!.UserId);

		Assert.Equal(
			123,
			(result.Take(1).First() as ValidateWebhookUserIdCommand)!.SupplierHubSpotId);

		Assert.Equal(
			47115417,
			(result.Take(2).Last() as ValidateWebhookUserIdCommand)!.UserId);

		Assert.Equal(
			456,
			(result.Take(2).Last() as ValidateWebhookUserIdCommand)!.SupplierHubSpotId);
	}
}
