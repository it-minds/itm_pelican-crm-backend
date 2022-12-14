using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Deals.HubSpotCommands.Delete;
using Pelican.Application.HubSpot.Commands.NewInstallation;
using Pelican.Domain.Shared;
using Pelican.Presentation.Api.Abstractions;
using Pelican.Presentation.Api.Contracts;
using Pelican.Presentation.Api.Controllers;
using Xunit;

namespace Pelican.Presentation.Api.Test.Controllers;
public class HubSpotControllerTests
{
	private const long OBJECT_ID = 0;

	private readonly HubSpotController _uut;
	private readonly Mock<ISender> _senderMock;
	private readonly Mock<IRequestToCommandMapper> _mapperMock;

	public HubSpotControllerTests()
	{
		_senderMock = new();
		_mapperMock = new();
		_uut = new(_senderMock.Object, _mapperMock.Object);
	}

	[Theory]
	[InlineData("code")]
	public async void NewInstallation_SenderReturnsSuccess_ReturnsOk(string code)
	{
		// Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<NewInstallationHubSpotCommand>(), default))
			.ReturnsAsync(Result.Success);

		// Act
		IActionResult result = await _uut.NewInstallation(code, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				It.Is<NewInstallationHubSpotCommand>(n => n.Code == code),
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
			.Setup(s => s.Send(It.IsAny<NewInstallationHubSpotCommand>(), default))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		// Act
		IActionResult result = await _uut.NewInstallation(code, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				It.Is<NewInstallationHubSpotCommand>(n => n.Code == code),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}


	[Fact]
	public async void Hook_ReceivedRequestSendToMapper_MapperCalledWithArg()
	{
		// Arrange
		List<HubSpotWebHookRequest> requests = new()
		{
			new(),
		};

		_mapperMock
			.Setup(mapper => mapper
				.ConvertToCommands(It.IsAny<IReadOnlyCollection<HubSpotWebHookRequest>>()))
			.Returns(new List<ICommand>());

		// Act
		IActionResult result = await _uut.Hook(requests, default);

		// Assert
		_mapperMock.Verify(
			s => s.ConvertToCommands(requests),
			Times.Once);
	}


	[Fact]
	public async void Hook_EmptyCommandCollectionFromMapper_NoCommandsSendReturnsOk()
	{
		// Arrange
		_mapperMock
			.Setup(mapper => mapper
				.ConvertToCommands(It.IsAny<IReadOnlyCollection<HubSpotWebHookRequest>>()))
			.Returns(new List<ICommand>());

		// Act
		IActionResult result = await _uut.Hook(null!, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()),
			Times.Never);

		Assert.IsType<OkResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneCommandRequestResultSuccess_OneCommandSendReturnsOk()
	{
		// Arrange
		ICommand command = new DeleteDealHubSpotCommand(OBJECT_ID);

		_mapperMock
			.Setup(mapper => mapper
				.ConvertToCommands(It.IsAny<IReadOnlyCollection<HubSpotWebHookRequest>>()))
			.Returns(new List<ICommand>() { new DeleteDealHubSpotCommand(OBJECT_ID) });

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success);

		// Act
		IActionResult result = await _uut.Hook(null!, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				command,
				default),
			Times.Once);

		Assert.IsType<OkResult>(result);
	}

	[Fact]
	public async void Hook_ReceivingOneCommandRequestResultFailure_OneCommandSendReturnsBadRequest()
	{
		// Arrange
		ICommand command = new DeleteDealHubSpotCommand(OBJECT_ID);

		_mapperMock
			.Setup(mapper => mapper
				.ConvertToCommands(It.IsAny<IReadOnlyCollection<HubSpotWebHookRequest>>()))
			.Returns(new List<ICommand>() { new DeleteDealHubSpotCommand(OBJECT_ID) });

		Error error = Error.NullValue;

		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure(error));

		// Act
		IActionResult result = await _uut.Hook(null!, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				command,
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}
}
