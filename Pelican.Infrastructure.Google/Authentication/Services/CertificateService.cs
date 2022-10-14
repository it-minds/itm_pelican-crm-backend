using System.Security.Cryptography.X509Certificates;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Options;
using Pelican.Application.Auth;
using Pelican.Infrastructure.Google.Settings;

namespace Pelican.Infrastructure.Google.Authentication.Services;
public class CertificateService : ICertificateService
{
	private readonly IOptionsMonitor<CertificateSettings> _certSettingsMonitor;
	private readonly CertificateClient _certClient;
	private readonly SecretClient _secretClient;
	public Task<X509Certificate2> GetGoogleCertificate(CancellationToken cancellationToken = default)
	{
		throw new NotImplementedException();
	}
}
