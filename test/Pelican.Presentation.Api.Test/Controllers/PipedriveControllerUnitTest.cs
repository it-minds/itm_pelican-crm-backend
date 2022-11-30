using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Deals.Commands.UpdateDeal;
using Pelican.Application.Pipedrive.Commands.NewInstallation;
using Pelican.Domain.Shared;
using Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests;
using Pelican.Presentation.Api.Controllers;
using Xunit;

namespace Pelican.Presentation.Api.Test.Controllers;
public class PipedriveControllerUnitTest
{
	private readonly PipedriveController _uut;
	private readonly Mock<ISender> _senderMock;

	public PipedriveControllerUnitTest()
	{
		_senderMock = new Mock<ISender>();
		_uut = new(_senderMock.Object);
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
			.Setup(s => s.Send(It.IsAny<NewInstallationPipedriveCommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		// Act
		IActionResult result = await _uut.NewInstallation(code, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				It.Is<NewInstallationPipedriveCommand>(n => n.Code == code),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Theory]
	[InlineData("code")]
	public async void NewInstallation_SenderReturnsSuccess_ReturnsOk(string code)
	{
		// Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<NewInstallationPipedriveCommand>(), default))
			.ReturnsAsync(Result.Success);

		// Act
		IActionResult result = await _uut.NewInstallation(code, default);

		// Assert
		_senderMock.Verify(
			s => s.Send(
				It.Is<NewInstallationPipedriveCommand>(n => n.Code == code),
				default),
			Times.Once);

		Assert.IsType<RedirectResult>(result);
	}

	[Theory]
	[InlineData("0", "fail")]
	public async void UpdateDeal_ReceivesCallAndReturnsFailure_FailureIsReturned(
		string errorCode,
		string errorMessage)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		UpdateDealResponse dealResponse = new();

		//Act
		IActionResult result = await _uut.UpdateDeal(dealResponse);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				It.IsAny<UpdateDealPipedriveCommand>(),
				default),
			Times.Once);

		Assert.IsType<BadRequestObjectResult>(result);
	}

	[Fact]
	public async void UpdateDeal_ReceivesCallAndReturnsSuccess_SuccessIsReturned()
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success);

		UpdateDealResponse dealResponse = new();

		//Act
		IActionResult result = await _uut.UpdateDeal(dealResponse);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				It.IsAny<UpdateDealPipedriveCommand>(),
				default),
			Times.Once);

		Assert.IsType<OkResult>(result);
	}
}
