namespace Pelican.Application.Abstractions.Infrastructure;

public abstract class ServiceBase<TSettings>
{
	protected readonly IClient<TSettings> _client;

	protected ServiceBase(
		IClient<TSettings> client)
		=> _client = client ?? throw new ArgumentNullException(nameof(client));
}
