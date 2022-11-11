using Moq;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.Contacts.Commands.UpdateContact;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Application.Test.Contacts;
public class UpdateContactCommandHandlerTests
{
	private readonly UpdateContactCommandHandler _uut;

	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly Mock<IHubSpotObjectService<Contact>> _hubSpotContactServiceMock = new();
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock = new();

	private const long OBJECT_ID = 0;
	private const long SUPPLIER_HUBSPOT_ID = 0;
	private const string NAME = "name";
	private const string VALUE = "value";

	private readonly UpdateContactCommand DEFAULT_COMMAND = new(
		OBJECT_ID,
		SUPPLIER_HUBSPOT_ID,
		NAME,
		VALUE);

	public UpdateContactCommandHandlerTests()
	{
		_uut = new(
			_unitOfWorkMock.Object,
			_hubSpotContactServiceMock.Object,
			_hubSpotAuthorizationServiceMock.Object);
	}

	[Fact]
	public void UpdateContactCommandHandler_UnitOfWorkNull_ThrowsArgumentNullException()
	{
		// Act
		var result = Record.Exception(() => new UpdateContactCommandHandler(
			null!,
			_hubSpotContactServiceMock.Object,
			_hubSpotAuthorizationServiceMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void UpdateContactCommandHandler_HubSpotContactServiceNull_ThrowsArgumentNullException()
	{
		// Act
		var result = Record.Exception(() => new UpdateContactCommandHandler(
			_unitOfWorkMock.Object,
			null!,
			_hubSpotAuthorizationServiceMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void UpdateContactCommandHandler_HubSpotAuthorizationServiceNull_ThrowsArgumentNullException()
	{
		// Act
		var result = Record.Exception(() => new UpdateContactCommandHandler(
			_unitOfWorkMock.Object,
			_hubSpotContactServiceMock.Object,
			null!));

		// Assert
		Assert.IsType<ArgumentNullException>(result);
	}

	[Fact]
	public void Handle_ClientNotFoundFetchingAccessTokenFailed_ReturnsFailure()
	{
		// Arrange


		//_hubSpotAuthorizationServiceMock
		//	.Setup(service=>service.


		// Act
		var result = _uut.Handle(DEFAULT_COMMAND, default);

		// Assert

	}





}
