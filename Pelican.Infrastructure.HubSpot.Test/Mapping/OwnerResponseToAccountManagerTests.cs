using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;
using Pelican.Infrastructure.HubSpot.Mapping.AccountManagers;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping;

public class OwnerResponseToAccountManagerTests
{
	[Fact]
	public void ToAccountManager_WithAssociations_ReturnClientWithDealsAndClientContacts()
	{
		const string ID = "id";
		const string FIRSTNAME = "fname";
		const string LASTNAME = "lname";
		const string EMAIL = "email";
		const long USERID = 123;

		/// Arrange
		OwnerResponse response = new()
		{

			Id = ID,
			Firstname = FIRSTNAME,
			Lastname = LASTNAME,
			Email = EMAIL,
			UserId = USERID,

		};

		/// Act
		AccountManager result = response.ToAccountManager();

		/// Assert
		Assert.Equal(ID, result.HubSpotId);
		Assert.Equal(FIRSTNAME, result.FirstName);
		Assert.Equal(LASTNAME, result.LastName);
		Assert.Equal(EMAIL, result.Email);
		Assert.Equal(USERID, result.HubSpotUserId);
	}
}
