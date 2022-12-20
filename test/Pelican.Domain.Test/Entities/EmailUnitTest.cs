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
	public void SetParagraph1_Paragraph1StringNotToLong_Paragraph1EqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Paragraph);

		// Act
		_uut.Paragraph1 = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Paragraph, _uut.Paragraph1!.Length);
		Assert.Equal(propertyValue, _uut.Paragraph1);
	}

	[Fact]
	public void SetParagraph1_Paragraph1StringToLong_Paragraph1ShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Paragraph * 2);

		// Act
		_uut.Paragraph1 = propertyValue;

		// Assert

		Assert.Equal(StringLengths.Paragraph, _uut.Paragraph1!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.Paragraph - 3) + "...", _uut.Paragraph1);
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
		Assert.Equal(StringLengths.Heading, _uut.Heading2!.Length);
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

	[Fact]
	public void SetParagraph2_Paragraph2StringNotToLong_Paragraph2EqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Paragraph);

		// Act
		_uut.Paragraph2 = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Paragraph, _uut.Paragraph2!.Length);
		Assert.Equal(propertyValue, _uut.Paragraph2);
	}

	[Fact]
	public void SetParagraph2_Paragraph2StringToLong_Paragraph2ShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Paragraph * 2);

		// Act
		_uut.Paragraph2 = propertyValue;

		// Assert

		Assert.Equal(StringLengths.Paragraph, _uut.Paragraph2!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.Paragraph - 3) + "...", _uut.Paragraph2);
	}

	[Fact]
	public void SetHeading3_Heading3StringNotToLong_Heading3EqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Heading);

		// Act
		_uut.Heading3 = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Heading, _uut.Heading3!.Length);
		Assert.Equal(propertyValue, _uut.Heading3);
	}

	[Fact]
	public void SetHeading3_Heading3StringToLong_Heading3ShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Heading * 2);

		// Act
		_uut.Heading3 = propertyValue;

		// Assert

		Assert.Equal(StringLengths.Heading, _uut.Heading3!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.Heading - 3) + "...", _uut.Heading3);
	}

	[Fact]
	public void SetParagraph3_Paragraph3StringNotToLong_Paragraph3EqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Paragraph);

		// Act
		_uut.Paragraph3 = propertyValue;

		// Assert
		Assert.Equal(StringLengths.Paragraph, _uut.Paragraph3!.Length);
		Assert.Equal(propertyValue, _uut.Paragraph3);
	}

	[Fact]
	public void SetParagraph3_Paragraph3StringToLong_Paragraph3ShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.Paragraph * 2);

		// Act
		_uut.Paragraph3 = propertyValue;

		// Assert

		Assert.Equal(StringLengths.Paragraph, _uut.Paragraph3!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.Paragraph - 3) + "...", _uut.Paragraph3);
	}

	[Fact]
	public void SetCtaButtonText_CtaButtonTextStringNotToLong_CtaButtonTextEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.CtaButtonText);

		// Act
		_uut.CtaButtonText = propertyValue;

		// Assert
		Assert.Equal(StringLengths.CtaButtonText, _uut.CtaButtonText!.Length);
		Assert.Equal(propertyValue, _uut.CtaButtonText);
	}

	[Fact]
	public void SetCtaButtonText_CtaButtonTextStringToLong_CtaButtonTextShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.CtaButtonText * 2);

		// Act
		_uut.CtaButtonText = propertyValue;

		// Assert

		Assert.Equal(StringLengths.CtaButtonText, _uut.CtaButtonText!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.CtaButtonText - 3) + "...", _uut.CtaButtonText);
	}
}
