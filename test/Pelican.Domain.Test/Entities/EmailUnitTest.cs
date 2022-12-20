using Bogus;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;
public class EmailUnitTest
{
	private readonly Email _uut = new();

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
	public void SetSubject_SubjectStringNotToLong_SubjectEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.SubjectLine);

		// Act
		_uut.Subject = propertyValue;

		// Assert
		Assert.Equal(StringLengths.SubjectLine, _uut.Subject!.Length);
		Assert.Equal(propertyValue, _uut.Subject);
	}

	[Fact]
	public void SetSubject_SubjectStringToLong_SubjectShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.SubjectLine * 2);

		// Act
		_uut.Subject = propertyValue;

		// Assert

		Assert.Equal(StringLengths.SubjectLine, _uut.Subject!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.SubjectLine - 3) + "...", _uut.Subject);
	}

	[Fact]
	public void SetHeading1_Heading1StringNotToLong_Heading1EqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Heading);

		// Act
		_uut.Heading1 = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Heading, _uut.Heading1!.Length);
		Assert.Equal(propertyValue, _uut.Heading1);
	}

	[Fact]
	public void SetHeading1_Heading1StringToLong_Heading1ShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Heading * 2);

		// Act
		_uut.Heading1 = propertyValue;

		// Assert

		Assert.Equal(StringLengths.Heading, _uut.Heading1!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.Heading - 3) + "...", _uut.Heading1);
	}

	[Fact]
	public void SetHeading2_Heading2StringNotToLong_Heading2EqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Heading);

		// Act
		_uut.Heading2 = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Heading, _uut.Heading1!.Length);
		Assert.Equal(propertyValue, _uut.Heading2);
	}

	[Fact]
	public void SetHeading2_Heading2StringToLong_Heading2ShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Heading * 2);

		// Act
		_uut.Heading2 = propertyValue;

		// Assert

		Assert.Equal(StringLengths.Heading, _uut.Heading2!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.Heading - 3) + "...", _uut.Heading2);
	}
}
