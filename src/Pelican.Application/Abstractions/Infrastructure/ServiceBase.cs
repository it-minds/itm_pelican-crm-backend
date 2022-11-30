namespace Pelican.Application.Abstractions.Infrastructure;

public abstract class ServiceBase
{
	protected readonly IClient _client;

	protected ServiceBase(
		IClient client)
		=> _client = client ?? throw new ArgumentNullException(nameof(client));
}
