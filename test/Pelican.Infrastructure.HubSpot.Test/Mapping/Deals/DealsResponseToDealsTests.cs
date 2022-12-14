using System.Linq.Expressions;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Deals;

public class DealsResponseToDealsTests
{
	const string ID = "id";
	const string OWNERID = "ownerid";

	readonly DealResponse response = new()
	{
		Properties = new()
		{
			HubSpotObjectId = ID,
			HubSpotOwnerId = OWNERID,
		}
	};

	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

	readonly PaginatedResponse<DealResponse> responses = new();

	[Fact]
	public async Task ToDeals_ArgResultsNull_ThrowException()
	{
		/// Arrange
		responses.Results = null!;

		/// Act
		var result = await Record.ExceptionAsync(() => responses.ToDeals(_unitOfWorkMock.Object, default));

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public async Task ToDeals_ArgResultsNotNull_ThrowNoException()
	{
		/// Arrange 
		responses.Results = new List<DealResponse>();

		/// Act
		var result = await Record.ExceptionAsync(() => responses.ToDeals(_unitOfWorkMock.Object, default));

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task ToDeals_ArgResultsNotNullNotEmpty_ThrowNoException()
	{
		/// Arrange 
		responses.Results = new List<DealResponse>() { response };

		_unitOfWorkMock
			.Setup(u => u
				.AccountManagerRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<AccountManager, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((AccountManager)null!);

		_unitOfWorkMock
			.Setup(u => u
				.ContactRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Contact, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_unitOfWorkMock
			.Setup(u => u
				.ClientRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Client, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((Client)null!);

		/// Act
		var result = await Record.ExceptionAsync(() => responses.ToDeals(_unitOfWorkMock.Object, default));

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task ToDeals_SingleResponse_ReturnSingle()
	{
		/// Arrange
		responses.Results = new List<DealResponse>() { response };

		_unitOfWorkMock
			.Setup(u => u
				.AccountManagerRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<AccountManager, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((AccountManager)null!);

		_unitOfWorkMock
			.Setup(u => u
				.ContactRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Contact, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((Contact)null!);

		_unitOfWorkMock
			.Setup(u => u
				.ClientRepository
				.FirstOrDefaultAsync(
					It.IsAny<Expression<Func<Client, bool>>>(),
					It.IsAny<CancellationToken>()))
			.ReturnsAsync((Client)null!);

		/// Act
		var result = await responses.ToDeals(_unitOfWorkMock.Object, default);

		/// Assert
		Assert.Equal(ID, result.First().SourceId);
		Assert.Equal(OWNERID, result.First().SourceOwnerId);
		Assert.Equal(Sources.HubSpot, result.First().Source);
	}
}
