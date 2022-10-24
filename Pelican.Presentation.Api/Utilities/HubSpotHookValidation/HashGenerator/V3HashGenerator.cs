using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.Extensions;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;

namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;

internal sealed class V3HashGenerator : IHashGenerator
{
	private readonly string _clientSecret;
	private readonly IHashComputer _hashComputer;

	public V3HashGenerator(
		string clientSecret,
		IHashComputerFactory hashComputerFactory)
	{
		_clientSecret = clientSecret;
		_hashComputer = hashComputerFactory.CreateClientSecretHashComputer(clientSecret);
	}

	public string GenerateHash(HttpRequest request)
	{
		bool hasTimestamp = request.Headers.TryGetValue("X-HubSpot-Request-Timestamp", out StringValues timestamp);
		if (!hasTimestamp
			|| Convert.ToInt64(timestamp) < DateTimeOffset.Now.AddMinutes(-5).ToUnixTimeMilliseconds())
		{
			return "";
		}

		StringBuilder builder = new(request.Method);
		builder.Append(request.GetEncodedUrl());

		if (request.ContentLength > 0)
		{
			string body = request.GetBody();
			builder.Append(body);
		}

		builder.Append(timestamp);

		return _hashComputer.ComputeHash(builder.ToString());
	}
}
