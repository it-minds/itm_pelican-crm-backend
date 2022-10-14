using System.Security.Cryptography.X509Certificates;

namespace Pelican.Application.Auth;
public interface ICertificateService
{
	Task<X509Certificate2> GetGoogleCertificate(CancellationToken cancellationToken = default);
}
