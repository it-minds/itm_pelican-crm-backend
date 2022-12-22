using Microsoft.Extensions.Options;
using Pelican.Application.Abstractions.Authentication;
using Pelican.Application.Options;
using Pelican.Application.Security;
using Xunit;

namespace Pelican.Application.Test.Security;
public class PasswordHasherUnitTest
{
	IOptions<HashingOptions> _options = Microsoft.Extensions.Options.Options.Create(new HashingOptions());
	private IPasswordHasher _uut;

	public PasswordHasherUnitTest()
	{
		_uut = new PasswordHasher(_options);
	}

	[Fact]
	public void Hash_PasswordHashedAndIncorrectFormat_PasswordContainsIterations10000AndCanBeSplitInto3BySplittingByPeriod()
	{
		//Arrange
		string testPassword = "testPassword";

		//Act
		var result = _uut.Hash(testPassword);

		//Assert
		Assert.Contains(_options.Value.Iterations.ToString(), result);
		Assert.Equal(3, result.Split(".").Length);
	}

	[Fact]
	public void Check_IncorrectPasswordFormat_ThrowsFormatExceptionWithCorrectMessage()
	{
		//Arrange
		string testPassword = "testPassword";


		//Act
		var result = Record.Exception(() => _uut.Check(testPassword, testPassword));

		//Assert
		Assert.IsType<FormatException>(result);
		Assert.Contains("Unexpected hash format. " +
				"Should be formatted as '{iterations}.{salt}.{hash}'", result.Message);
	}

	[Fact]
	public void Check_IncorrectPasswordInputted_VerifiedFalseAndDoesNeedsUpgradeFalse()
	{
		//Arrange
		string testPassword = "testPassword";
		var hash = _uut.Hash(testPassword);

		//Act
		var result = _uut.Check(hash, "notTestPassword");

		//Assert
		Assert.False(result.Verified);
		Assert.False(result.NeedsUpgrade);
	}

	[Fact]
	public void Check_InCorrectPasswordInputtedAndInCorrectIterations_VerifiedFalseAndNeedsUpgradeTrue()
	{
		//Arrange
		string testPassword = "testPassword";
		var hash = _uut.Hash(testPassword);
		IOptions<HashingOptions> oldIterationOptions = Microsoft.Extensions.Options.Options.Create(new HashingOptions() { Iterations = 10 });
		_uut = new PasswordHasher(oldIterationOptions);

		//Act
		var result = _uut.Check(hash, "notTestPassword");

		//Assert
		Assert.False(result.Verified);
		Assert.True(result.NeedsUpgrade);
	}

	[Fact]
	public void Check_CorrectPasswordInputtedButIncorrectIterations_VerifiedTrueAndNeedsUpgradeTrue()
	{
		//Arrange
		string testPassword = "testPassword";
		var hash = _uut.Hash(testPassword);
		IOptions<HashingOptions> oldIterationOptions = Microsoft.Extensions.Options.Options.Create(new HashingOptions() { Iterations = 10 });
		_uut = new PasswordHasher(oldIterationOptions);

		//Act
		var result = _uut.Check(hash, testPassword);

		//Assert
		Assert.True(result.Verified);
		Assert.True(result.NeedsUpgrade);
	}

	[Fact]
	public void Check_CorrectPasswordAndCorrectIterationsInputted_VerifiedTrueAndNeedsUpgradeFalse()
	{
		//Arrange
		string testPassword = "testPassword";
		var hash = _uut.Hash(testPassword);

		//Act
		var result = _uut.Check(hash, testPassword);

		//Assert
		Assert.True(result.Verified);
		Assert.False(result.NeedsUpgrade);
	}
}
