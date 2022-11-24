using Bogus;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;
public class AccountManagerUnitTest
{
	private readonly AccountManager _uut;
	public AccountManagerUnitTest()
	{
		_uut = new AccountManager(Guid.NewGuid());
	}

	[Fact]
	public void SetFirstName_FirstNameStringNotToLong_FirstnameEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Email);

		// Act
		_uut.FirstName = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Name, _uut.FirstName!.Length);
		Assert.Equal(propertyValue, _uut.FirstName);
	}

	[Fact]
	public void SetLastName_LastNameStringNotToLong_LastNameEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Email);

		// Act
		_uut.LastName = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Name, _uut.LastName!.Length);
		Assert.Equal(propertyValue, _uut.LastName);
	}

	[Fact]
	public void SetEmail_EmailStringNotToLong_EmailEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Email);

		// Act
		_uut.Email = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Email, _uut.Email!.Length);
		Assert.Equal(propertyValue, _uut.Email);
	}

	[Fact]
	public void SetPhoneNumber_PhoneNumberStringNotToLong_PhoneNumberEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.PhoneNumber);

		// Act
		_uut.PhoneNumber = propertyValue;

		// Assert
		Assert.Equal(StringLengths.PhoneNumber, _uut.PhoneNumber!.Length);
		Assert.Equal(propertyValue, _uut.PhoneNumber);
	}

	[Fact]
	public void SetPictureUrl_PictureUrlStringNotToLong_PictureUrlEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Url);

		// Act
		_uut.PictureUrl = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Url, _uut.PictureUrl!.Length);
		Assert.Equal(propertyValue, _uut.PictureUrl);
	}

	[Fact]
	public void SetLinkedInUrl_LinkedInUrlStringNotToLong_LinkedInUrlEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Url);

		// Act
		_uut.LinkedInUrl = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Url, _uut.LinkedInUrl!.Length);
		Assert.Equal(propertyValue, _uut.LinkedInUrl);
	}
	[Fact]
	public void SetFirstName_FirstNameStringToLong_FirstnameShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Name * 2);

		// Act
		_uut.FirstName = propertyValue;

		// Assert

		Assert.Equal(StringLengths.Name, _uut.FirstName!.Length);
		Assert.Equal("...", _uut.FirstName.Substring(StringLengths.Name - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.Name - 3), _uut.FirstName.Substring(0, StringLengths.Name - 3));
	}

	[Fact]
	public void SetLastName_LastNameStringToLong_LastNameShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Name * 2);

		// Act
		_uut.LastName = propertyValue;
		// Assert
		Assert.Equal(StringLengths.Name, _uut.LastName!.Length);
		Assert.Equal("...", _uut.LastName.Substring(StringLengths.Name - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.Name - 3), _uut.LastName.Substring(0, StringLengths.Name - 3));
	}

	[Fact]
	public void SetEmail_EmailStringToLong_EmailShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Email * 2);

		// Act
		_uut.Email = propertyValue;
		// Assert
		Assert.Equal(StringLengths.Email, _uut.Email!.Length);
		Assert.Equal("...", _uut.Email.Substring(StringLengths.Email - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.Email - 3), _uut.Email.Substring(0, StringLengths.Email - 3));
	}

	[Fact]
	public void SetPhoneNumber_PhoneNumberStringToLong_PhoneNumberShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.PhoneNumber * 2);

		// Act
		_uut.PhoneNumber = propertyValue;

		// Assert
		Assert.Equal(StringLengths.PhoneNumber, _uut.PhoneNumber!.Length);
		Assert.Equal("...", _uut.PhoneNumber.Substring(StringLengths.PhoneNumber - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.PhoneNumber - 3), _uut.PhoneNumber.Substring(0, StringLengths.PhoneNumber - 3));
	}
	[Fact]
	public void SetPictureUrl_PictureUrlStringToLong_PictureInUrlShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Url * 2);

		// Act
		_uut.PictureUrl = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Url, _uut.PictureUrl!.Length);
		Assert.Equal("...", _uut.PictureUrl.Substring(StringLengths.Url - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.Url - 3), _uut.PictureUrl.Substring(0, StringLengths.Url - 3));
	}

	[Fact]
	public void SetLinkedInUrl_LinkedInUrlStringToLong_LinkedInUrlShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Url * 2);

		// Act
		_uut.LinkedInUrl = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Url, _uut.LinkedInUrl!.Length);
		Assert.Equal("...", _uut.LinkedInUrl.Substring(StringLengths.Url - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.Url - 3), _uut.LinkedInUrl.Substring(0, StringLengths.Url - 3));
	}
}
