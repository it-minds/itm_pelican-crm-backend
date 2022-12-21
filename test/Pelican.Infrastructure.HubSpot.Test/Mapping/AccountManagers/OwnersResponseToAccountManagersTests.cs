using Pelican.Domain;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Common;
using Pelican.Infrastructure.HubSpot.Mapping.AccountManagers;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.AccountManagers;

public class OwnersResponseToAccountManagersTests
{
	const string ID = "id";
	const string FIRSTNAME = "fname";
	const string LASTNAME = "lname";
	const string EMAIL = "email";
	const long USERID = 123;

	readonly OwnerResponse response = new()
	{
		Id = ID,
		Firstname = FIRSTNAME,
		Lastname = LASTNAME,
		Email = EMAIL,
		UserId = USERID,
	};

	readonly PaginatedResponse<OwnerResponse> responses = new();

	[Fact]
	public void ToAccountManagers_ArgResultsNull_ThrowException()
	{
		/// Arrange
		responses.Results = null!;

		/// Act
		var result = Record.Exception(() => responses.ToAccountManagers());

		/// Assert
		Assert.NotNull(result);

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public void ToAccountManagers_ArgResultsNotNull_ThrowNoException()
	{
		/// Arrange 
		responses.Results = new List<OwnerResponse>();

		/// Act
		var result = Record.Exception(() => responses.ToAccountManagers());

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public void ToAccountManagers_ArgResultsNotNullNotEmpty_ThrowNoException()
	{
		/// Arrange 
		responses.Results = new List<OwnerResponse>() { response };

		/// Act
		var result = Record.Exception(() => responses.ToAccountManagers());

		/// Assert
		Assert.Null(result);
	}

	[Fact]
	public void ToAccountManagers_SingleResponse_ReturnSingle()
	{
		/// Arrange
		responses.Results = new List<OwnerResponse>() { response };

		/// Act
		var result = responses.ToAccountManagers();

		/// Assert
		Assert.Equal(ID, result.First().SourceId);
		Assert.Equal(FIRSTNAME, result.First().FirstName);
		Assert.Equal(LASTNAME, result.First().LastName);
		Assert.Equal(EMAIL, result.First().Email);
		Assert.Equal(USERID, result.First().SourceUserId);
		Assert.Equal(Sources.HubSpot, result.First().Source);
	}
}
