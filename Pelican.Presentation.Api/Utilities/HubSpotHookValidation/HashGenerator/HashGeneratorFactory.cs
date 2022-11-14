using Microsoft.Extensions.Options;
using Pelican.Domain.Settings;
using Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashComputer;

namespace Pelican.Presentation.Api.Utilities.HubSpotHookValidation.HashGenerator;

internal sealed class HashGeneratorFactory : IHashGeneratorFactory
{
	private readonly HubSpotSettings _settings;
	public HashGeneratorFactory(
		IOptions<HubSpotSettings> options)
	{
		_settings = options.Value;
	}

	public IHashGenerator CreateHashGenerator(int version) => version switch
	{
		1 => new V1HashGenerator(_settings.App.HubSpotClientSecret, new HashComputerFactory()),
		2 => new V2HashGenerator(_settings.App.HubSpotClientSecret, new HashComputerFactory()),
		3 => new V3HashGenerator(_settings.App.HubSpotClientSecret, new HashComputerFactory()),
		_ => throw new Exception("Unsupported signature version")
	};
}
