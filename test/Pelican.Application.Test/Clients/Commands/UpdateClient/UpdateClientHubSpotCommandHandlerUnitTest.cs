using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Clients.HubSpotCommands.UpdateClient;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Clients.Commands.UpdateClient;

public class UpdateClientCommandHandlerTests
{
	private readonly UpdateClientHubSpotCommandHandler _uut;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly Mock<IHubSpotObjectService<Client>> _hubSpotClientServiceMock;
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock;

	public UpdateClientCommandHandlerTests()
	{
		_unitOfWorkMock = new();
		_hubSpotClientServiceMock = new();
		_hubSpotAuthorizationServiceMock = new();

		_uut = new(
			_unitOfWorkMock.Object,
			_hubSpotClientServiceMock.Object,
			_hubSpotAuthorizationServiceMock.Object);
	}
	[Fact]
	public void UpdateClientCommandHandler_UnitOfWorkNull_ThrowsArgumentNullException()
	{
		// Act
		var result = Record.Exception(() => new UpdateClientHubSpotCommandHandler(
			null!,
			_hubSpotClientServiceMock.Object,
			_hubSpotAuthorizationServiceMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"unitOfWork",
			result.Message);
	}

	[Fact]
	public void UpdateClientCommandHandler_HubSpotContactServiceNull_ThrowsArgumentNullException()
	{
		// Act
		var result = Record.Exception(() => new UpdateClientHubSpotCommandHandler(
			_unitOfWorkMock.Object,
			null!,
			_hubSpotAuthorizationServiceMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"hubSpotClientService",
			result.Message);
	}

	[Fact]
	public void UpdateClientCommandHandler_HubSpotAuthorizationServiceNull_ThrowsArgumentNullException()
	{
		// Act
		var result = Record.Exception(() => new UpdateClientHubSpotCommandHandler(
			_unitOfWorkMock.Object,
			_hubSpotClientServiceMock.Object,
			null!));

		// Assert
		Assert.IsType<ArgumentNullException>(result);

		Assert.Contains(
			"hubSpotAuthorizationService",
			result.Message);
	}

	[Theory]
	[InlineData(0, 0, 1, "0", "0")]
	public async void Handle_ClientNotFoundRefreshAccessTokenReturnsFailure_ReturnsFailureErrorNullValue(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		_unitOfWorkMock.Setup(
			u => u.ClientRepository.FindByCondition(
				It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_hubSpotAuthorizationServiceMock.Setup(
			h => h.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<string>(Error.NullValue));

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.ClientRepository.FindByCondition(
				Client => Client.SourceId == command.ObjectId.ToString() && Client.Source == Sources.HubSpot),
			Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, 0, "0", "0")]
	public async void Handle_ClientNotFoundGetAccesTokenSuccessButClientNotFoundOnHubSpot_ReturnsFailureAndErrorNullValue(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};

		_unitOfWorkMock.Setup(
			unitOfWork => unitOfWork.ClientRepository.FindByCondition(
				It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_hubSpotAuthorizationServiceMock.Setup(
			service => service.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("AccessToken"));

		_hubSpotClientServiceMock.Setup(
			h => h.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Client>(Error.NullValue));

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		_hubSpotClientServiceMock.Verify(
			service => service.GetByIdAsync(
				"AccessToken",
				portalId,
				default),
			Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, 0, "0", "0")]
	public async void Handle_ClientNotFoundClientCreatedWithNoLocalOrRemoteAssociations_ReturnsSuccesAndCreatesNewClient(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};
		Mock<Client> clientMock = new();

		_unitOfWorkMock.Setup(
			unitOfWork => unitOfWork.ClientRepository.FindByCondition(
				It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock.Setup(
			unitOfWork => unitOfWork.ContactRepository.FindByCondition(
				It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(Enumerable.Empty<Contact>().AsQueryable());

		_hubSpotAuthorizationServiceMock.Setup(
			service => service.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("AccessToken"));

		_hubSpotClientServiceMock.Setup(
			h => h.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(clientMock.Object));

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		clientMock.Verify(
			c => c.FillOutClientContacts(
				It.IsAny<List<Contact>>()),
			Times.Once);

		Assert.True(result.IsSuccess);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(
				default),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.ClientRepository.CreateAsync(
				clientMock.Object,
				default),
			Times.Once);
	}

	[Theory]
	[InlineData(0, 0, 0, "0", "0")]
	public async void Handle_ClientNotFoundClientCreatedWithRemoteAssociationMatchingLocalAssociation_ReturnsSuccesAndCreatesNewClient(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};
		Mock<Client> clientMock = new();
		ClientContact clientContact = new();
		List<ClientContact> clientContactList = new();
		Contact contact = new(Guid.NewGuid())
		{
			SourceId = "HubSpotId"
		};
		List<Contact> contactList = new();
		contactList.Add(contact);
		clientContact.Contact = contact;
		clientContactList.Add(clientContact);
		clientMock.Object.ClientContacts = clientContactList;

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.ContactRepository
				.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(new List<Contact>() { contact }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("AccessToken"));

		_hubSpotClientServiceMock
			.Setup(h => h.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(clientMock.Object));

		// Act
		Result result = await _uut.Handle(command, default);

		// Assert
		clientMock.Verify(
			c => c.FillOutClientContacts(
				contactList),
			Times.Once);

		Assert.True(result.IsSuccess);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(
				default),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.ClientRepository.CreateAsync(
				clientMock.Object,
				default),
			Times.Once);
	}

	[Theory]
	[InlineData(0, 0, 0, "property", "value")]
	public async void Handle_ClientFoundClientUpdateCalled_ReturnsSucces(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Mock<Client> clientMock = new();

		_unitOfWorkMock.Setup(
			unitOfWork => unitOfWork.ClientRepository.FindByCondition(
				It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { clientMock.Object }.AsQueryable());

		// Act
		var result = await _uut.Handle(command, default);

		// Assert
		Assert.True(result.IsSuccess);

		clientMock.Verify(
			c => c.UpdateProperty(
				propertyName,
				propertyValue,
				updateTime),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(
				default),
			Times.Once);
	}

	[Theory]
	[InlineData(0, 0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedHubSpotAuthorizationServiceReturnsFailure_ReturnsFailure(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateClientHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Mock<Client> clientMock = new();

		_unitOfWorkMock.Setup(
			unitOfWork => unitOfWork.ClientRepository.FindByCondition(
				It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { clientMock.Object }.AsQueryable());

		_hubSpotAuthorizationServiceMock.Setup(
			h => h.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<string>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, default);

		//Assert
		_hubSpotAuthorizationServiceMock.Verify(
			h => h.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				portalId,
				_unitOfWorkMock.Object,
				default),
			Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedHubSpotClientServiceReturnsFailure_ReturnsFailure(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateClientHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Mock<Client> clientMock = new();

		_unitOfWorkMock.Setup(
			unitOfWork => unitOfWork.ClientRepository.FindByCondition(
				It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { clientMock.Object }.AsQueryable());

		_hubSpotAuthorizationServiceMock.Setup(
			h => h.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("AccessToken"));

		_hubSpotClientServiceMock.Setup(
			h => h.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Client>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, default);

		//Assert
		_hubSpotAuthorizationServiceMock.Verify(
			h => h.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				portalId,
				_unitOfWorkMock.Object,
				default),
			Times.Once);

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedHubSpotClientServiceReturnsClient_ReturnsSuccess(
		long objectId,
		long portalId,
		long updateTime,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateClientHubSpotCommand command = new(objectId, portalId, updateTime, propertyName, propertyValue);

		Mock<Client> clientMock = new();

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.ClientRepository
				.FindByCondition(
					It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { clientMock.Object }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(h => h.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				It.IsAny<long>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("AccessToken"));

		_hubSpotClientServiceMock
			.Setup(h => h.GetByIdAsync(
				It.IsAny<string>(),
				It.IsAny<long>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Client(Guid.NewGuid()));

		_unitOfWorkMock
			.Setup(u => u
				.ClientContactRepository
				.AttachAsAdded(It.IsAny<IEnumerable<ClientContact>>()));

		// Act
		var result = await _uut.Handle(command, default);

		//Assert
		clientMock.Verify(
			c => c.UpdateClientContacts(
				clientMock.Object.ClientContacts, updateTime),
			Times.Once);

		_unitOfWorkMock.Verify(
			u => u.SaveAsync(default),
			Times.Once);

		Assert.True(result.IsSuccess);
	}
}
