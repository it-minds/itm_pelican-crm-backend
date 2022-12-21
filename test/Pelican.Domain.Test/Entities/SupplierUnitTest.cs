using Bogus;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;
public class SupplierUnitTest
{
	private readonly Supplier _uut;
	public SupplierUnitTest()
	{
		_uut = new Supplier();
	}

	[Fact]
	public void SetName_InputNull_ValueNull()
	{
		// Act
		_uut.Name = null!;

		//Assert
		Assert.Null(_uut.Name);
	}


	[Fact]
	public void SetPhoneNumber_InputNull_ValueNull()
	{
		// Act
		_uut.PhoneNumber = null!;

		//Assert
		Assert.Null(_uut.PhoneNumber);
	}

	[Fact]
	public void SetEmail_InputNull_ValueNull()
	{
		// Act
		_uut.Email = null!;

		//Assert
		Assert.Null(_uut.Email);
	}

	[Fact]
	public void SetLinkedInUrl_InputNull_ValueNull()
	{
		// Act
		_uut.LinkedInUrl = null!;

		//Assert
		Assert.Null(_uut.LinkedInUrl);
	}

	[Fact]
	public void SetWebsiteUrl_InputNull_ValueNull()
	{
		// Act
		_uut.WebsiteUrl = null!;

		//Assert
		Assert.Null(_uut.WebsiteUrl);
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
		Assert.Equal(propertyValue.Substring(0, StringLengths.Name - 3) + "...", _uut.Name);
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
		Assert.Equal(propertyValue.Substring(0, StringLengths.Email - 3) + "...", _uut.Email);
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
		Assert.Equal(propertyValue.Substring(0, StringLengths.PhoneNumber - 3) + "...", _uut.PhoneNumber);
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
		Assert.Equal(propertyValue.Substring(0, StringLengths.Url - 3) + "...", _uut.LinkedInUrl);
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
		Assert.Equal(propertyValue.Substring(0, StringLengths.Url - 3) + "...", _uut.WebsiteUrl);
	}


	[Fact]
	public void SetOfficeLocation_OfficeLocationStringNotToLong_LastNameEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.OfficeLocation);

		// Act
		_uut.OfficeLocation = propertyValue;

		// Assert
		Assert.Equal(StringLengths.OfficeLocation, _uut.OfficeLocation!.Length);
		Assert.Equal(propertyValue, _uut.OfficeLocation);
	}

	[Fact]
	public void SetOfficeLocation_OfficeLocationStringToLong_OfficeLocationShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.OfficeLocation * 2);

		// Act
		_uut.OfficeLocation = propertyValue;

		// Assert
		Assert.Equal(StringLengths.OfficeLocation, _uut.OfficeLocation!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.OfficeLocation - 3) + "...", _uut.OfficeLocation);
	}
}
