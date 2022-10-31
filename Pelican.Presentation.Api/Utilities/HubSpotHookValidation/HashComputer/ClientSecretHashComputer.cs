using System.Security.Cryptography;
using System.Text;

namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;

internal sealed class ClientSecretHashComputer : IHashComputer
{
	private readonly string _clientSecret;

	public ClientSecretHashComputer(string clientSecret)
	{
		_clientSecret = clientSecret ?? throw new ArgumentNullException(nameof(clientSecret));
	}

	public string ComputeHash(string text)
	{
		using var hmac = new HMACSHA256(Encoding.ASCII.GetBytes(_clientSecret));
		byte[] bytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(text));

		return Convert.ToBase64String(bytes);
	}
}
