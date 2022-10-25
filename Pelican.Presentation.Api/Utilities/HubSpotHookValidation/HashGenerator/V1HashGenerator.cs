using System.Text;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.Extensions;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;

namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;

internal sealed class V1HashGenerator : IHashGenerator
{
	private readonly string _clientSecret;
	private readonly IHashComputer _hashComputer;

	public V1HashGenerator(
		string clientSecret,
		IHashComputerFactory hashComputerFactory)
	{
		_clientSecret = clientSecret;
		_hashComputer = hashComputerFactory.CreateSHA256HashComputer();
	}

	public string GenerateHash(HttpRequest request)
	{
		StringBuilder builder = new(_clientSecret);

		if (request.ContentLength > 0)
		{
			string body = request.GetBody();
			builder.Append(body);
		}

		return _hashComputer.ComputeHash(builder.ToString());
	}
}
