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
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock;
	private readonly CancellationToken _cancellationToken;

	public UpdateClientCommandHandlerTests()
	{
		_unitOfWorkMock = new();
		_clientRepositoryMock = new();
		_supplierRepositoryMock = new();
		_hubSpotClientServiceMock = new();
		_hubSpotAuthorizationServiceMock = new();

		_uut = new(
			_unitOfWorkMock.Object,
			_hubSpotClientServiceMock.Object,
			_hubSpotAuthorizationServiceMock.Object);

		_cancellationToken = new();
	}

	private void SetupUnitOfWork()
	{
		_unitOfWorkMock
			.Setup(u => u.ClientRepository)
			.Returns(_clientRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(u => u.SupplierRepository)
			.Returns(_supplierRepositoryMock.Object);
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

		SetupUnitOfWork();

		_unitOfWorkMock
			.Setup(u => u.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock
			.Setup(u => u.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(Enumerable.Empty<Supplier>().AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock
			.Verify(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(Client => Client.HubSpotId == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock
			.Verify(unitOfWork => unitOfWork.SupplierRepository
				.FindByCondition(supplier => supplier.HubSpotId == portalId),
			Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientNotFoundFailsRefreshingToken_ReturnsFailureAndErrorCode0AndErrorMessageError(
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
				.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<string>(new Error("0", "error")));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_hubSpotAuthorizationServiceMock
			.Verify(service => service
					.RefreshAccessTokenAsync(It.IsAny<string>(), _cancellationToken),
				Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal("0", result.Error.Code);
		Assert.Equal("error", result.Error.Message);
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientNotFoundSupplierRefreshTokenIsEmptyStringOnHubSpot_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "",
		};

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock.
			Setup(unitOfWork => unitOfWork.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock
			.Verify(x => x.SupplierRepository
				.FindByCondition(s => s.HubSpotId == portalId));
		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientNotFoundSupplierRefreshTokenIsNullOnHubSpot_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Supplier supplier = new(Guid.NewGuid());
		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock
			.Verify(x => x.SupplierRepository
				.FindByCondition(s => s.HubSpotId == portalId));
		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientNotFoundFailureFetchingFromClientHubSpot_ReturnsFailure(
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
				.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("token"));

		_hubSpotClientServiceMock
			.Setup(service => service
				.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Client>(Error.NullValue));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_hubSpotClientServiceMock
			.Verify(service => service
					.GetByIdAsync("token", command.ObjectId, _cancellationToken),
				Times.Once());

		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientNotFoundSuccessFetchingClientFromHubSpot_ReturnsSuccess(
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

		Client client = new(Guid.NewGuid());

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
				.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("token"));

		_hubSpotClientServiceMock
			.Setup(service => service
				.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(client);

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock
			.Verify(x => x.ClientRepository
					.CreateAsync(client, _cancellationToken),
				Times.Once());

		_unitOfWorkMock
			.Verify(x => x
					.SaveAsync(_cancellationToken),
				Times.Once());
		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientNotFoundSuccessFetchingClientFromWithClientContactsButHubspotClientContact_ContactNull_ReturnsSuccess(
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
		Guid ContactId = Guid.NewGuid();
		string localContactHubSpotId = Guid.NewGuid().ToString();
		Client client = new(Guid.NewGuid())
		{
			ClientContacts = new List<ClientContact>()
			{
				new ClientContact(Guid.NewGuid())
				{
					ContactId = ContactId,
					HubSpotContactId = localContactHubSpotId,
					Contact= new Contact(ContactId)
					{
						HubSpotId=localContactHubSpotId
					}
				}
			}
		};

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_unitOfWorkMock.Setup(
			unitOfWork => unitOfWork.ContactRepository
				.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(Enumerable.Empty<Contact>().AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("token"));

		_hubSpotClientServiceMock
			.Setup(service => service
				.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(client);

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock
			.Verify(x => x.ClientRepository
					.CreateAsync(client, _cancellationToken),
				Times.Once());

		_unitOfWorkMock
			.Verify(x => x
					.SaveAsync(_cancellationToken),
				Times.Once());
		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientNotFoundSuccessFetchingClientFromWithClientContactsHubspotClientContact_ContactEqualToLocal_ReturnsSuccess(
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
		Guid contactId = Guid.NewGuid();
		string localContactHubSpotId = Guid.NewGuid().ToString();
		Client client = new(Guid.NewGuid())
		{
			ClientContacts = new List<ClientContact>()
			{
				new ClientContact(Guid.NewGuid())
				{
					ContactId = contactId,
					HubSpotContactId = localContactHubSpotId,
					Contact= new Contact(contactId)
					{
						HubSpotId=localContactHubSpotId
					}
				}
			}
		};
		string hubSpotClientContactContactId = Guid.NewGuid().ToString();
		List<Contact> hubSpotContact = new List<Contact>();
		hubSpotContact.Add(new Contact(Guid.NewGuid())
		{
			HubSpotId = localContactHubSpotId,
		});

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_unitOfWorkMock.Setup(
			unitOfWork => unitOfWork.ContactRepository
				.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(hubSpotContact.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("token"));

		_hubSpotClientServiceMock
			.Setup(service => service
				.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(client);

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		Assert.Equal(hubSpotContact.First(), client.ClientContacts.First().Contact);

		_unitOfWorkMock
			.Verify(x => x.ClientRepository
					.CreateAsync(client, _cancellationToken),
				Times.Once());

		_unitOfWorkMock
			.Verify(x => x
					.SaveAsync(_cancellationToken),
				Times.Once());
		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "invalid_property", "value")]
	public async void Handle_ClientFoundInvalidPropertyUpdated_ReturnsException(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client client = new(Guid.NewGuid());

		Client updatedClient = client;
		updatedClient.Name = command.PropertyValue;

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { client }.AsQueryable());

		// Act
		var ex = await Record.ExceptionAsync(() => _uut.Handle(command, _cancellationToken));

		// Assert
		Assert.NotNull(ex);
		Assert.IsType<ArgumentException>(ex);
	}

	[Theory]
	[InlineData(0, 0, "name", "TestCompanyName")]
	public async void Handle_ClientFoundNameUpdated_ReturnsSuccessAndClientWasUpdated(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client client = new(Guid.NewGuid());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { client }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		Assert.Equal(command.PropertyValue, client.Name);

		_unitOfWorkMock
			.Verify(unitOfWork => unitOfWork.ClientRepository
					.Update(client),
				Times.Once());

		_unitOfWorkMock
			.Verify(unitOfWork => unitOfWork
					.SaveAsync(_cancellationToken),
				Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "website", "www.testUrl.com")]
	public async void Handle_ClientFoundWebsiteUpdated_ReturnsSuccessAndClientWasUpdated(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client client = new(Guid.NewGuid());


		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { client }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		Assert.Equal(command.PropertyValue, client.Website);

		_unitOfWorkMock
			.Verify(unitOfWork => unitOfWork.ClientRepository
					.Update(It.IsAny<Client>()),
				Times.Once());

		_unitOfWorkMock
			.Verify(unitOfWork => unitOfWork
				.SaveAsync(_cancellationToken),
				Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "city", "TestCity")]
	public async void Handle_ClientFoundCityUpdated_ReturnsSuccessAndClientWasUpdated(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client client = new(Guid.NewGuid());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { client }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		Assert.Equal(command.PropertyValue, client.OfficeLocation);

		_unitOfWorkMock
			.Verify(unitOfWork => unitOfWork.ClientRepository
					.Update(It.IsAny<Client>()),
				Times.Once());

		_unitOfWorkMock
			.Verify(unitOfWork => unitOfWork
					.SaveAsync(_cancellationToken),
				Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedContactNotUpdatedSupplierNotFound_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client client = new(Guid.NewGuid());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { client }.AsQueryable());

		_unitOfWorkMock
			.Setup(u => u.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(Enumerable.Empty<Supplier>().AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);
		//Assert
		_unitOfWorkMock
			.Verify(unitOfWork => unitOfWork.SupplierRepository
					.FindByCondition(supplier => supplier.HubSpotId == portalId),
				Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedContactNotUpdatedSupplierRefreshTokenIsEmptyStringOnHubSpot_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client client = new(Guid.NewGuid());

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "",
		};

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { client }.AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock
			.Verify(x => x.SupplierRepository
					.FindByCondition(s => s.HubSpotId == portalId),
				Times.Once());
		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedContactNotUpdatedAccessTokenNotRefreshed_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client client = new(Guid.NewGuid());

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { client }.AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<string>(new Error("0", "error")));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);
		//Assert
		_hubSpotAuthorizationServiceMock
			.Verify(service => service
					.RefreshAccessTokenAsync(It.IsAny<string>(), _cancellationToken),
				Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal("0", result.Error.Code);
		Assert.Equal("error", result.Error.Message);
	}
	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedContactNotUpdatedClientOnHubSpotNotFound_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);
		Client client = new(Guid.NewGuid());

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { client }.AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("token"));

		_hubSpotClientServiceMock
			.Setup(service => service
				.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Client>(Error.NullValue));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);
		//Assert
		_hubSpotClientServiceMock
			.Verify(service => service
					.GetByIdAsync("token", command.ObjectId, _cancellationToken),
				Times.Once());

		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}

	private void MockSetupForNumAssociatedContactsCaseSuccess(Client client, Client HubSpotClient, Contact? contact)
	{
		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { client }.AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_unitOfWorkMock.Setup(
			unitOfWork => unitOfWork.ContactRepository
				.FindByCondition(It.IsAny<Expression<Func<Contact, bool>>>()))
			.Returns(new List<Contact> { contact }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("token"));

		_hubSpotClientServiceMock
			.Setup(service => service
				.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(HubSpotClient);
	}

	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedContactUpdatedButNoClientContactsInClientExisted_ReturnsSuccess(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);
		Client client = new(Guid.NewGuid());
		Client ClientHubSpot = new(Guid.NewGuid());
		Contact contact = new(Guid.NewGuid())
		{
			HubSpotId = Guid.NewGuid().ToString()
		};
		ClientContact clientContactHubSpot = new(Guid.NewGuid())
		{
			HubSpotContactId = contact.HubSpotId
		};
		ClientHubSpot.ClientContacts.Add(clientContactHubSpot);
		MockSetupForNumAssociatedContactsCaseSuccess(client, ClientHubSpot, contact);

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		//Assert

		Assert.NotEmpty(client.ClientContacts);
		Assert.Equal(clientContactHubSpot, client.ClientContacts.First());

		_unitOfWorkMock
			.Verify(x => x.ClientRepository
					.Update(client),
				Times.Once());
		_unitOfWorkMock
			.Verify(x => x
					.SaveAsync(_cancellationToken),
				Times.Once());

		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedContactUpdatedButClientContactsInClientNotInHubSpotClient_ReturnsSuccessAndIsActiveSetFalse(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);
		Client client = new(Guid.NewGuid())
		{
			ClientContacts = new List<ClientContact>()
			{
				new ClientContact
				{
					HubSpotContactId = Guid.NewGuid().ToString(),
					HubSpotClientId = Guid.NewGuid().ToString()
				}
			}
		};

		Client ClientHubSpot = new(Guid.NewGuid());
		Contact contact = new(Guid.NewGuid())
		{
			HubSpotId = Guid.NewGuid().ToString()
		};
		client.ClientContacts.Add(new(Guid.NewGuid())
		{
			Contact = contact,
			HubSpotContactId = client.ClientContacts.First().HubSpotContactId,
			HubSpotClientId = client.ClientContacts.First().HubSpotClientId
		});

		MockSetupForNumAssociatedContactsCaseSuccess(client, ClientHubSpot, contact);

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		//Assert

		Assert.NotEmpty(client.ClientContacts);
		Assert.False(client.ClientContacts.First().IsActive);

		_unitOfWorkMock
			.Verify(x => x.ClientRepository
					.Update(client),
				Times.Once());

		_unitOfWorkMock
			.Verify(x => x
					.SaveAsync(_cancellationToken),
				Times.Once());

		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedContactUpdatedNewClientContactInHubSpotAddedToLocalClient_ReturnsSuccess(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);
		Client client = new(Guid.NewGuid())
		{
			ClientContacts = new List<ClientContact>()
			{
				new ClientContact(Guid.NewGuid())
				{
					HubSpotClientId= Guid.NewGuid().ToString(),
					HubSpotContactId= Guid.NewGuid().ToString(),
				}
			}
		};

		Client clientHubSpot = new(Guid.NewGuid());
		Contact contact = new(Guid.NewGuid())
		{
			HubSpotId = Guid.NewGuid().ToString()
		};
		clientHubSpot.ClientContacts.Add(new(Guid.NewGuid())
		{
			Contact = contact,
			HubSpotContactId = Guid.NewGuid().ToString(),
			HubSpotClientId = Guid.NewGuid().ToString()
		});

		MockSetupForNumAssociatedContactsCaseSuccess(client, clientHubSpot, contact);

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		//Assert

		Assert.NotEmpty(client.ClientContacts);

		Assert.Equal(clientHubSpot.ClientContacts, client.ClientContacts);

		_unitOfWorkMock
			.Verify(x => x.ClientRepository
					.Update(client),
				Times.Once());

		_unitOfWorkMock
			.Verify(x => x
					.SaveAsync(_cancellationToken),
				Times.Once());

		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedContactUpdatedClientContactsInClientSameAsInHubSpotClient_ReturnsSuccess(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		//Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);
		Contact contact = new Contact(Guid.NewGuid());
		Client client = new(Guid.NewGuid());
		ClientContact clientContact = new(Guid.NewGuid())
		{
			IsActive = true,
			HubSpotClientId = Guid.NewGuid().ToString(),
			HubSpotContactId = Guid.NewGuid().ToString(),
			Contact = contact
		};
		client.ClientContacts.Add(clientContact);

		Client ClientHubSpot = new(Guid.NewGuid());
		ClientContact clientContactHubSpot = new(Guid.NewGuid())
		{
			HubSpotClientId = client.ClientContacts.First().HubSpotClientId,
			HubSpotContactId = client.ClientContacts.First().HubSpotContactId,
			Contact = contact
		};
		ClientHubSpot.ClientContacts.Add(clientContactHubSpot);

		MockSetupForNumAssociatedContactsCaseSuccess(client, ClientHubSpot, contact);

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		//Assert
		Assert.True(client.ClientContacts.First().IsActive);
		_unitOfWorkMock
			.Verify(x => x.ClientRepository
					.Update(client),
				Times.Once());
		_unitOfWorkMock
			.Verify(x => x
					.SaveAsync(_cancellationToken),
				Times.Once());

		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}
}
