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
	private readonly Mock<IGenericRepository<Client>> _ClientRepositoryMock;
	private readonly Mock<IGenericRepository<Supplier>> _supplierRepositoryMock;
	private readonly Mock<IHubSpotObjectService<Client>> _hubSpotClientServiceMock;
	private readonly Mock<IHubSpotAuthorizationService> _hubSpotAuthorizationServiceMock;
	private readonly CancellationToken _cancellationToken;

	public UpdateClientCommandHandlerTests()
	{
		_unitOfWorkMock = new();
		_ClientRepositoryMock = new();
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
		_unitOfWorkMock.Setup(u => u.ClientRepository)
			.Returns(_ClientRepositoryMock.Object);

		_unitOfWorkMock.Setup(u => u.SupplierRepository)
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

		_unitOfWorkMock.Setup(u => u
				.ClientRepository
					.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
				.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock.Setup(u => u
				.SupplierRepository
					.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
				.Returns(Enumerable.Empty<Supplier>().AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(unitOfWork => unitOfWork.ClientRepository
					.FindByCondition(Client => Client.HubSpotId == command.ObjectId.ToString()),
				Times.Once());

		_unitOfWorkMock.Verify(unitOfWork => unitOfWork.SupplierRepository
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
			.Setup(unitOfWork => unitOfWork
				.SupplierRepository
					.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
				.Returns(new List<Supplier> { supplier }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(Result.Failure<string>(new Error("0", "error")));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_hubSpotAuthorizationServiceMock.Verify(service => service
			.RefreshAccessTokenAsync(It.IsAny<string>(), _cancellationToken), Times.Once());

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

		_unitOfWorkMock.Setup(unitOfWork => unitOfWork.ClientRepository
			.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock.Setup(unitOfWork => unitOfWork
			.SupplierRepository
			.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(x => x.SupplierRepository.FindByCondition(s => s.HubSpotId == portalId));
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
		_unitOfWorkMock.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
				.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock.Setup(unitOfWork => unitOfWork
			.SupplierRepository
			.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(x => x.SupplierRepository.FindByCondition(s => s.HubSpotId == portalId));
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

		_unitOfWorkMock.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
				.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock.Setup(unitOfWork => unitOfWork
			.SupplierRepository
			.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_hubSpotAuthorizationServiceMock.Setup(service => service
			.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("token"));

		_hubSpotClientServiceMock.Setup(service => service
			.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Client>(Error.NullValue));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_hubSpotClientServiceMock.Verify(
			service => service.GetByIdAsync("token", command.ObjectId, _cancellationToken),
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

		Client Client = new(Guid.NewGuid());

		_unitOfWorkMock.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
				.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock.Setup(unitOfWork => unitOfWork
			.SupplierRepository
			.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_hubSpotAuthorizationServiceMock.Setup(service => service
			.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Success("token"));

		_hubSpotClientServiceMock.Setup(service => service
			.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Client);

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(x => x.SaveAsync(_cancellationToken), Times.Once());
		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientFoundButPropertyNameDoesNotMatchAnyCase_ReturnsSuccess(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client Client = new(Guid.NewGuid());

		_unitOfWorkMock.Setup(unitOfWork => unitOfWork.ClientRepository
			.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { Client }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(unitOfWork => unitOfWork.ClientRepository
			.Update(Client), Times.Once());

		_unitOfWorkMock.Verify(unitOfWork => unitOfWork
			.SaveAsync(_cancellationToken), Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
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

		Client Client = new(Guid.NewGuid());

		Client updatedClient = Client;
		updatedClient.Name = command.PropertyValue;

		_unitOfWorkMock.Setup(unitOfWork => unitOfWork.ClientRepository
			.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { Client }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(unitOfWork => unitOfWork.ClientRepository
			.Update(updatedClient), Times.Once());

		_unitOfWorkMock.Verify(unitOfWork => unitOfWork
			.SaveAsync(_cancellationToken), Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "industry", "TestIndustry")]
	public async void Handle_ClientFoundIndustryUpdated_ReturnsSuccessAndClientWasUpdated(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client Client = new(Guid.NewGuid());

		Client updatedClient = Client;
		updatedClient.Segment = command.PropertyValue;

		_unitOfWorkMock.Setup(unitOfWork => unitOfWork.ClientRepository
				.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
				.Returns(new List<Client> { Client }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert

		_unitOfWorkMock.Verify(unitOfWork => unitOfWork.ClientRepository
			.Update(updatedClient), Times.Once());

		_unitOfWorkMock.Verify(unitOfWork => unitOfWork
			.SaveAsync(_cancellationToken), Times.Once());

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

		Client Client = new(Guid.NewGuid());

		Client updatedClient = Client;
		updatedClient.Segment = command.PropertyValue;

		_unitOfWorkMock.Setup(unitOfWork => unitOfWork.ClientRepository
			.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { Client }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(unitOfWork => unitOfWork.ClientRepository
			.Update(updatedClient), Times.Once());

		_unitOfWorkMock.Verify(unitOfWork => unitOfWork
			.SaveAsync(_cancellationToken), Times.Once());

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

		Client Client = new(Guid.NewGuid());

		Client updatedClient = Client;
		updatedClient.OfficeLocation = command.PropertyValue;

		_unitOfWorkMock.Setup(unitOfWork => unitOfWork.ClientRepository
			.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { Client }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(unitOfWork => unitOfWork.ClientRepository
			.Update(updatedClient), Times.Once());

		_unitOfWorkMock.Verify(unitOfWork => unitOfWork
			.SaveAsync(_cancellationToken), Times.Once());

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

		Client Client = new(Guid.NewGuid());

		_unitOfWorkMock.Setup(unitOfWork => unitOfWork.ClientRepository
			.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { Client }.AsQueryable());

		_unitOfWorkMock.Setup(u => u
				.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
				.Returns(Enumerable.Empty<Supplier>().AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);
		//Assert
		_unitOfWorkMock.Verify(unitOfWork => unitOfWork.SupplierRepository
			.FindByCondition(supplier => supplier.HubSpotId == portalId)
			, Times.Once());

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

		Client Client = new(Guid.NewGuid());

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "",
		};

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
			.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { Client }.AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
				.Returns(new List<Supplier> { supplier }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(x => x.SupplierRepository.FindByCondition(s => s.HubSpotId == portalId));
		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedContactNotUpdatedSupplierRefreshTokenNullOnHubSpot_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client Client = new(Guid.NewGuid());

		Supplier supplier = new(Guid.NewGuid());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
			.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { Client }.AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
				.Returns(new List<Supplier> { supplier }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(x => x.SupplierRepository.FindByCondition(s => s.HubSpotId == portalId));
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

		Client Client = new(Guid.NewGuid());

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
			.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { Client }.AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
				.Returns(new List<Supplier> { supplier }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(Result.Failure<string>(new Error("0", "error")));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);
		//Assert
		_hubSpotAuthorizationServiceMock.Verify(service => service
			.RefreshAccessTokenAsync(It.IsAny<string>(), _cancellationToken), Times.Once());

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
		Client Client = new(Guid.NewGuid());

		Supplier supplier = new(Guid.NewGuid())
		{
			RefreshToken = "token",
		};

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository
			.FindByCondition(It.IsAny<Expression<Func<Client, bool>>>()))
			.Returns(new List<Client> { Client }.AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
				.Returns(new List<Supplier> { supplier }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(Result.Success("token"));

		_hubSpotClientServiceMock.Setup(service => service
			.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Client>(Error.NullValue));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);
		//Assert
		_hubSpotClientServiceMock.Verify(
			service => service.GetByIdAsync("token", command.ObjectId, _cancellationToken),
			Times.Once());

		Assert.True(result.IsFailure);
		Assert.Equal(Error.NullValue, result.Error);
	}

	private void MockSetupForNumAssociatedContactsCaseSuccess(Client client, Client HubSpotClient)
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
			.Setup(unitOfWork => unitOfWork
				.SupplierRepository
				.FindByCondition(It.IsAny<Expression<Func<Supplier, bool>>>()))
				.Returns(new List<Supplier> { supplier }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service
				.RefreshAccessTokenAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
				.ReturnsAsync(Result.Success("token"));

		_hubSpotClientServiceMock.Setup(service => service
			.GetByIdAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
			.ReturnsAsync(HubSpotClient);
	}

	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedContactUpdatedButNoClientContactsInClientExisted_ReturnsSuccessAndEmptyClientContacts(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client Client = new(Guid.NewGuid());
		Client ClientHubSpot = new(Guid.NewGuid());
		ClientContact clientContactHubSpot = new(Guid.NewGuid());
		ClientHubSpot.ClientContacts.Add(clientContactHubSpot);
		MockSetupForNumAssociatedContactsCaseSuccess(Client, ClientHubSpot);
		// Act
		Result result = await _uut.Handle(command, _cancellationToken);
		//Assert
		Assert.Empty(Client.ClientContacts);
		_unitOfWorkMock.Verify(x => x.ClientRepository.Update(Client));
		_unitOfWorkMock.Verify(x => x.SaveAsync(_cancellationToken), Times.Once());
		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedContactUpdatedButClientContactsInClientNotTheSameAsInHubSpotClient_ReturnsSuccess(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client client = new(Guid.NewGuid());
		ClientContact clientContact = new()
		{
			IsActive = true,
			HubSpotClientId = Guid.NewGuid().ToString(),
			HubSpotContactId = Guid.NewGuid().ToString()
		};
		client.ClientContacts.Add(clientContact);

		Client ClientHubSpot = new(Guid.NewGuid());
		ClientContact clientContactHubSpot = new()
		{
			HubSpotClientId = Guid.NewGuid().ToString(),
			HubSpotContactId = Guid.NewGuid().ToString()
		};
		ClientHubSpot.ClientContacts.Add(clientContactHubSpot);
		MockSetupForNumAssociatedContactsCaseSuccess(client, ClientHubSpot);

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);
		//Assert
		Assert.False(client.ClientContacts.First().IsActive);
		_unitOfWorkMock.Verify(x => x.ClientRepository.Update(client));
		_unitOfWorkMock.Verify(x => x.SaveAsync(_cancellationToken), Times.Once());
		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "num_associated_contacts", "0")]
	public async void Handle_ClientFoundNumAssociatedContactUpdatedButClientContactsHubSpotClientIdInClientNotTheSameAsInHubSpotClient_ReturnsSuccessAndClientContactIsActiveSetFalse(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client client = new(Guid.NewGuid());
		ClientContact clientContact = new()
		{
			IsActive = true,
			HubSpotClientId = Guid.NewGuid().ToString(),
			HubSpotContactId = Guid.NewGuid().ToString()
		};
		client.ClientContacts.Add(clientContact);

		Client ClientHubSpot = new(Guid.NewGuid());
		ClientContact clientContactHubSpot = new()
		{
			HubSpotClientId = Guid.NewGuid().ToString(),
			HubSpotContactId = client.ClientContacts.First().HubSpotClientId
		};
		ClientHubSpot.ClientContacts.Add(clientContactHubSpot);

		MockSetupForNumAssociatedContactsCaseSuccess(client, ClientHubSpot);
		// Act
		Result result = await _uut.Handle(command, _cancellationToken);
		//Assert
		Assert.False(client.ClientContacts.First().IsActive);
		_unitOfWorkMock.Verify(x => x.ClientRepository.Update(client));
		_unitOfWorkMock.Verify(x => x.SaveAsync(_cancellationToken), Times.Once());
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
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client client = new(Guid.NewGuid());
		ClientContact clientContact = new()
		{
			IsActive = true,
			HubSpotClientId = Guid.NewGuid().ToString(),
			HubSpotContactId = Guid.NewGuid().ToString()
		};
		client.ClientContacts.Add(clientContact);

		Client ClientHubSpot = new(Guid.NewGuid());
		ClientContact clientContactHubSpot = new()
		{
			HubSpotClientId = client.ClientContacts.First().HubSpotClientId,
			HubSpotContactId = client.ClientContacts.First().HubSpotContactId
		};
		ClientHubSpot.ClientContacts.Add(clientContactHubSpot);

		MockSetupForNumAssociatedContactsCaseSuccess(client, ClientHubSpot);

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);
		//Assert
		Assert.True(client.ClientContacts.First().IsActive);
		_unitOfWorkMock.Verify(x => x.ClientRepository.Update(client));
		_unitOfWorkMock.Verify(x => x.SaveAsync(_cancellationToken), Times.Once());
		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}
}
