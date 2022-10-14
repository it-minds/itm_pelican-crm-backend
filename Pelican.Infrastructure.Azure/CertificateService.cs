//using System.Security.Cryptography.X509Certificates;
//using Azure.Core;
//using Azure.Identity;
//using Azure.Security.KeyVault.Certificates;
//using Azure.Security.KeyVault.Secrets;
//using Microsoft.Extensions.Options;
//using Pelican.Application.Auth;

//namespace Pelican.Infrastructure.Azure;
//public class CertificateService : ICertificateService
//{
//	private readonly IOptionsMonitor<CertificateSettings> _certSettingsMonitor;
//	private readonly CertificateClient _certClient;
//	private readonly SecretClient _secretClient;
//	public CertificateService(IOptionsMonitor<CertificateSettings> certOptions)
//	{
//		_certSettingsMonitor = certOptions;
//		_certClient = new CertificateClient(new Uri(_certSettingsMonitor.CurrentValue.KeyVaultUrl), GetCredential());
//		_secretClient = new SecretClient(new Uri(_certSettingsMonitor.CurrentValue.KeyVaultUrl), GetCredential());
//	}
//	private TokenCredential GetCredential()
//	{
//		TokenCredential credential;
//		if (_certSettingsMonitor.CurrentValue.UseManagedIdentity)
//		{
//			credential = new ManagedIdentityCredential();
//		}
//		else
//		{
//			credential = new DefaultAzureCredential();
//		}
//		return credential;
//	}
//	public async Task<X509Certificate2> GetGoogleCertificate(CancellationToken cancellationToken = default)
//	{
//		return await GetCertificate(_certSettingsMonitor.CurrentValue.CertificateName, cancellationToken);
//	}
//	protected async Task<X509Certificate2> GetCertificate(string name, CancellationToken cancellationToken = default)
//	{
//		var certResponse = await _certClient.GetCertificateAsync(name, cancellationToken);
//		Uri secretId = certResponse.Value.SecretId;
//		var segments = secretId.Segments;
//		string secretName = segments[2].Trim('/');
//		string version = segments[3].Trim('/');
//		var secretResponse = await _secretClient.GetSecretAsync(secretName, version, cancellationToken);
//		var secret = secretResponse.Value;
//		byte[] privateKeyBytes = Convert.FromBase64String(secret.Value);
//		return new X509Certificate2(privateKeyBytes, (string)null, X509KeyStorageFlags.Exportable);
//	}
//}
