﻿using Moq;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.HubSpot.Commands.NewInstallation;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.HubSpot.Commands.NewInstallation;

public class NewInstallationCommandHandlerTests
{
	private readonly NewInstallationCommandHandler _uut;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock;
	private readonly CancellationToken cancellationToken;

	public NewInstallationCommandHandlerTests()
	{
		_unitOfWorkMock = new Mock<IUnitOfWork>();
		_hubSpotAuthorizationServiceMock = new Mock<IHubSpotAuthorizationService>();
		_uut = new NewInstallationCommandHandler(
			_hubSpotAuthorizationServiceMock.Object,
			_unitOfWorkMock.Object);
		cancellationToken = new();
	}

	[Theory]
	[InlineData("code")]
	public async void Handle_HubSpotServiceReturnsSuccess_ReturnsResultIsSuccessTrueWithErrorNone(
		string code)
	{
		// Arrange
		var command = new NewInstallationCommand(code);

		_hubSpotAuthorizationServiceMock
			.Setup(h => h.AuthorizeUserAsync(command.Code, cancellationToken))
			.ReturnsAsync(Result.Success(new Tuple<string, string>("token", "token")));

		// Act 
		var result = await _uut.Handle(command, cancellationToken);

		// Assert
		_hubSpotAuthorizationServiceMock.Verify(h => h.AuthorizeUserAsync(command.Code, cancellationToken));
		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}

	[Theory]
	[InlineData("code", "0", "fail")]
	public async void Handle_HubSpotServiceReturnsFailure_ReturnsResultIsFailureTrueWithError(
		string code,
		string errorCode,
		string errorMessage)
	{
		// Arrange
		var command = new NewInstallationCommand(code);

		_hubSpotAuthorizationServiceMock
			.Setup(h => h.AuthorizeUserAsync(command.Code, cancellationToken))
			.ReturnsAsync(Result.Failure<Tuple<string, string>>(new Error(errorCode, errorMessage)));

		// Act 
		var result = await _uut.Handle(command, cancellationToken);

		// Assert
		_hubSpotAuthorizationServiceMock.Verify(h => h.AuthorizeUserAsync(command.Code, cancellationToken));
		Assert.True(result.IsFailure);
		Assert.Equal(errorCode, result.Error.Code);
		Assert.Equal(errorMessage, result.Error.Message);
	}
}
