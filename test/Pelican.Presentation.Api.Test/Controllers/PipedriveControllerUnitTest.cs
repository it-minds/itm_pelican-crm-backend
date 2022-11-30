using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Deals.PipedriveCommands.UpdateDeal;
using Pelican.Domain.Shared;
using Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests;
using Pelican.Presentation.Api.Contracts.PipedriveWebHookRequests.UpdateDeal;
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
	[InlineData("0", "fail")]
	public async void UpdateDeal_ReceivesCallAndReturnsFailure_FailureIsReturned(
		string errorCode,
		string errorMessage)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure(new Error(errorCode, errorMessage)));

		UpdateDealRequest dealResponse = new();

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

	[Theory]
	[InlineData("TestDescription", "TestName", 1, "TestContactDate", 1, 1, 1)]
	public async void UpdateDeal_ReceivesCallAndReturnsSuccess_SuccessIsReturned(
		string dealDescription,
		string dealName,
		int dealStatusId,
		string lastContactDate,
		int objectId,
		int supplierPipedriveId,
		int userId)
	{
		//Arrange
		_senderMock
			.Setup(s => s.Send(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success);
		UpdateDealCurrentProperties updateDealCurrentProperties = new()
		{
			DealDescription = dealDescription,
			DealName = dealName,
			DealStatusId = dealStatusId,
			LastContactDate = lastContactDate,
		};
		MetaProperties metaProperties = new()
		{
			ObjectId = objectId,
			SupplierPipedriveId = supplierPipedriveId,
			UserId = userId,
		};
		UpdateDealRequest dealRequest = new()
		{
			CurrentProperties = updateDealCurrentProperties,
			MetaProperties = metaProperties,
		};
		UpdateDealPipedriveCommand expectedCommand = new(
			supplierPipedriveId,
			objectId,
			userId,
			dealStatusId,
			dealDescription,
			dealName,
			lastContactDate,
			null,
			null);

		//Act
		IActionResult result = await _uut.UpdateDeal(dealRequest);

		//Assert
		_senderMock.Verify(
			s => s.Send(
				expectedCommand,
				default),
			Times.Once);

		Assert.IsType<OkResult>(result);
	}
}
