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
		_unitOfWorkMock
			.Setup(
				u => u.ClientRepository)
			.Returns(_ClientRepositoryMock.Object);

		_unitOfWorkMock
			.Setup(
				u => u.SupplierRepository)
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
			.Setup(
				u => u
					.ClientRepository
					.FindByCondition(Client => Client.Id.ToString() == command.ObjectId.ToString()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock
			.Setup(
				u => u
					.SupplierRepository
					.FindByCondition(supplier => supplier.HubSpotId == portalId))
			.Returns(Enumerable.Empty<Supplier>().AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			u => u
				.ClientRepository
				.FindByCondition(
					Client => Client.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			u => u
				.SupplierRepository
				.FindByCondition(
					supplier => supplier.HubSpotId == portalId),
			Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal(Error.NullValue, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientNotFoundFailsRefreshingToken_ReturnsFailure(
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
			.Setup(unitOfWork => unitOfWork.ClientRepository.FindByCondition(Client => Client.Id.ToString() == command.ObjectId.ToString()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.SupplierRepository
				.FindByCondition(supplier => supplier.HubSpotId == portalId))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(It.IsAny<string>(), _cancellationToken))
			.ReturnsAsync(Result.Failure<string>(new Error("0", "error")));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.ClientRepository.FindByCondition(
				Client => Client.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.SupplierRepository.FindByCondition(
				supplier => supplier.HubSpotId == portalId),
			Times.Once());

		_hubSpotAuthorizationServiceMock.Verify(
			service => service.RefreshAccessTokenAsync(It.IsAny<string>(), _cancellationToken),
			Times.Once());

		Assert.True(result.IsFailure);

		Assert.Equal("0", result.Error.Code);
		Assert.Equal("error", result.Error.Message);
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientNotFoundSuccessFetchingFromHubSpot_ReturnsFailure(
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

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository.FindByCondition(Client => Client.Id.ToString() == command.ObjectId.ToString()))
			.Returns(Enumerable.Empty<Client>().AsQueryable());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork
				.SupplierRepository
				.FindByCondition(supplier => supplier.HubSpotId == portalId))
			.Returns(new List<Supplier> { supplier }.AsQueryable());

		_hubSpotAuthorizationServiceMock
			.Setup(service => service.RefreshAccessTokenAsync(It.IsAny<string>(), _cancellationToken))
			.ReturnsAsync(Result.Success("token"));

		_hubSpotClientServiceMock
			.Setup(service => service.GetByIdAsync("token", command.ObjectId, _cancellationToken))
			.ReturnsAsync(Result.Success(Client));

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.ClientRepository.FindByCondition(
				Client => Client.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.SupplierRepository.FindByCondition(
				supplier => supplier.HubSpotId == portalId),
			Times.Once());

		_hubSpotClientServiceMock.Verify(
			service => service.GetByIdAsync("token", command.ObjectId, _cancellationToken),
			Times.Once());

		Assert.True(result.IsSuccess);
		Assert.Equal(Error.None, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "0", "0")]
	public async void Handle_ClientFoundNoUpdates_ReturnsFailure(
		long objectId,
		long portalId,
		string propertyName,
		string propertyValue)
	{
		// Arrange
		UpdateClientCommand command = new(objectId, portalId, propertyName, propertyValue);

		Client Client = new(Guid.NewGuid());

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository.FindByCondition(Client => Client.Id.ToString() == command.ObjectId.ToString()))
			.Returns(new List<Client> { Client }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.ClientRepository.FindByCondition(
				Client => Client.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.ClientRepository.Update(Client),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.SaveAsync(_cancellationToken),
			Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}

	[Theory]
	[InlineData(0, 0, "name", "TestCompanyName")]
	public async void Handle_ClientFoundNameUpdated_ReturnsFailure(
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

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository.FindByCondition(Client => Client.Id.ToString() == command.ObjectId.ToString()))
			.Returns(new List<Client> { Client }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.ClientRepository.FindByCondition(
				Client => Client.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.ClientRepository.Update(updatedClient),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.SaveAsync(_cancellationToken),
			Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "industry", "TestIndustry")]
	public async void Handle_ClientFoundIndustryUpdated_ReturnsFailure(
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

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository.FindByCondition(Client => Client.Id.ToString() == command.ObjectId.ToString()))
			.Returns(new List<Client> { Client }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.ClientRepository.FindByCondition(
				Client => Client.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.ClientRepository.Update(updatedClient),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.SaveAsync(_cancellationToken),
			Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "website", "www.testUrl.com")]
	public async void Handle_ClientFoundWebsiteUpdated_ReturnsFailure(
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

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository.FindByCondition(Client => Client.Id.ToString() == command.ObjectId.ToString()))
			.Returns(new List<Client> { Client }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.ClientRepository.FindByCondition(
				Client => Client.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.ClientRepository.Update(updatedClient),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.SaveAsync(_cancellationToken),
			Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}
	[Theory]
	[InlineData(0, 0, "city", "TestCity")]
	public async void Handle_ClientFoundCityUpdated_ReturnsFailure(
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

		_unitOfWorkMock
			.Setup(unitOfWork => unitOfWork.ClientRepository.FindByCondition(Client => Client.Id.ToString() == command.ObjectId.ToString()))
			.Returns(new List<Client> { Client }.AsQueryable());

		// Act
		Result result = await _uut.Handle(command, _cancellationToken);

		// Assert
		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.ClientRepository.FindByCondition(
				Client => Client.Id.ToString() == command.ObjectId.ToString()),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.ClientRepository.Update(updatedClient),
			Times.Once());

		_unitOfWorkMock.Verify(
			unitOfWork => unitOfWork.SaveAsync(_cancellationToken),
			Times.Once());

		Assert.True(result.IsSuccess);

		Assert.Equal(Error.None, result.Error);
	}
}
