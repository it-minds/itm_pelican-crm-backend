using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.AccountManagers;
using Pelican.Infrastructure.HubSpot.Mapping.AccountManagers;
using Xunit;

namespace Pelican.Infrastructure.HubSpot.Test.Mapping.AccountManagers;

public class OwnerResponseToAccountManagerTests
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

	[Fact]
	public void ToAccountManager_IdNull_ThrowException()
	{
		/// Arrange 
		response.Id = null!;

		/// Act
		var result = Record.Exception(() => response.ToAccountManager());

		/// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public void ToAccountManager_EmailNull_ThrowException()
	{
		/// Arrange 
		response.Email = null!;

		/// Act
		var result = Record.Exception(() => response.ToAccountManager());

		/// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public void ToAccountManager_FirstnameNull_ThrowException()
	{
		/// Arrange 
		response.Firstname = null!;

		/// Act
		var result = Record.Exception(() => response.ToAccountManager());

		/// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public void ToAccountManager_LastnameNull_ThrowException()
	{
		/// Arrange 
		response.Lastname = null!;

		/// Act
		var result = Record.Exception(() => response.ToAccountManager());

		/// Assert
		Assert.NotNull(result);
	}

	[Fact]
	public void ToAccountManager_ReturnCorrectProperties()
	{
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
