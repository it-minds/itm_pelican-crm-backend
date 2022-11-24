using Bogus;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;
public class SupplierUnitTest
{
	private readonly Supplier _uut;
	public SupplierUnitTest()
	{
		_uut = new Supplier(Guid.NewGuid());
	}
	[Fact]
	public void SetName_NameStringNotToLong_NameEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Name);

		// Act
		_uut.Name = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Name, _uut.Name!.Length);
		Assert.Equal(propertyValue, _uut.Name);
	}

	[Fact]
	public void SetRefreshToken_RefreshTokenStringNotToLong_RefreshTokenEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Token);

		// Act
		_uut.RefreshToken = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Token, _uut.RefreshToken!.Length);
		Assert.Equal(propertyValue, _uut.RefreshToken);
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
	public void SetPictureUrl_PictureUrlStringNotToLong_PictureUrlEqualeToValueSet()
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
	public void SetWebsiteUrl_WebsiteUrlStringNotToLong_WebsiteUrlEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Url);

		// Act
		_uut.WebsiteUrl = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Url, _uut.WebsiteUrl!.Length);
		Assert.Equal(propertyValue, _uut.WebsiteUrl);
	}
	[Fact]
	public void SetName_NameStringToLong_NameShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Name * 2);

		// Act
		_uut.Name = propertyValue;

		// Assert

		Assert.Equal(StringLengths.Name, _uut.Name!.Length);
		Assert.Equal("...", _uut.Name.Substring(StringLengths.Name - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.Name - 3), _uut.Name.Substring(0, StringLengths.Name - 3));
	}

	[Fact]
	public void SetRefreshToken_RefreshTokenStringToLong_RefreshTokenShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Token * 2);

		// Act
		_uut.RefreshToken = propertyValue;
		// Assert
		Assert.Equal(StringLengths.Token, _uut.RefreshToken!.Length);
		Assert.Equal("...", _uut.RefreshToken.Substring(StringLengths.Token - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.Token - 3), _uut.RefreshToken.Substring(0, StringLengths.Token - 3));
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

	[Fact]
	public void SetWebsiteUrl_WebsiteUrlStringToLong_WebsiteUrlShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Url * 2);

		// Act
		_uut.WebsiteUrl = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Url, _uut.WebsiteUrl!.Length);
		Assert.Equal("...", _uut.WebsiteUrl.Substring(StringLengths.Url - 3));
		Assert.Equal(propertyValue.Substring(0, StringLengths.Url - 3), _uut.WebsiteUrl.Substring(0, StringLengths.Url - 3));
	}
}
