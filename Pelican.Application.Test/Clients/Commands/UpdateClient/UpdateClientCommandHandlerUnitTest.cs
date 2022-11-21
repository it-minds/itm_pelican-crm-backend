using System.Linq.Expressions;
using Moq;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Clients.Commands.UpdateClient;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Xunit;

namespace Pelican.Application.Test.Clients.Commands.UpdateClient;

public class UpdateClientCommandHandlerTests
{
	private readonly UpdateClientCommandHandler _uut;
	private readonly Mock<IUnitOfWork> _unitOfWorkMock;
	private readonly Mock<IGenericRepository<Client>> _clientRepositoryMock;
	private readonly Mock<IGenericRepository<Supplier>> _supplierRepositoryMock;
	private readonly Mock<IHubSpotObjectService<Client>> _hubSpotClientServiceMock;
	private readonly Mock<IHubSpotObjectService<Contact>> _hubSpotContactServiceMock;
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock;
	private readonly CancellationToken _cancellationToken;

	public UpdateClientCommandHandlerTests()
	{
		_unitOfWorkMock = new();
		_clientRepositoryMock = new();
		_supplierRepositoryMock = new();
		_hubSpotClientServiceMock = new();
		_hubSpotAuthorizationServiceMock = new();
		_hubSpotContactServiceMock = new();

		_uut = new(
			_unitOfWorkMock.Object,
			_hubSpotClientServiceMock.Object,
			_hubSpotAuthorizationServiceMock.Object);

		_cancellationToken = new();
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientNotFoundSupplierNotFound_ReturnsFailureErrorNullValue(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		_unitOfWorkMock
			.Setup(u => u.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock
			.Setup(u => u.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(Enumerable.Empty<Supplier>().AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(h => h.RefreshAccessTokenAsync(It.IsAny<long>(), It.IsAny<IUnitOfWork>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<string>(Error.NullValue));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock
			.Verify(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(Client => Client.HubSpotId == command.ObjectId.ToString()),
			Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientNotFoundGetAccesTokenButClientNotFoundOnHubSpot_ReturnsFailureAndErrorNullValue(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<long>(), _unitOfWorkMock.Object, It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("AccessToken"));

		_hubSpotClientServiceMock.Setup(h => h.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Client>(Error.NullValue));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_hubSpotClientServiceMock.Verify(service => service.GetByIdAsync("AccessToken", portalId, _cancellationToken));

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientNotFoundClientCreatedWithNoLocalOrRemoteAssociations_ReturnsSuccesAndCreatesNewClient(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};
		Mock<Client> clientMock = new();

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ContactRepository
				.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(Enumerable.Empty<Contact>().AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<long>(), _unitOfWorkMock.Object, It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("AccessToken"));

		_hubSpotClientServiceMock
			.Setup(h => h
				.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(clientMock.Object));


		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		clientMock
			.Verify(c => c
				.FillOutClientContacts(It.IsAny<List<Contact>>()), Times.Once());

		Assert.True(result.IsSuccess);

		_unitOfWorkMock.Verify(u => u.SaveAsync(_cancellationToken), Times.Once());

		_unitOfWorkMock.Verify(u => u.ClientRepository.CreateAsync(clientMock.Object, _cancellationToken), Times.Once());
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientNotFoundClientCreatedWithRemoteAssoicationMatchingLocalAssociation_ReturnsSuccesAndCreatesNewClient(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};
		Mock<Client> clientMock = new();
		ClientContact clientContact = new();
		List<ClientContact> clientContactList = new();
		Contact contact = new(Guid.NewGuid())
		{
			HubSpotId = "HubSpotId"
		};
		List<Contact> contactList = new();
		contactList.Add(contact);
		clientContact.Contact = contact;
		clientContactList.Add(clientContact);
		clientMock.Object.ClientContacts = clientContactList;

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ContactRepository
				.FirstOrDefaultAsync(It.IsAny<Expression<Func<Contact, bool>>>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(contact);

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<long>(), _unitOfWorkMock.Object, It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("AccessToken"));

		_hubSpotClientServiceMock
			.Setup(h => h
				.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success(clientMock.Object));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		clientMock
			.Verify(c => c
				.FillOutClientContacts(contactList), Times.Once());

		Assert.True(result.IsSuccess);

		_unitOfWorkMock.Verify(u => u.SaveAsync(_cancellationToken), Times.Once());

		_unitOfWorkMock.Verify(u => u.ClientRepository.CreateAsync(clientMock.Object, _cancellationToken), Times.Once());
	}

	[Theory]
	[InlineData(0, 0, "property", "value")]
	public async void Handle_ClientFoundAndIsUpdated_ReturnsSucces(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Mock<Client> clientMock = new();

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { clientMock.Object }.AsQueryable());

		// Act
		var result = await _uut.Handle(command, _cancellationToken);

		// Assert
		Assert.True(result.IsSuccess);

		_unitOfWorkMock.Verify(u => u.SaveAsync(_cancellationToken), Times.Once());
		_unitOfWorkMock.Verify(u => u
			.ClientRepository
			.Update(clientMock.Object));
	}

	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedHubSpotAuthorizationServiceReturnsFailure_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Mock<Client> clientMock = new();

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { clientMock.Object }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(h => h.RefreshAccessTokenAsync(It.IsAny<long>(), It.IsAny<IUnitOfWork>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<string>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, _cancellationToken);

		//Assert
		_hubSpotAuthorizationServiceMock
			.Verify(h => h.RefreshAccessTokenAsync(portalId, _unitOfWorkMock.Object, _cancellationToken), Times.Once());
		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedHubSpotClientServiceReturnsFailure_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Mock<Client> clientMock = new();

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { clientMock.Object }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(h => h.RefreshAccessTokenAsync(It.IsAny<long>(), It.IsAny<IUnitOfWork>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("AccessToken"));

		_hubSpotClientServiceMock
			.Setup(h => h.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Client>(Error.NullValue));

		// Act
		var result = await _uut.Handle(command, _cancellationToken);

		//Assert
		_hubSpotAuthorizationServiceMock
			.Verify(h => h.RefreshAccessTokenAsync(portalId, _unitOfWorkMock.Object, _cancellationToken), Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedHubSpotClientServiceReturnsClient_ReturnsSucces(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Mock<Client> clientMock = new();

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { clientMock.Object }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(h => h.RefreshAccessTokenAsync(It.IsAny<long>(), It.IsAny<IUnitOfWork>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("AccessToken"));

		_hubSpotClientServiceMock
			.Setup(h => h.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(new Client(Guid.NewGuid()));

		// Act
		var result = await _uut.Handle(command, _cancellationToken);

		//Assert
		clientMock.Verify(c => c.UpdateClientContacts(clientMock.Object.ClientContacts), Times.Once());

		_unitOfWorkMock.Verify(u => u.SaveAsync(_cancellationToken), Times.Once());

		_unitOfWorkMock.Verify(u => u.ClientRepository.Update(clientMock.Object));

		Assert.True(result.IsSuccess);
	}
}
