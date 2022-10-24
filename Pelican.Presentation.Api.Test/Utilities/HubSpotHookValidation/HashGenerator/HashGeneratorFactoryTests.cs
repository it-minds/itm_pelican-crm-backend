using Microsoft.Extensions.Options;
using Moq;
using Pelican.Infrastructure.HubSpot.Settings;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;
using Xunit;

namespace Pelican.Presentation.Api.Test.Utilities.HubSpotHookValidation.HashGenerator;

public class HashGeneratorFactoryTests
{
	private readonly Mock<IOptions<HubSpotSettings>> _optionsMock;
	private readonly HashGeneratorFactory _uut;

	public HashGeneratorFactoryTests()
	{
		_optionsMock = new();

		_optionsMock
			.Setup(option => option.Value)
			.Returns(new HubSpotSettings()
			{
				App = new HubSpotAppSettings()
				{
					ClientSecret = "clientsecret",
				}
			});

		_uut = new(_optionsMock.Object);
	}

	[Fact]
	public void GetHashGenerator_UnsupportedVersion_ThrowError()
	{
		/// Act
		var exception = Record.Exception(() => _uut
			.GetHashGenerator(9));

		/// Assert
		Assert.Equal(
			new Exception("Unsupported signature version").Message,
			exception.Message);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(3)]
	public void GetHashGenerator_SupportedVersion_ThrowNoError(int version)
	{
		/// Act
		var exception = Record.Exception(() => _uut
			.GetHashGenerator(version));

		/// Assert
		Assert.Null(exception);
	}

	[Fact]
	public void GetHashGenerator_Version1_ThrowNoError()
	{
		/// Act
		var generator = _uut
			.GetHashGenerator(1);

		/// Assert
		Assert.Equal(
		 typeof(V1HashGenerator),
		 generator.GetType());
	}

	[Fact]
	public void GetHashGenerator_Version2_ThrowNoError()
	{
		/// Act
		var generator = _uut
			.GetHashGenerator(2);

		/// Assert
		Assert.Equal(
		 typeof(V2HashGenerator),
		 generator.GetType());
	}

	[Fact]
	public void GetHashGenerator_Version3_ThrowNoError()
	{
		/// Act
		var generator = _uut
			.GetHashGenerator(3);

		/// Assert
		Assert.Equal(
		 typeof(V3HashGenerator),
		 generator.GetType());
	}



}
