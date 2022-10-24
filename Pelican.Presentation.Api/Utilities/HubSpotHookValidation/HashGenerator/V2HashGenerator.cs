using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.Extensions;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;

namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;

internal sealed class V2HashGenerator : IHashGenerator
{
	private readonly string _clientSecret;
	private readonly IHashComputer _hashComputer;

	public V2HashGenerator(
		string clientSecret,
		IHashComputerFactory hashComputerFactory)
	{
		_clientSecret = clientSecret;
		_hashComputer = hashComputerFactory.CreateSHA256HashComputer();
	}

	public string GenerateHash(HttpRequest request)
	{
		var builder = new StringBuilder(_clientSecret);
		builder.Append(request.Method);
		builder.Append(request.GetEncodedUrl());

		if (request.ContentLength > 0)
		{
			var body = request.GetBody();
			builder.Append(body);
		}

		return _hashComputer.ComputeHash(builder.ToString());
	}
}

