using Moq;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Domain;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;
using Pelican.Infrastructure.HubSpot.Mapping.Contacts;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Contacts;

public class ContactsResponseToContactsTests
{
	const string ID = "id";
	const string OWNERID = "ownerid";

	private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();

	readonly ContactResponse response = new()
	{
		Properties = new()
		{
			HubSpotObjectId = ID,
			HubSpotOwnerId = OWNERID,
		}
	};

	readonly PaginatedResponse<ContactResponse> responses = new();

	[Fact]
	public async Task ToContacts_ArgResultsNull_ThrowExceptionAsync()
	{
		/// Arrange
		responses.Results = null!;

		/// Act
		var result = await Record.ExceptionAsync(() => responses.ToContacts(_unitOfWorkMock.Object, default));

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public async Task ToContacts_ArgResultsNotNull_ThrowNoExceptionAsync()
	{
		/// Arrange 
		responses.Results = new List<ContactResponse>();

		/// Act
		var result = await Record.ExceptionAsync(() => responses.ToContacts(_unitOfWorkMock.Object, default));

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public async Task ToContacts_ArgResultsNotNullNotEmpty_ThrowNoExceptionAsync()
	{
		/// Arrange 
		responses.Results = new List<ContactResponse>() { response };

		/// Act
		var result = await Record.ExceptionAsync(() => responses.ToContacts(_unitOfWorkMock.Object, default));

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public async void ToContacts_SingleResponse_ReturnSingle()
	{
		/// Arrange
		responses.Results = new List<ContactResponse>() { response };

		/// Act
		var result = await responses.ToContacts(_unitOfWorkMock.Object, default);

		/// Assert
		Assert.Equal(ID, result.First().SourceId);
		Assert.Equal(OWNERID, result.First().SourceOwnerId);
		Assert.Equal(Sources.HubSpot, result.First().Source);
	}
}
