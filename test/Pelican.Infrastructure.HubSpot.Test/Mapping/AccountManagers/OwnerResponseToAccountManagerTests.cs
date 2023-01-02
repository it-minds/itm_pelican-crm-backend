using Bogus;
using Pelican.Domain;
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

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
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

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
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

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
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

		Assert.Equal(
			typeof(ArgumentNullException),
			result.GetType());
	}

	[Fact]
	public void ToAccountManager_ReturnCorrectProperties()
	{
		/// Act
		AccountManager result = response.ToAccountManager();

		/// Assert
		Assert.Equal(ID, result.SourceId);
		Assert.Equal(FIRSTNAME, result.FirstName);
		Assert.Equal(LASTNAME, result.LastName);
		Assert.Equal(EMAIL, result.Email);
		Assert.Equal(USERID, result.SourceUserId);
		Assert.Equal(Sources.HubSpot, result.Source);
	}
	[Fact]
	public void ToAccountManager_FirstNameStringTooLong_FirstNameShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Firstname = faker.Lorem.Letter(StringLengths.Name * 2);

		/// Act
		AccountManager result = response.ToAccountManager();

		/// Assert
		Assert.Equal(StringLengths.Name, result.FirstName.Length);
		Assert.Equal("...", result.FirstName.Substring(StringLengths.Name - 3));
		Assert.Equal(response.Firstname.Substring(0, StringLengths.Name - 3), result.FirstName.Substring(0, StringLengths.Name - 3));
	}

	[Fact]
	public void ToAccountManager_LastNameStringTooLong_LastNameShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Lastname = faker.Lorem.Letter(StringLengths.Name * 2);

		/// Act
		AccountManager result = response.ToAccountManager();

		/// Assert
		Assert.Equal(StringLengths.Name, result.LastName.Length);
		Assert.Equal("...", result.LastName.Substring(StringLengths.Name - 3));
		Assert.Equal(response.Lastname.Substring(0, StringLengths.Name - 3), result.LastName.Substring(0, StringLengths.Name - 3));
	}

	[Fact]
	public void ToAccountManager_EmailStringTooLong_EmailShortenededAndAppendedWithThreeDots()
	{
		Faker faker = new();
		response.Email = faker.Lorem.Letter(StringLengths.Email * 2);

		/// Act
		AccountManager result = response.ToAccountManager();

		/// Assert
		Assert.Equal(StringLengths.Email, result.Email.Length);
		Assert.Equal("...", result.Email.Substring(StringLengths.Email - 3));
		Assert.Equal(response.Email.Substring(0, StringLengths.Email - 3), result.Email.Substring(0, StringLengths.Email - 3));
	}
}
