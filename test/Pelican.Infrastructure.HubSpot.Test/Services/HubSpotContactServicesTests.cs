using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings.HubSpot;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;
using Pelican.Infrastructure.HubSpot.Services;
using RestSharp;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Services;

public class HubSpotContactServicesTests
{
	private readonly Mock<IClient<HubSpotSettings>> _hubSpotClientMock = new();
	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
	private readonly HubSpotContactService _uut;

	public HubSpotContactServicesTests()
	{
		_uut = new HubSpotContactService(
			_hubSpotClientMock.Object,
			_unitOfWorkMock.Object);
	}

	[Fact]
	public void HubSpotContactService_ClientNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new HubSpotContactService(null!, _unitOfWorkMock.Object));

		// Assert
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains(
			"client",
			result.Message);
	}

	[Fact]
	public void HubSpotContactService_UnitOfWorkNull_ThrowException()
	{
		// Act
		var result = Record.Exception(() => new HubSpotContactService(_hubSpotClientMock.Object, null!));

		// Assert
		Assert.IsType<ArgumentNullException>(result);
		Assert.Contains(
			"unitOfWork",
			result.Message);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		Mock<IResponse<ContactResponse>> responseMock = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<ContactResponse, IUnitOfWork, CancellationToken, Task<Contact>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<Contact>(Error.NullValue));

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetByIdAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<ContactResponse>> responseMock = new();

		Contact Contact = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<ContactResponse, IUnitOfWork, CancellationToken, Task<Contact>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Contact);

		/// Act
		var result = await _uut.GetByIdAsync("", 0, default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			Contact,
			result.Value);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsFailure_ReturnFailure()
	{
		/// Arrange
		Mock<IResponse<PaginatedResponse<ContactResponse>>> responseMock = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<PaginatedResponse<ContactResponse>>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<PaginatedResponse<ContactResponse>, IUnitOfWork, CancellationToken, Task<List<Contact>>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Result.Failure<List<Contact>>(Error.NullValue));

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<PaginatedResponse<ContactResponse>>> responseMock = new();

		List<Contact> Contacts = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<PaginatedResponse<ContactResponse>>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<PaginatedResponse<ContactResponse>, IUnitOfWork, CancellationToken, Task<List<Contact>>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(Contacts);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			Contacts,
			result.Value);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccessTwice_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<PaginatedResponse<ContactResponse>>> responseMock0 = new();
		Mock<IResponse<PaginatedResponse<ContactResponse>>> responseMock1 = new();

		List<Contact> contacts = new();

		Paging p = new()
		{
			Next = new()
			{
				After = "1",
			}
		};

		responseMock0
			.Setup(r => r.Data)
			.Returns(new PaginatedResponse<ContactResponse>()
			{
				Paging = p,
			});

		_hubSpotClientMock
			.SetupSequence(client => client.GetAsync<PaginatedResponse<ContactResponse>>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock0.Object)
			.ReturnsAsync(responseMock1.Object);

		responseMock0
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<PaginatedResponse<ContactResponse>, IUnitOfWork, CancellationToken, Task<List<Contact>>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(contacts);

		responseMock1
			.Setup(r => r.GetResultWithUnitOfWork(
				It.IsAny<Func<PaginatedResponse<ContactResponse>, IUnitOfWork, CancellationToken, Task<List<Contact>>>>(),
				It.IsAny<IUnitOfWork>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(contacts);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			contacts,
			result.Value);
	}
}
