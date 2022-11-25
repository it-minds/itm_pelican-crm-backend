using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;
using Pelican.Infrastructure.HubSpot.Mapping.Contacts;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.Contacts;

public class ContactsResponseToContactsTests
{
	const string ID = "id";
	const string OWNERID = "ownerid";

	readonly ContactResponse response = new()
	{
		Properties = new()
		{
			HubSpotObjectId = ID,
			HubSpotOwnerId = OWNERID,
		}
	};

	readonly ContactsResponse responses = new();

	[Fact]
	public void ToContacts_ArgResultsNull_ThrowException()
	{
		/// Arrange
		responses.Results = null!;

		/// Act
		var result = Record.Exception(() => responses.ToContacts());

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public void ToContacts_ArgResultsNotNull_ThrowNoException()
	{
		/// Arrange 
		responses.Results = new List<ContactResponse>();

		/// Act
		var result = Record.Exception(() => responses.ToContacts());

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public void ToContacts_ArgResultsNotNullNotEmpty_ThrowNoException()
	{
		/// Arrange 
		responses.Results = new List<ContactResponse>() { response };

		/// Act
		var result = Record.Exception(() => responses.ToContacts());

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public void ToContacts_SingleResponse_ReturnSingle()
	{
		/// Arrange
		responses.Results = new List<ContactResponse>() { response };

		/// Act
		var result = responses.ToContacts();

		/// Assert
		Assert.Equal(ID, result.First().HubSpotId);
		Assert.Equal(OWNERID, result.First().HubSpotOwnerId);
	}
}
