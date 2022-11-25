using Bogus;
using Pelican.Domain.Entities;
using Xunit;

namespace Pelican.Domain.Test.Entities;
public class LocationUnitTest
{
	private readonly Location _uut;

	public LocationUnitTest()
	{
		_uut = new Location(Guid.NewGuid());
	}

	[Fact]
	public void SetCityName_CityNameStringNotToLong_CityNameEqualToValueSet()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.OfficeLocation);

		// Act
		_uut.CityName = propertyValue;

		// Assert
		Assert.Equal(StringLengths.OfficeLocation, _uut.CityName!.Length);
	}

	[Fact]
	public void SetCityName_CityNameStringToLong_CityNameShortenedAndAppendedWithThreeDots()
	{
		// Arrange
		Faker faker = new();
		string propertyValue = faker.Lorem.Letter(StringLengths.OfficeLocation * 2);

		// Act
		_uut.CityName = propertyValue;
		// Assert
		Assert.Equal(StringLengths.OfficeLocation, _uut.CityName!.Length);
		Assert.Equal(propertyValue.Substring(0, StringLengths.OfficeLocation - 3) + "...", _uut.CityName);
	}
}
