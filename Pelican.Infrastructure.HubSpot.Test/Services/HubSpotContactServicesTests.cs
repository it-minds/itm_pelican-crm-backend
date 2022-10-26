//using Moq;
//using Pelican.Infrastructure.HubSpot.Abstractions;
//using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;
//using Pelican.Infrastructure.HubSpot.Services;
//using RestSharp;
//using Xunit;

//namespace Pelican.Infrastructure.HubSpot.Test.Services;

//public class HubSpotContactServicesTests
//{
//	private const string ID = "Id";

//	private readonly Mock<IHubSpotClient> _hubSpotClientMock;
//	private readonly HubSpotContactService _uut;
//	private readonly CancellationToken _cancellationToken;

//	public HubSpotContactServicesTests()
//	{
//		_hubSpotClientMock = new();
//		_cancellationToken = new();
//		_uut = new HubSpotContactService(_hubSpotClientMock.Object);
//	}

//	[Fact]
//	public async Task GetByIdAsync_ClientReturnsFailure_ReturnFailure()
//	{
//		/// Arrange
//		_hubSpotClientMock
//			.Setup(client => client.GetAsync<ContactResponse>(
//				It.IsAny<RestRequest>(),
//				_cancellationToken))
//			.ReturnsAsync(new RestResponse<ContactResponse>()
//			{
//				IsSuccessStatusCode = false,
//			});

//		/// Act
//		var result = await _uut.GetByIdAsync("", 0, _cancellationToken);

//		/// Assert
//		Assert.True(result.IsFailure);
//	}

//	[Fact]
//	public async Task GetByIdAsync_ClientReturnsSuccess_ReturnSuccess()
//	{
//		/// Arrange
//		ContactResponse response = new()
//		{
//			Properties = new()
//			{
//				HubSpotObjectId = ID,
//			},
//		};

//		_hubSpotClientMock
//			.Setup(client => client.GetAsync<ContactResponse>(
//				It.IsAny<RestRequest>(),
//				_cancellationToken))
//			.ReturnsAsync(new RestResponse<ContactResponse>()
//			{
//				IsSuccessStatusCode = true,
//				ResponseStatus = ResponseStatus.Completed,
//				Data = response
//			});

//		/// Act
//		var result = await _uut.GetByIdAsync("", 0, _cancellationToken);

//		/// Assert
//		Assert.True(result.IsSuccess);
//		Assert.Equal("Id", result.Value.HubSpotId);
//	}

//	[Fact]
//	public async Task GetAsync_ClientReturnsFailure_ReturnFailure()
//	{
//		/// Arrange
//		_hubSpotClientMock
//			.Setup(client => client.GetAsync<ContactsResponse>(
//				It.IsAny<RestRequest>(),
//				_cancellationToken))
//			.ReturnsAsync(new RestResponse<ContactsResponse>()
//			{
//				IsSuccessStatusCode = false,
//			});

//		/// Act
//		var result = await _uut.GetAsync("", _cancellationToken);

//		/// Assert
//		Assert.True(result.IsFailure);
//	}

//	[Fact]
//	public async Task GetAsync_ClientReturnsSuccess_ReturnSuccess()
//	{
//		/// Arrange
//		ContactsResponse responses = new()
//		{
//			Results = new ContactResponse[]
//			{
//				new ContactResponse()
//				{
//					Properties = new()
//					{
//						HubSpotObjectId = ID,
//					},
//				},
//			},
//		};

//		_hubSpotClientMock
//			.Setup(client => client.GetAsync<ContactsResponse>(
//				It.IsAny<RestRequest>(),
//				_cancellationToken))
//			.ReturnsAsync(new RestResponse<ContactsResponse>()
//			{
//				IsSuccessStatusCode = true,
//				ResponseStatus = ResponseStatus.Completed,
//				Data = responses
//			});

//		/// Act
//		var result = await _uut.GetAsync("", _cancellationToken);

//		/// Assert
//		Assert.True(result.IsSuccess);
//		Assert.Equal("Id", result.Value.First().HubSpotId);
//	}
//}
