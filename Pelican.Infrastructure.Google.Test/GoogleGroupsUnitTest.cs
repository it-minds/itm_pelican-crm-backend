using Pelican.Infrastructure.Google.Authentication;
using Xunit;
namespace Pelican.Infrastructure.Google.Test;

public class GoogleGroupsUnitTest
{
	[Fact]
	public void Test1()
	{
		//Arrange
		string testMail = "TestMail@it-minds.dk";
		//Act
		//Assert
		Assert.Equal($"{testMail}-google-groups", GoogleGroups.GetGroupCacheStringForUser(testMail));
	}
}
