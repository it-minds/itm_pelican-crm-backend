using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Deals.Commands.DeleteDeal;
using Pelican.Application.Deals.Commands.UpdateDeal;
using Pelican.Application.HubSpot.Commands.NewInstallation;
using Pelican.Domain.Shared;
using Pelican.Presentation.Api.Contracts;
using Pelican.Presentation.Api.Controllers;
using Xunit;

namespace Pelican.Presentation.Api.Test.Controllers;
public class HubSpotControllerTests
{
	private const long OBJECT_ID = 0;
	private const long PORTAL_ID = 0;
	private const string PROPERTY_NAME = "propertyName";
	private const string PROPERTY_VALUE = "newValue";

	private readonly HubSpotController _uut;
	private readonly Mock<ISender> _senderMock;

	public HubSpotControllerTests()
	{
		_senderMock = new();
		_uut = new(_senderMock.Object);
	}

	[Theory]
	[InlineData("code")]
	public async void NewInstallation_SenderReturnsSuccess_ReturnsOk(string code)
	{
		// Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<NewInstallationCommand>(), default))
			.ReturnsAsync(Result.Success);

		// Act
		IActionResult result = await _uut.NewInstallation(code, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				It.Is<NewInstallationCommand>(n => n.Code == code),
				default),
			Times.Once);

		Assert.IsType<RedirectResult>(result);
	}

	[Theory]
	[InlineData("code", "0", "fail")]
	public async void NewInstallation_SenderReturnsFailure_ReturnsBadRequest(
		string code,
		string errorCode,
		string errorMessage)
	{
		// Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<NewInstallationCommand>(), default))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		// Act
		IActionResult result = await _uut.NewInstallation(code, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				It.Is<NewInstallationCommand>(n => n.Code == code),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingEmptyBody_NoCommandsSendReturnsOk()
	{
		// Arrange
		List<WebHookRequest> requests = new();

		// Act
		IActionResult result = await _uut.Hook(requests, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(It.IsAny<ICommand>(), default),
			Times.Never);

		Assert.IsType<OkResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneUnhandledRequest_NoCommandsSendReturnsOk()
	{
		// Arrange
		List<WebHookRequest> requests = new()
		{
			new()
			{
				SubscriptionType = "HelloWorld"
			}
		};

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), default))
			.ReturnsAsync(Result.Success);

		// Act
		IActionResult result = await _uut.Hook(requests, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(It.IsAny<ICommand>(), default),
			Times.Never);

		Assert.IsType<OkResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneDealDeletionRequestResultSuccess_OneCommandSendReturnsOk()
	{
		// Arrange
		List<WebHookRequest> requests = new()
		{
			new()
			{
				SubscriptionType = "deal.deletion",
				ObjectId = OBJECT_ID,
			}
		};

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), default))
			.ReturnsAsync(Result.Success);

		// Act
		IActionResult result = await _uut.Hook(requests, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				It.Is<DeleteDealCommand>(c => c.ObjectId == OBJECT_ID),
				default),
			Times.Once);

		Assert.IsType<OkResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneDealDeletionRequestResultFailure_OneCommandSendReturnsBadRequest()
	{
		// Arrange
		List<WebHookRequest> requests = new()
		{
			new()
			{
				SubscriptionType = "deal.deletion",
				ObjectId = OBJECT_ID,
			},
		};

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), default))
			.ReturnsAsync(Result.Failure(new Error("0", "fail")));

		// Act
		IActionResult result = await _uut.Hook(requests, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				It.Is<DeleteDealCommand>(c => c.ObjectId == OBJECT_ID),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneDealPropertyChangeRequestResultSuccess_OneCommandSendReturnsOk()
	{
		// Arrange
		List<WebHookRequest> requests = new()
		{
			new()
			{
				SubscriptionType = "deal.propertyChange",
				ObjectId=OBJECT_ID,
				PortalId=PORTAL_ID,
				PropertyName=PROPERTY_NAME,
				PropertyValue=PROPERTY_VALUE,
			}
		};

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), default))
			.ReturnsAsync(Result.Success);

		// Act
		IActionResult result = await _uut.Hook(requests, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				It.Is<UpdateDealCommand>(c =>
					c.ObjectId == OBJECT_ID
					&& c.SupplierHubSpotId == PORTAL_ID
					&& c.PropertyName == PROPERTY_NAME
					&& c.PropertyValue == PROPERTY_VALUE),
				default),
			Times.Once);

		Assert.IsType<OkResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneDealPropertyChangeRequestWithEmptyNameAndValueResultFailure_OneCommandSendReturnsBadRequest()
	{
		// Arrange
		List<WebHookRequest> requests = new()
		{
			new()
			{
				SubscriptionType = "deal.propertyChange",
				ObjectId=OBJECT_ID,
				PortalId=PORTAL_ID
			}
		};

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), default))
			.ReturnsAsync(Result.Failure(new Error("0", "fail")));

		// Act
		IActionResult result = await _uut.Hook(requests, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				It.Is<UpdateDealCommand>(c =>
					c.ObjectId == OBJECT_ID
					&& c.SupplierHubSpotId == PORTAL_ID
					&& c.PropertyName == string.Empty
					&& c.PropertyValue == string.Empty),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneDealPropertyChangeRequestResultFailure_OneCommandSendReturnsBadRequest()
	{
		// Arrange
		List<WebHookRequest> requests = new()
		{
			new()
			{
				SubscriptionType = "deal.propertyChange",
				ObjectId=OBJECT_ID,
				PortalId=PORTAL_ID,
				PropertyName=PROPERTY_NAME,
				PropertyValue=PROPERTY_VALUE,
			}
		};

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), default))
			.ReturnsAsync(Result.Failure(new Error("0", "fail")));

		// Act
		IActionResult result = await _uut.Hook(requests, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				It.Is<UpdateDealCommand>(c =>
					c.ObjectId == OBJECT_ID
					&& c.SupplierHubSpotId == PORTAL_ID
					&& c.PropertyName == PROPERTY_NAME
					&& c.PropertyValue == PROPERTY_VALUE),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}
}
