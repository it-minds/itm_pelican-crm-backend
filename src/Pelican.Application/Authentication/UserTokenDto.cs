
using System.IO;
namespace Pelican.Application.Authentication;

public sealed class UserTokenDto
{
	public UserDto User { get; set; } = new();
	public string Token { get; set; } = string.Empty;
}
