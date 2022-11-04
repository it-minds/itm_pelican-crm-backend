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
	private readonly HubSpotController _uut;
	private readonly Mock<ISender> _senderMock;
	private readonly CancellationToken _cancellationToken;

	public HubSpotControllerTests()
	{
		_senderMock = new();
		_uut = new(_senderMock.Object);
		_cancellationToken = new();
	}

	[Theory]
	[InlineData("code")]
	public async void NewInstallation_SenderReturnsSuccess_ReturnsOk(string code)
	{
		// Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<NewInstallationCommand>(), _cancellationToken))
			.ReturnsAsync(Result.Success);

		// Act
		IActionResult result = await _uut.NewInstallation(code, _cancellationToken);

		// Assert
		_senderMock.Verify(s => s.Send(It.IsAny<NewInstallationCommand>(), _cancellationToken));

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
			.Setup(s => s.Send(It.IsAny<NewInstallationCommand>(), _cancellationToken))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		// Act
		IActionResult result = await _uut.NewInstallation(code, _cancellationToken);

		// Assert
		_senderMock.Verify(s => s.Send(It.IsAny<NewInstallationCommand>(), _cancellationToken));

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingEmptyBody_NoCommandsSendReturnsOk()
	{
		// Arrange
		List<WebHookRequest> requests = new();

		// Act
		IActionResult result = await _uut.Hook(requests, _cancellationToken);

		// Assert
		_senderMock.Verify(s => s.Send(It.IsAny<ICommand>(), _cancellationToken), Times.Never());

		Assert.IsType<OkResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneUnhandledRequest_NoCommandsSendReturnsOk()
	{
		// Arrange
		List<WebHookRequest> requests = new()
		{
			new WebHookRequest { SubscriptionType = "HelloWorld"}
		};

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), _cancellationToken))
			.ReturnsAsync(Result.Success);

		// Act
		IActionResult result = await _uut.Hook(requests, _cancellationToken);

		// Assert
		_senderMock.Verify(s => s.Send(It.IsAny<ICommand>(), _cancellationToken), Times.Never());

		Assert.IsType<OkResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneDealDeletionRequestResultSuccess_OneCommandSendReturnsOk()
	{
		// Arrange
		List<WebHookRequest> requests = new()
		{
			new WebHookRequest { SubscriptionType = "deal.deletion"}
		};

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), _cancellationToken))
			.ReturnsAsync(Result.Success);

		// Act
		IActionResult result = await _uut.Hook(requests, _cancellationToken);

		// Assert
		_senderMock.Verify(s => s.Send(It.IsAny<ICommand>(), _cancellationToken), Times.Once());

		_senderMock.Verify(s => s.Send(It.IsAny<DeleteDealCommand>(), _cancellationToken), Times.Once());

		Assert.IsType<OkResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneDealDeletionRequestResultFailure_OneCommandSendReturnsBadRequest()
	{
		// Arrange
		List<WebHookRequest> requests = new()
		{
			new WebHookRequest { SubscriptionType = "deal.deletion"}
		};

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), _cancellationToken))
			.ReturnsAsync(Result.Failure(new Error("0", "fail")));

		// Act
		IActionResult result = await _uut.Hook(requests, _cancellationToken);

		// Assert
		_senderMock.Verify(s => s.Send(It.IsAny<ICommand>(), _cancellationToken), Times.Once());

		_senderMock.Verify(s => s.Send(It.IsAny<DeleteDealCommand>(), _cancellationToken), Times.Once());

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneDealPropertyChangeRequestResultSuccess_OneCommandSendReturnsOk()
	{
		// Arrange
		List<WebHookRequest> requests = new()
		{
			new WebHookRequest { SubscriptionType = "deal.propertyChange", PortalId=0 }
		};

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), _cancellationToken))
			.ReturnsAsync(Result.Success);

		// Act
		IActionResult result = await _uut.Hook(requests, _cancellationToken);

		// Assert
		_senderMock.Verify(s => s.Send(It.IsAny<ICommand>(), _cancellationToken), Times.Once());

		_senderMock.Verify(s => s.Send(It.IsAny<UpdateDealCommand>(), _cancellationToken), Times.Once());

		Assert.IsType<OkResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneDealPropertyChangeRequestWithEmptyNameAndValueResultFailure_OneCommandSendReturnsBadRequest()
	{
		// Arrange
		List<WebHookRequest> requests = new()
		{
			new WebHookRequest { SubscriptionType = "deal.propertyChange", PortalId=0}
		};

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), _cancellationToken))
			.ReturnsAsync(Result.Failure(new Error("0", "fail")));

		// Act
		IActionResult result = await _uut.Hook(requests, _cancellationToken);

		// Assert
		_senderMock.Verify(s => s.Send(It.IsAny<ICommand>(), _cancellationToken), Times.Once());

		_senderMock.Verify(s => s.Send(It.IsAny<UpdateDealCommand>(), _cancellationToken), Times.Once());

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneDealPropertyChangeRequestResultFailure_OneCommandSendReturnsBadRequest()
	{
		// Arrange
		List<WebHookRequest> requests = new()
		{
			new WebHookRequest { SubscriptionType = "deal.propertyChange", PortalId=0, PropertyName="propertyName", PropertyValue="newValue"}
			};

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), _cancellationToken))
			.ReturnsAsync(Result.Failure(new Error("0", "fail")));

		// Act
		IActionResult result = await _uut.Hook(requests, _cancellationToken);

		// Assert
		_senderMock.Verify(s => s.Send(It.IsAny<ICommand>(), _cancellationToken), Times.Once());

		_senderMock.Verify(s => s.Send(It.IsAny<UpdateDealCommand>(), _cancellationToken), Times.Once());

		Assert.IsType<BadRequestObjectResult>(result);
	}
}
