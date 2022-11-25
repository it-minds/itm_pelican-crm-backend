using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Primitives;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.Extensions;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;

namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;

internal sealed class V3HashGenerator : IHashGenerator
{
	private readonly IHashComputer _hashComputer;

	public V3HashGenerator(
		string clientSecret,
		IHashComputerFactory hashComputerFactory)
	{
		_hashComputer = hashComputerFactory.CreateClientSecretHashComputer(clientSecret);
	}

	public string GenerateHash(HttpRequest request)
	{
		bool hasTimestamp = request.Headers.TryGetValue("X-HubSpot-Request-Timestamp", out StringValues timestamp);
		bool hasLongTimestamp = long.TryParse(timestamp, out long longTimestamp);

		if (!hasTimestamp
			|| !hasLongTimestamp
			|| longTimestamp < DateTimeOffset.Now.AddMinutes(-5).ToUnixTimeMilliseconds())
		{
			return string.Empty;
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
