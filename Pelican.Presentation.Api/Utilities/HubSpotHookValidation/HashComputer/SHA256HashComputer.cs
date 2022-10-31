using System.Security.Cryptography;
using System.Text;

namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;

internal sealed class SHA256HashComputer : IHashComputer
{
	public string ComputeHash(string text)
	{
		using var sha = SHA256.Create();
		byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(text));

		var hashBuilder = new StringBuilder();

		foreach (var b in bytes)
		{
			hashBuilder.Append(b.ToString("x2"));
		}

		return hashBuilder.ToString();
	}
}
