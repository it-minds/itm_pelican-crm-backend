using Moq;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;
using Pelican.Infrastructure.HubSpot.Services;
using RestSharp;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Services;

public class HubSpotContactServicesTests
{
	private readonly Mock<IClient> _hubSpotClientMock;
	private readonly HubSpotContactService _uut;

	public HubSpotContactServicesTests()
	{
		_hubSpotClientMock = new();
		_uut = new HubSpotContactService(_hubSpotClientMock.Object);
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
			.Setup(r => r.GetResult(It.IsAny<Func<ContactResponse, Contact>>()))
			.Returns(Result.Failure<Contact>(Error.NullValue));

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
			.Setup(r => r.GetResult(It.IsAny<Func<ContactResponse, Contact>>()))
			.Returns(Contact);

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
		Mock<IResponse<ContactsResponse>> responseMock = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactsResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<ContactsResponse, List<Contact>>>()))
			.Returns(Result.Failure<List<Contact>>(Error.NullValue));

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsFailure);
	}

	[Fact]
	public async Task GetAsync_ClientReturnsSuccess_ReturnSuccess()
	{
		/// Arrange
		Mock<IResponse<ContactsResponse>> responseMock = new();

		List<Contact> Contacts = new();

		_hubSpotClientMock
			.Setup(client => client.GetAsync<ContactsResponse>(
				It.IsAny<RestRequest>(),
				It.IsAny<CancellationToken>()))
			.ReturnsAsync(responseMock.Object);

		responseMock
			.Setup(r => r.GetResult(It.IsAny<Func<ContactsResponse, List<Contact>>>()))
			.Returns(Contacts);

		/// Act
		var result = await _uut.GetAsync("", default);

		/// Assert
		Assert.True(result.IsSuccess);
		Assert.Equal(
			Contacts,
			result.Value);
	}
}
